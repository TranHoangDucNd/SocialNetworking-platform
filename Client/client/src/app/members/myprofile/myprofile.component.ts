import { Component, Input } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { PresenceService } from 'src/app/_service/presence.service';

@Component({
  selector: 'app-myprofile',
  templateUrl: './myprofile.component.html',
  styleUrls: ['./myprofile.component.css']
})
export class MyprofileComponent {
  @Input() member!: Member;

  constructor(public presenceService: PresenceService){}
}
