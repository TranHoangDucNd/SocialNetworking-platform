import { Component } from '@angular/core';
import { User } from '../_models/user';
import { PostResponseDto } from '../_models/PostModels';
import { AccountService } from '../_service/account.service';
import { PostService } from '../_service/post.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { take } from 'rxjs';
import { UpdatepostComponent } from '../post/updatepost/updatepost.component';
import { ToastrService } from 'ngx-toastr';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ChatComponent } from '../post/chat/chat.component';
import { Overlay } from '@angular/cdk/overlay';

@Component({
  selector: 'app-personalpage',
  templateUrl: './personalpage.component.html',
  styleUrls: ['./personalpage.component.css']
})
export class PersonalpageComponent {
  user!: User;
  posts: PostResponseDto[] = [];

  constructor(private accountService: AccountService,
    private postService: PostService,
    private modalService: BsModalService,
    private toast: ToastrService,
    private dialog: MatDialog,
    private overlay: Overlay
  ){
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: data =>{
        if(data){
          this.user = data;
        }
      }
    })
    this.getPosts();
  }

  getPosts() {
    this.postService.getAll().subscribe(
      (data: any) =>{
        this.posts = data.resultObj;
        console.log(this.posts);
      },
      (error: any) =>{
        console.log(error);
      }
    );
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
          this.toast.success('Deleted post success');
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
}
