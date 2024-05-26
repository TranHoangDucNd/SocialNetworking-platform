import {
  AfterViewInit,
  Component,
  ElementRef,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { NgForm } from '@angular/forms';
import { HubConnection } from '@microsoft/signalr';
import { take } from 'rxjs';
import { CreateMessageDto } from 'src/app/_models/message';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_service/account.service';
import { MessageService } from 'src/app/_service/message.service';
import $ from 'jquery';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from 'src/app/messages/confirm-dialog/confirm-dialog.component';
import { MembersService } from 'src/app/_service/members.service';
import { PresenceService } from 'src/app/_service/presence.service';
@Component({
  selector: 'app-messages-thread',
  templateUrl: './messages-thread.component.html',
  styleUrls: ['./messages-thread.component.css'],
})
export class MessagesThreadComponent
  implements OnInit, OnChanges, AfterViewInit, OnDestroy
{
  @ViewChild('messageForm') messageForm?: NgForm;
  @Input() username!: string;
  @Input() knownAs!: string;
  @Input() imgPath!: string;
  user!: User;
  messageContent = '';
  loading = false;
  @ViewChild('messageContainer') private messageContainer!: ElementRef;

  private hubConnection?: HubConnection;
  createMessage: CreateMessageDto;
  formData: FormData;
  selectedImage?: File | null;
  imagePreview: string | ArrayBuffer | null = null;

  constructor(
    public messageService: MessageService,
    private accountService: AccountService,
    private toastr: ToastrService,public dialog: MatDialog, 
    private memberService: MembersService,
    public presenceService: PresenceService
  ) {
    this.formData = new FormData();
    this.createMessage = {
      recipientUsername: '',
      content: '',
      url: null,
      publicId: null,
    };
  }

  ngOnInit(): void {
    $('#action_menu_btn').click(function () {
      $('.action_menu').toggle();
    });
    // Lấy thông tin người dùng hiện tại
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) {
          this.user = user;
        }
      },
    });
  }

  ngAfterViewInit(): void {
    this.scrollToBottom();
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Kiểm tra nếu giá trị username đã thay đổi
    if (changes['username']) {
      // Nếu username đã được truyền vào từ component cha, gọi lại hàm tạo kết nối
      this.createHubConnection();
    }
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  // Phương thức tạo kết nối SignalR
  createHubConnection(): void {
    if (this.user && this.username) {
      // Tạo kết nối SignalR với thông tin người dùng và username
      this.messageService.createHubConnection(this.user, this.username);
      this.messageService.messageThread$.subscribe(() => {
        this.scrollToBottom();
      });
    } else {
      this.stopHubConnection();
    }
  }

  AddImage(event: Event) {
    const selectedFile = event.target as HTMLInputElement;
    if (selectedFile.files && selectedFile.files.length > 0) {
      this.selectedImage = selectedFile.files[0];
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result!;
      };
      reader.readAsDataURL(this.selectedImage);
    }
  }

  onSubmit() {
    this.createMessage.content = this.messageContent;
    this.createMessage.recipientUsername = this.username!;
    if (this.selectedImage) {
      this.formData.append('file', this.selectedImage);
      this.messageService
        .upLoadImageMessage(this.formData)
        .subscribe((response) => {
          this.createMessage.url = response.path;
          this.createMessage.publicId = response.publicId;
          this.sendMessage();
        });
    } else {
      this.sendMessage();
    }
  }

  private sendMessage() {
    this.messageService
      .sendMessage(this.createMessage)
      .then(() => {
        this.messageForm?.reset();
        this.selectedImage = null;
        this.imagePreview = null;
        this.createMessage = {
          recipientUsername: '',
          content: '',
          url: null,
          publicId: null,
        };
        this.formData = new FormData();
      })
      .finally(() => (this.loading = false));
  }
  stopHubConnection() {
    //nếu có kết nối ms stop
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }

  //Delete all messages
  deleteAllMessages(otherUsername: string) {
    this.messageService.deleteAllMessage(otherUsername).subscribe({
      next: (_) => {
        this.toastr.success('Deleted successfully');
      },
      error: (err) => {
        this.toastr.error('Delete failed');
      },
    });
  }

  openconfirmDialog(otherUsername: string, event: Event){
    event.stopPropagation();
    const dialogRef = this.dialog.open(ConfirmDialogComponent);

    dialogRef.afterClosed().subscribe(result =>{
      if(result){
        this.deleteAllMessages(otherUsername);
      }
    })
  }

  //Follow
  addLike(username: string){
    this.memberService.addLike(username).subscribe({
      next: () =>{
        this.toastr.success("Successfully");
      },
      error: error => this.toastr.error(error.error)
    })
  }

  private scrollToBottom(): void {
    try {
      setTimeout(() => {
        this.messageContainer.nativeElement.scrollTop =
          this.messageContainer.nativeElement.scrollHeight;
      }, 0);
    } catch (err) {
      console.error('Scroll to bottom failed:', err);
    }
  }
}
