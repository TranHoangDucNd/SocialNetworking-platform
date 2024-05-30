import {Component, DestroyRef, EventEmitter, inject, Input, OnInit, Output} from '@angular/core';
import {CommentDto, ReactionType} from "../../_models/PostModels";
import {User} from "../../_models/user";
import {PostService} from "../../_service/post.service";
import {ToastrService} from "ngx-toastr";
import {Subject, switchMap, throttleTime} from "rxjs";
import {takeUntilDestroyed} from "@angular/core/rxjs-interop";
import {convertToEmoji, countComments} from "../../_service/post.helper";

@Component({
  selector: 'app-post-comment',
  templateUrl: './post-comment.component.html',
  styleUrls: ['./post-comment.component.css'],
})
export class PostCommentComponent implements OnInit {
  @Output() commentChanged = new EventEmitter<boolean>();
  postService = inject(PostService);
  toastr = inject(ToastrService);
  _comment: CommentDto | undefined;
  destroy$ = inject(DestroyRef)

  reaction$ = new Subject();

  commentCount: number = 0;
  reactionCount: number = 0;
  currentReaction: string = '';

  commentOpened = false;
  showReactions: boolean = false;

  @Input() set comment(value: CommentDto | undefined) {
    this.postService.getAllUsersShort().subscribe({
      next: (data: any) => {
        this._comment = value;
        this._comment = {
          ...this._comment,
          userShort: data.resultObj.find((user: any) => user.id === this._comment?.userId)
        } as CommentDto;
      }
    });
    this.commentCount = countComments(value?.descendants as CommentDto[]);
  }

  get comment(): CommentDto | undefined {
    return this._comment;
  }

  @Input() level = 0;
  @Input() user: User | undefined;

  ngOnInit() {
    this.getReactions();
    this.reaction$.pipe(throttleTime(100),
      switchMap((reaction) => this.postService.updateCommentReaction({
        targetId: this.comment?.id as number,
        reactionType: ReactionType[reaction as keyof typeof ReactionType],
        target: this.comment?.id as number
      })),
      takeUntilDestroyed(this.destroy$))
      .subscribe({
        next: (res: any) => {
          this.toastr.success('Reaction updated successfully');
          this.getReactions()
          // this.postChanged.emit(true);
        }
      });
  }

  getReactions() {
    this.postService.getCommentReactionDetail(this.comment?.id as number).subscribe({
      next: (data: any) => {
        this.reactionCount = data.resultObj.length;
        if (data.resultObj.length === 0 || !data.resultObj.some((reaction: {
          userId: number;
        }) => reaction.userId === this.user?.id)) {
          this.currentReaction = '';
          return;
        }
        const type = data.resultObj.find((reaction: {
          userId: number;
        }) => reaction.userId === this.user?.id).type as number;
        this.currentReaction = convertToEmoji(ReactionType[type]);
      }
    })
  }

  handleReaction(reaction: string) {
    this.reaction$.next(reaction);
  }
  
  openedComments: { [key: number]: boolean } = {};
  openComment(commentId: number) {
    this.openedComments[commentId] = !this.openedComments[commentId];
  }
  openCommentBox: boolean = false;
  openCommentBx() {
    this.openCommentBox = !this.openCommentBox;
  }

  handleCommentAdded($event: boolean) {
    if ($event) {
      this.commentChanged.emit(true);
    }
  }

  handleCommentChanged($event: boolean) {
    if ($event) {
      this.commentChanged.emit(true);
    }
  }

  delete(id: number) {
    this.postService.deleteComment(id).subscribe({
      next: (res: any) => {
        this.toastr.success('Comment deleted successfully');
        this.commentChanged.emit(true);
      }
    })
  }
}
