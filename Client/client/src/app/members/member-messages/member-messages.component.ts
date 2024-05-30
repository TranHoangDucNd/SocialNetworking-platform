import { CommonModule } from '@angular/common';
import { AfterViewInit, ChangeDetectionStrategy, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TimeagoModule } from 'ngx-timeago';
import { CreateMessageDto, Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_service/message.service';
import { MatIconModule } from '@angular/material/icon';
@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-member-messages',
  standalone: true,
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css'],
  imports: [CommonModule, TimeagoModule, FormsModule, BsDropdownModule,MatIconModule],
})
export class MemberMessagesComponent implements OnInit,AfterViewInit {
  @ViewChild('messageForm') messageForm?: NgForm;
  @Input() username?: string;
  messageContent = '';
  loading = false;
  createMessage: CreateMessageDto;
  @ViewChild('messageContainer') private messageContainer!: ElementRef;
  formData: FormData;
  selectedImage?: File | null;
  imagePreview: string | ArrayBuffer |  null = null;

  constructor(public messageService: MessageService) {
    this.formData = new FormData();
    this.createMessage = {
      recipientUsername: '',
      content: '',
      url: null,
      publicId: null
    }
  }
  ngOnInit(): void {
    this.messageService.messageThread$.subscribe(() => {
      this.scrollToBottom();
    });
  }

  AddImage(event: Event) {
    const selectedFile = event.target as HTMLInputElement;
    if (selectedFile.files && selectedFile.files.length > 0) {

        this.selectedImage = selectedFile.files[0];
        const reader = new FileReader();
        reader.onload = () => {
          this.imagePreview = reader.result!;
        }
        reader.readAsDataURL(this.selectedImage);
      }
    }

  
  onSubmit(){
      this.createMessage.content = this.messageContent;
      this.createMessage.recipientUsername = this.username!;
      if(this.selectedImage){
        this.formData.append('file', this.selectedImage);
        this.messageService.upLoadImageMessage(this.formData).subscribe(
            response =>{
              this.createMessage.url = response.path;
              this.createMessage.publicId = response.publicId;
              this.sendMessage();
            }
        )
      }else{
        this.sendMessage();
      }
    }
  

  private sendMessage() {
    this.messageService
      .sendMessage(this.createMessage)
      .then(() => {
        this.messageForm?.reset();
        this.selectedImage = null
        this.imagePreview = null
        this.createMessage = {
          recipientUsername: '',
          content: '',
          url: null,
          publicId: null
        };
        this.formData = new FormData();
      })
      .finally(() => (this.loading = false));
    
  }

  ngAfterViewInit(): void {
    this.scrollToBottom();
  }

  private scrollToBottom(): void {
    try {
      setTimeout(() => {
        this.messageContainer.nativeElement.scrollTop = this.messageContainer.nativeElement.scrollHeight;
      }, 0);
    } catch (err) {
      console.error('Scroll to bottom failed:', err);
    }
  }
  // deleteMessage(id: number,containerDeleteMessage: string){
  //   this.messageService.deleteMessage(id,containerDeleteMessage).subscribe({
  //     next: () =>{
  //       this.messages?.splice(this.messages.findIndex(m => m.id === id), 1);
  //     }
  //   })
  // }
}
