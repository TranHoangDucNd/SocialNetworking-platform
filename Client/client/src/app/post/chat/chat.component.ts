import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { CommentPostDto } from 'src/app/_models/PostModels';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_service/account.service';
import { PostService } from 'src/app/_service/post.service';
@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class ChatComponent implements OnInit {
  reply(id: number) {}

  isCommented = false;
  isUpdateCommented = false;
  isEdit!: number; //id comment edit
  private hubConnection!: HubConnection;
  public Editor = ClassicEditor;
  user!: User;
  imgUser: string | null = null;
  userId!: number;
  postId: number;
  comments: CommentPostDto[] | null = null;
  createComment: CommentPostDto = {
    id: 0,
    userId: 0,
    postId: 0,
    userShort: null,
    content: '',
    createdAt: new Date(),
    updatedAt: null
  };

  updateComment: CommentPostDto = {
    id: 0,
    userId: 0,
    postId: 0,
    userShort: null,
    content: '',
    createdAt: new Date(),
    updatedAt: null
  };

  createCommentContent: string = '';
  contentUpdate: string = '';



  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { SubId: number },
    private postService: PostService,
    private router: Router,
    private accountService: AccountService,
    private toastr: ToastrService
  ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) =>{
        if(user){
          this.user = user
        }
      }
    });

    this.postId = data.SubId;
  }
  ngOnInit(): void {
    this.hubConnection = new HubConnectionBuilder()
    .withUrl(this.postService.getChatSignalR())
    .build();

    this.hubConnection.start().then(() => {
      console.log('Kết nối thành công');
    }).catch((error)=>{
      console.log('Lỗi khi thiết lập kết nối',error)
    })

    this.hubConnection.on('ReceiveComment', (data: any)=>{
      this.comments = data.resultObj;
    })

    this.getUserShort()
    this.getChatPost();
  }

  getUserShort(){
    this.postService.getUserShort().subscribe(
      (data: any) =>{
        console.log(data);
        this.userId = data.resultObj.id;
      }
    )
  }

  getChatPost(){
    this.postService.getPostComment(this.postId).subscribe((data: any) =>{
      console.log(data);
      this.comments = data.resultObj as CommentPostDto[];
    })
  }

  public editorConfig = {
    toolbar: {
      items: [
        'heading',
        'bold',
        'italic',
        'blockQuote',
        'bulletedList',
        'numberedList',
        'link',
      ],
    },
    placeholder: 'Comment...',
    language: 'en',
  };

  hasImage(content: string): boolean {
    const imageRegex = /<img[^>]+src\s*=\s*['"]([^'"]+)['"][^>]*>/g;
    const containsImage = imageRegex.test(content);
    if (containsImage) {
      return true;
    }
    return false;
  }

  //Bắt dữ liệu sự kiện nhập comment
  onEditorChange(event: any) {
    this.isCommented = true;
    this.createCommentContent = event.editor.getData();
    if (this.hasImage(this.contentUpdate)) {
      this.toastr.warning('Không được bình luận có nội dung là ảnh!');
    }
  }

  //bắt dữ liệu sự kiện edit comment
  onEditChange(event: any) {
    this.isUpdateCommented = true;
    this.contentUpdate = event.editor.getData();

    if (this.hasImage(this.contentUpdate)) {
      this.toastr.warning('Không được bình luận có nội dung là ảnh!');
    }
  }



  sendComment(){
    if(this.hasImage(this.createCommentContent)){
      this.toastr.warning("Không được bình luận có nội dung hình ảnh!")
      return;
    }

    this.createComment.postId = this.postId;
    this.createComment.userId = this.userId;
    this.createComment.content = this.createCommentContent.trim();

    this.postService.CreatePostComment(this.createComment).subscribe(
      (data: any) =>{
        this.cancelComment();
      },
      (error) => console.log(error.message)
    )
  }

  submitEdited(){
    if(this.hasImage(this.createCommentContent)){
      this.toastr.warning("Không được bình luận có nội dung hình ảnh!")
      return;
    }

    this.updateComment.content = this.contentUpdate?.trim();

    if(this.contentUpdate.trim() == ''){
      this.toastr.warning("Vui lòng không để trống bình luận")
      return;
    }

    this.postService.UpdatePostComment(this.updateComment).subscribe(
      (data) => {
        this.contentUpdate = '';
        this.cancelEditComment();
      }
    )
  }

  editComment(id: number){
    var foundComment = this.comments?.find(x => x.id === id)
    if(foundComment)
      {
        this.updateComment = foundComment;
        this.contentUpdate = foundComment.content;
        this.isEdit = id;
      }
  }

  deleteComment(id: number){
    this.postService.DeleteComment(id).subscribe(
      (data: any) =>{},
      (error) => console.log('Lỗi', error)
    )
  }

  isCheckCommented(){
    return this.isCommented;
  }

  isCheckEdit(id: number): boolean{
    return this.isEdit == id;
  }

  loginUser(){
    const currentUrl = this.router.url;
    this.router.navigate(['/login'], {state: {redirect: currentUrl}})
  }

  cancelComment() {
    this.isCommented = false;
    this.createCommentContent = '';
  }

  cancelEditComment() {
    this.isUpdateCommented = false;
    this.contentUpdate = '';
    this.isEdit = -1;
  }

  onPaste(event: any) {
    const clipboardData =
      event.clipboardData ||
      (event.originalEvent && event.originalEvent.clipboardData);

    if (clipboardData) {
      const items = clipboardData.items;
      console.log(1);
      for (let i = 0; i < items.length; i++) {
        const item = items[i];

        if (item.type.indexOf('image') !== -1) {
          // Ngăn chặn dán ảnh
          console.log(2);
          event.preventDefault();
          this.toastr.info('Dán ảnh không được phép.');
          break;
        }
      }
    }
  }

}
