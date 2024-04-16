import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Member } from 'src/app/_models/member';
import { GalleryModule, GalleryItem, ImageItem } from 'ng-gallery';
import { CommonModule } from '@angular/common';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { Message } from 'src/app/_models/message';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_service/account.service';
import { MessageService } from 'src/app/_service/message.service';
import { take } from 'rxjs';
import { PresenceService } from 'src/app/_service/presence.service';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  imports: [
    GalleryModule,
    CommonModule,
    TabsModule,
    TimeagoModule,
    MemberMessagesComponent,
  ],
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', {static: true}) memberTabs?: TabsetComponent
  member: Member = {} as Member; //đây không phải người dùng hiện tại nó không chứa token
  images: GalleryItem[] = [];
  activeTab?: TabDirective;
  messages: Message[] = [];
  user?: User; //current user

  constructor(private accountService:AccountService,
     private route:ActivatedRoute, private messageService: MessageService,
     public presenceService: PresenceService) {
      this.accountService.currentUser$.pipe(take(1)).subscribe({
        next: user => {
          if(user){
            this.user = user
          }
        }
      })
      }

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data=>{
        this.member = data['member'] //member: resolve: {member: memberDetailedResolver} ở approuting
      }
    })
    this.route.queryParams.subscribe({
      next: params =>{
        params['tab'] && this.selectTab(params['tab'])
      }
    })

    this.getImages();
  }

  ngOnDestroy(): void {
      this.messageService.stopHubConnection();
  }

  //Khi người dùng kích vào tin nhắn nó sẽ đi đến luôn mục message
  onTabActivated(data: TabDirective){
    this.activeTab = data;

    if(this.activeTab.heading === 'Messages' && this.user){
      this.messageService.createHubConnection(this.user, this.member.userName)
    }else{
      this.messageService.stopHubConnection();
    }
  }

  loadMessages(){
    if(this.member){
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: messages => {
          this.messages = messages;
        }
      })
    }
  }

  selectTab(heading: string){
    if(this.memberTabs){
      this.memberTabs.tabs.find(x=>x.heading === heading)!.active = true;   
    }
  }

  getImages(){
    if(!this.member) return;
    for(const photo of this.member?.photos){
      this.images.push(new ImageItem({src: photo.url, thumb: photo.url}));
    }
  }
}
