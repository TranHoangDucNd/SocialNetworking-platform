import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { getPaginationHeaders, getPaginationResult } from './paginationHelper';
import { CreateMessageDto, Message, MessagesForUser, UploadImageMess } from '../_models/message';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take, tap } from 'rxjs';
import { User } from '../_models/user';
import { Group } from '../_models/group';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();

  //xay dung cho last message
  private messageListSource = new BehaviorSubject<MessagesForUser[]>([]);
  messageList$ = this.messageListSource.asObservable();
  private messages: MessagesForUser[] = [];

  constructor(private http: HttpClient) {}

  createHubConnection(user: User, otherUsername: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?user=' + otherUsername, {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build();
    this.hubConnection.start().catch((error) => console.log(error));

    this.hubConnection.on('ReceiveMessageThread', (messages) => {
      this.messageThreadSource.next(messages);
    });

    this.hubConnection.on('UpdateUI', (message) => {
      this.updateMessageList(message);
    });


    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      if (group.connections.some((x) => x.username === otherUsername)) {
        this.messageThread$.pipe(take(1)).subscribe({
          next: (messages) => {
            messages.forEach((message) => {
              if (!message.dateRead) {
                message.dateRead = new Date(Date.now());
              }
            });

            this.messageThreadSource.next([...messages]);
          },
        });
      }
    });

    this.hubConnection.on('NewMessage', (message) => {
      this.messageThread$.pipe(take(1)).subscribe({
        next: (messages) => {
          this.messageThreadSource.next([...messages, message]); //lấy tin nhắn hiện có bằng toán tử mở rộng và thêm tin nhắn mới vào
        },
      });
    });
  }

  stopHubConnection() {
    //nếu có kết nối ms stop
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }

  getMessage(pageNumber: number, pageSize: number, container: string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);
    return getPaginationResult<Message[]>(
      this.baseUrl + 'messages',
      params,
      this.http
    );
  }

  getMessageThread(username: string) {
    return this.http.get<Message[]>(
      this.baseUrl + 'messages/thread/' + username
    );
  }

  async sendMessage(createMessage: CreateMessageDto) {
    //Lấy đúng tên method   public async Task SendMessage(CreateMessageDto createMessageDto)
    return this.hubConnection
      ?.invoke('SendMessage', createMessage)
      .catch((error) => console.log(error));
  }

  upLoadImageMessage(formData: FormData){
    return this.http.post<UploadImageMess>(this.baseUrl + 'messages/upload-image-message', formData);
  }

  deleteAllMessage(otherUsername: string){
    return this.http.delete(this.baseUrl + 'messages/delete-messages-by-username/' + otherUsername);
  }

  //New messages
  getMessagesForUser(){
    return this.http.get<MessagesForUser[]>(this.baseUrl + 'messages/get-messages-for-user').pipe(
      tap(messages => {
        this.messages = messages;
        this.messageListSource.next(messages);
      })
    )
  }

  private updateMessageList(message: MessagesForUser) {
    // Lấy danh sách tin nhắn hiện tại
    let currentMessages = this.messageListSource.value;
    // Tìm tin nhắn cũ của người gửi hoặc người nhận trong danh sách
    const index = currentMessages.findIndex(m => m.username === message.username);
    // Nếu tìm thấy, cập nhật tin nhắn
    if (index !== -1) {
      currentMessages[index] = message;
    } else {
      // Nếu không tìm thấy, thêm tin nhắn mới vào danh sách
      currentMessages.push(message);
    }
    // Cập nhật lại danh sách tin nhắn
    this.messageListSource.next(currentMessages);
  }

}
