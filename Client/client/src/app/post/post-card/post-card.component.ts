import {Component, DestroyRef, EventEmitter, inject, Input, OnInit, Output} from '@angular/core';
import {CommentDto, PostResponseDto, ReactionType} from "../../_models/PostModels";
import {User} from "../../_models/user";
import {BsModalService} from "ngx-bootstrap/modal";
import {PostService} from "../../_service/post.service";
import {UpdatepostComponent} from "../updatepost/updatepost.component";
import {ToastrService} from "ngx-toastr";
import {ReportComponent} from "../../report/report.component";
import {forkJoin, Subject, switchMap, throttleTime} from "rxjs";
import {takeUntilDestroyed} from "@angular/core/rxjs-interop";
import {convertToEmoji, countComments} from "../../_service/post.helper";

@Component({
  selector: 'app-post-card',
  templateUrl: './post-card.component.html',
  styleUrls: ['./post-card.component.css']
})
export class PostCardComponent implements OnInit {
  _post: PostResponseDto | undefined;
  @Output() postChanged = new EventEmitter<boolean>();

  @Input() set post(value: PostResponseDto | undefined) {
    this._post = value;
    this.getReactions();
    this.getComments();
  }

  get post() {
    return this._post;
  }

  @Input() postId = 0;
  @Input() user: User | undefined;
  @Input() commentOpened = false;
  modalService = inject(BsModalService);
  postService = inject(PostService);
  toastr = inject(ToastrService);
  destroy$ = inject(DestroyRef)

  reaction$ = new Subject();

  comments: CommentDto[] = [];
  commentCount: number = 0;
  reactionCount: number = 0;
  currentReaction: string = '';


  showReactions: boolean = false;

  ngOnInit() {
    console.log(this.post)
    this.getComments();
    this.getReactions();
    this.reaction$.pipe(throttleTime(100),
      switchMap((reaction) => this.postService.updatePostReaction({
        targetId: this.post?.id as number,
        reactionType: ReactionType[reaction as keyof typeof ReactionType],
        target: this.post?.id as number
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
    this.postService.getPostReactionDetail(this.post?.id || this.postId).subscribe({
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

  getComments() {
    forkJoin([
      this.postService.getAllUsersShort(),
      this.postService.getAllCommentsOfPost(this.post?.id || this.postId)
    ]).subscribe({
      next: ([allShorts, comments]) => {
        this.postService.setAllUsersShort(allShorts);

        const commentsWithUserShort = comments.resultObj.map((comment: { userId: number; }) => {
          const userShort = allShorts.resultObj.find((short: { id: number; }) => short.id === comment.userId);
          return {
            ...comment,
            userShort
          }
        })
        this.comments = commentsWithUserShort;
        this.commentCount = countComments(commentsWithUserShort);
      }
    })
  }

  handleReaction(reaction: string) {
    this.reaction$.next(reaction);
  }

  updatePost(id: number) {
    const initialState = {id: id};
    this.modalService.show(UpdatepostComponent, {
      class: 'modal-lg',
      initialState: initialState // tự map dữ liệu vào id bên updatepostcomponent
    });
  }

  deletePost(id: number) {
    this.postService.deletePost(id).subscribe(
      (data: any) => {
        if (data.isSuccessed === true) {
          this.postChanged.emit(true);
          this.toastr.success('Deleted post success');
        }
      },
      (error: any) => {
        console.log(error);
      }
    )
  }

  report(id: number) {
    const initialState = {postId: id};
    this.modalService.show(ReportComponent, {
      class: 'modal-lg',
      initialState: initialState
    })
  }

  openComment() {
    this.commentOpened = !this.commentOpened
  }

  handleCommentChanged($event: boolean) {
    if ($event) {
      this.getComments()
    }
  }

  openImageInNewTab(img: string) {
    window.open(img, '_blank');
  }
}
