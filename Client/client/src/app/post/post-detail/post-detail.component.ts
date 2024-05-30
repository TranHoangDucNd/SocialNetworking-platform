import {Component, inject, OnInit} from '@angular/core';
import {PostService} from "../../_service/post.service";
import {PostResponseDto} from "../../_models/PostModels";
import {ActivatedRoute} from "@angular/router";
import {User} from "../../_models/user";
import {AccountService} from "../../_service/account.service";

@Component({
  selector: 'app-post-detail',
  templateUrl: './post-detail.component.html',
  styleUrls: ['./post-detail.component.css']
})
export class PostDetailComponent implements OnInit {
  postService = inject(PostService);
  accountService = inject(AccountService);
  route = inject(ActivatedRoute);
  post: PostResponseDto | undefined;
  id: number = 0;
  user: User | undefined;

  constructor() {
  }

  ngOnInit(): void {
    this.user = this.accountService.getCurrentUser();
    this.route.params.subscribe(params => {
      this.id = params['id'] as number;
      if(this.id) this.getPost();
    });

  }

  getPost() {
    this.postService.getPostUpdate(this.id).subscribe({
      next: (data: any) => {
        this.post = data.resultObj;
      },
      error: (error: any) => {
        console.log(error);
      }
    });
  }

}
