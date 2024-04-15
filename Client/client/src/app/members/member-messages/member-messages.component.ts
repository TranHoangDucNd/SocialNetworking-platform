import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TimeagoModule } from 'ngx-timeago';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_service/message.service';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css'],
  imports: [CommonModule, TimeagoModule, FormsModule, BsDropdownModule],
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm?: NgForm;
  @Input() username?: string;
  @Input() user?: string;
  @Input() messages: Message[] = [];

  messageContent = '';

  constructor(private messageService: MessageService) {}
  ngOnInit(): void {}

  sendMessage() {
    if (!this.username) return;
    this.messageService
      .sendMessage(this.username, this.messageContent)
      .subscribe({
        next: (messsage) => {
          this.messages.push(messsage);
          this.messageForm?.reset();
        },
      });
  }

  loadMessages() {
    if (this.username) {
      this.messageService.getMessageThread(this.username).subscribe({
        next: (messages) => {
          this.messages = messages;
        },
      });
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
