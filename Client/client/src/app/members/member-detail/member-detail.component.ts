import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Member } from 'src/app/_models/member';
import { GalleryModule, GalleryItem, ImageItem } from 'ng-gallery';
import { CommonModule } from '@angular/common';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  imports: [GalleryModule,CommonModule,TabsModule, TimeagoModule]
})
export class MemberDetailComponent implements OnInit{

  member: Member = {} as Member;
  images: GalleryItem[] = [];

  constructor(private route: ActivatedRoute){}
  ngOnInit(): void {
    this.route.data.subscribe({
      next: data=>{
        this.member = data['member']
      }
    })

    this.getImage();
  }

  getImage(){
    if(!this.member) return;
    for(const photo of this.member.photos){
      this.images.push(new ImageItem({src: photo.url, thumb: photo.url}))
    }
  }

}
