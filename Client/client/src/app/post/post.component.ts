import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_service/account.service';
import { PostService } from '../_service/post.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { User } from '../_models/user';
import { PostResponseDto, UserShortDto } from '../_models/PostModels';
import { CreatepostComponent } from './createpost/createpost.component';
import { UpdatepostComponent } from './updatepost/updatepost.component';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css'],
})
export class PostComponent implements OnInit {
  user!: User;
  userShort!: UserShortDto;
  posts: PostResponseDto[] = [];

  constructor(
    private accountService: AccountService,
    private postService: PostService,
    private modalService: BsModalService,
    private toastr: ToastrService
  ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) {
          this.user = user;
        }
      },
    });
  }
  ngOnInit(): void {
    this.getPosts();
    this.getUserShort()
  }
  getPosts() {
    this.postService.getPosts().subscribe(
      (data: any) => {
        console.log(data);
        if (data.isSuccessed === true) {
          this.posts = data.resultObj;
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
        console.log(data);
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
}
