import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PostDetailReportResponseDto } from 'src/app/_models/admin';
import { AdminService } from 'src/app/_service/admin.service';
import { PostService } from 'src/app/_service/post.service';

@Component({
  selector: 'app-postreportdetail',
  templateUrl: './postreportdetail.component.html',
  styleUrls: ['./postreportdetail.component.css']
})
export class PostreportdetailComponent implements OnInit{

  postId: number;
  postDetail!: PostDetailReportResponseDto;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: {SubId: number},
    private adminService: AdminService){
    this.postId = data.SubId
  }

  ngOnInit(): void {
   this.getPost(); 
  }

  getPost(){
    this.adminService.getPostReportDetail(this.postId).subscribe(
      (data: any) =>{
        if(data.isSuccessed === true){
          this.postDetail = data.resultObj as PostDetailReportResponseDto;
          console.log(this.postDetail)
        }
      }
    )
  }

}
