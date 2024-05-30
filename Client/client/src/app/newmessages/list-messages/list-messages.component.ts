import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { MessagesForUser } from 'src/app/_models/message';
import { MembersService } from 'src/app/_service/members.service';
import { MessageService } from 'src/app/_service/message.service';
import { PresenceService } from 'src/app/_service/presence.service';

@Component({
  selector: 'app-list-messages',
  templateUrl: './list-messages.component.html',
  styleUrls: ['./list-messages.component.css']
})
export class ListMessagesComponent implements OnInit {
  messages?: MessagesForUser[];
  loading = false;
  @Output() selectedUser = new EventEmitter<string>();
  @Output() imgPath = new EventEmitter<string>();
  @Output() knownAs = new EventEmitter<string>();
  selectedUsername?: string;
  member!: Member
  constructor(private messageService: MessageService, 
    public presenceService: PresenceService,
    private memberService: MembersService){

  }
  ngOnInit(): void {
    this.loadMessages();

    this.messageService.messageList$.subscribe({
      next: messages => {
        this.messages = messages;
      }
    });
  }

  selectUser(username: string) {
    this.selectedUsername = username;
    this.selectedUser.emit(username);
    this.memberService.getMember(username).subscribe({
      next: member =>{
        if(member){
          this.member = member;
          this.imgPath.emit(this.member.photoUrl);
          this.knownAs.emit(this.member.knownAs);
        }
      }
    })

  }
 
  loadMessages(){
    this.loading = true;
    this.messageService.getMessagesForUser().subscribe({
      next: data => {
        console.log(data);
        this.messages = data;
      }
    })

    this.messageService.getMessagesForUser();
  }
}
