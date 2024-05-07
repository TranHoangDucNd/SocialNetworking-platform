import { Component, Input, OnInit } from '@angular/core';
import { CountLikesAndPosts } from 'src/app/_models/PostModels';
import { PostService } from 'src/app/_service/post.service';

@Component({
  selector: 'app-countlikecomment',
  templateUrl: './countlikecomment.component.html',
  styleUrls: ['./countlikecomment.component.css']
})
export class CountlikecommentComponent implements OnInit{
  @Input() postId!: number;

  count: CountLikesAndPosts = {
    likes: 0,
    comments: 0
  }
  constructor(private postService: PostService){
    
  }
  ngOnInit(): void {
    this.getLikesAnhComments();
  }
  getLikesAnhComments() {
    return this.postService.countLikesAndCommentsOfPost(this.postId).subscribe(
      (data: any) =>{
        this.count = data;
      }
    )
  }


}
