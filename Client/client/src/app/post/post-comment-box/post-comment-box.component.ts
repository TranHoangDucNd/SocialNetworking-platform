import {Component, EventEmitter, inject, Input, OnInit, Output} from '@angular/core';
import {switchMap, take} from "rxjs";
import {AccountService} from "../../_service/account.service";
import {User} from "../../_models/user";
import {PostService} from "../../_service/post.service";
import {CreateCommentDto, UserShortDto} from "../../_models/PostModels";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-post-comment-box',
  templateUrl: './post-comment-box.component.html',
  styleUrls: ['./post-comment-box.component.css']
})
export class PostCommentBoxComponent implements OnInit {
  @Output() commentAdded = new EventEmitter<boolean>();

  @Input() postId: number | undefined;
  @Input() parentCommentId: number | undefined;

  // #accountService = inject(AccountService);
  #postService = inject(PostService);
  toastr = inject(ToastrService);
  userShort: UserShortDto | null | undefined;
  content = "";


  ngOnInit() {
    this.#postService.getUserShort()
      .subscribe({
        next: (res: any) => {
          this.userShort = res.resultObj;
        }
      })
  }

  sendComment() {
    console.log(this.content);
    if (!this.content || this.content.trim().length === 0) {
      this.toastr.error('Comment cannot be empty');
      return;
    }
    const input = new CreateCommentDto({
      userId: this.userShort?.id,
      postId: this.postId,
      parentCommentId: this.parentCommentId,
      content: this.content,
      userShort: this.userShort
    });
    return this.#postService.createComment(input)
      .subscribe({
        next: (res: any) => {
          this.content = "";
          this.commentAdded.emit(true);
          this.toastr.success('Comment added successfully');
        }
      })
  }


}
