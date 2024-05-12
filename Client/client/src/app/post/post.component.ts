import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_service/account.service';
import { PostService } from '../_service/post.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { User } from '../_models/user';
import { PostFpkDto, PostLike, PostResponseDto, UserShortDto } from '../_models/PostModels';
import { CreatepostComponent } from './createpost/createpost.component';
import { UpdatepostComponent } from './updatepost/updatepost.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ChatComponent } from './chat/chat.component';
import { Overlay } from '@angular/cdk/overlay';
import { ReportComponent } from '../report/report.component';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css'],
})
export class PostComponent implements OnInit {
  user!: User;
  userShort!: UserShortDto;
  posts: PostResponseDto[] = [];
  postLike: PostFpkDto = {
    postId: 0,
    userId: 0
  }

  posId!: number;

  countLike: number = 0;

  constructor(
    private accountService: AccountService,
    private postService: PostService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private overlay: Overlay,
    private dialog: MatDialog
  ) {

  }
  ngOnInit(): void {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) {
          this.user = user;
        }
      },
    });
    this.getPosts();
    this.getUserShort();
  }
  getPosts() {
    this.postService.getPosts().subscribe(
      (data: any) => {
        if (data.isSuccessed === true) {
          this.posts = data.resultObj as PostResponseDto[];
        }
      },
      (error: any) => {
        console.log(error);
      }
    );
  }

  getUserShort() {
    this.postService.getUserShort().subscribe(
      (data: any) =>{
        this.postService.setUserShort(data);
        this.userShort = data.resultObj;

      },
      (error: any) =>{
        console.log(error);
      }
    )
  }

  openCreatePostModal(){
    this.modalService.show(CreatepostComponent, {
      class: 'modal-lg'
    })
  }

  updatePost(id: number){
    const initialState = {id: id};
    this.modalService.show(UpdatepostComponent, {
      class: 'modal-lg',
      initialState: initialState // tự map dữ liệu vào id bên updatepostcomponent
    });
  }

  deletePost(id: number){
    this.postService.deletePost(id).subscribe(
      (data: any) => {
        console.log(data);
        if(data.isSuccessed === true){
          this.getPosts();
          this.toastr.success('Deleted post success');
        }
      },
      (error: any) =>{
        console.log(error);
      }
    )
  }

  Comment(postId: number){
    this.openDialogComment('10ms', '10ms', postId);
  }

  openDialogComment(enteranimation: any, exitanimation: any, postId: number) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.hasBackdrop = false;
    const popup = this.dialog.open(ChatComponent, {
      enterAnimationDuration: enteranimation,
      exitAnimationDuration: exitanimation,
      width: '414px',
      height: '100%',
      data: {
        SubId: postId,
      },
      panelClass: 'right-aligned-dialog',
      backdropClass: 'custom-backdrop',
      // Enable scoll page
      scrollStrategy: this.overlay.scrollStrategies.noop(),
    });
  }


  getLike(postLikes: PostLike[]): any{
    return postLikes.some((postlike) => postlike.userId === this.userShort.id);
  }


  likeOrDisLike(postId: number){
    this.postLike.postId = postId;
    this.postLike.userId = this.userShort.id;
    this.postService.likeOrDisLike(this.postLike).subscribe(
      (data: any) =>{
        this.posts = data.resultObj;
      },
      (error: any) => {console.log(error)}
    );
  }

  Report(id: number){
    const initialState = {postId: id};
    this.modalService.show(ReportComponent, {
      class: 'modal-lg',
      initialState: initialState
    })
  }
}
