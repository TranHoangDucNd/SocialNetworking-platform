import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-newmessages',
  templateUrl: './newmessages.component.html',
  styleUrls: ['./newmessages.component.css']
})
export class NewmessagesComponent implements OnInit{
  selectedUsername!: string;
  knownAs!: string;
  photoUrl!: string;
  ngOnInit(): void {
    console.log(this.selectedUsername);
    
  }
  onUserSelected(username: string) {
    this.selectedUsername = username;
  }
  onUserKnownAs(knownAs: string){
    this.knownAs = knownAs;
  }
  onUserPhotoUrl(photoUrl: string){
    this.photoUrl = photoUrl;
  }
}
