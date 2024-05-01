import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { CreatePostDto } from 'src/app/_models/PostModels';
import { AccountService } from 'src/app/_service/account.service';
import { PostService } from 'src/app/_service/post.service';

@Component({
  selector: 'app-updatepost',
  templateUrl: './updatepost.component.html',
  styleUrls: ['./updatepost.component.css'],
})
export class UpdatepostComponent implements OnInit {
  @ViewChild('CreatePostTemplate') CreatePostTemplate!: TemplateRef<any>;
  createPost: CreatePostDto;
  modalRef?: BsModalRef;
  selectedImages: string[] = [];
  formData: FormData;
  username!: string;
  id!: number; //id đã được truyền từ personalpage qua rồi

  constructor(private postService: PostService,
    private toastr: ToastrService,
    private accountService: AccountService,
    private modalService: BsModalService
    ) {
    this.createPost = {
      id: 0,
      content: '',
      image: undefined,
    };
    this.formData = new FormData();
  }
  ngOnInit(): void {
    this.getPost();
  }

  getPost() {
    this.postService.getPostUpdate(this.id).subscribe(
      (data: any) => {
        console.log(data);
        if (data.isSuccessed === true) {
          this.createPost.id = data.resultObj.id;
          this.createPost.content = data.resultObj.content;
          this.selectedImages = data.resultObj.images;
        } else {
          this.toastr.error(data.message, 'Lỗi');
        }
      },
      (error: any) => {
        console.log(error);
      }
    );
  }

  AddImage(event: Event) {
    this.selectedImages = [];
    const selectedFiles = (event.target as HTMLInputElement).files;
    if (selectedFiles) {
      for (let i = 0; i < selectedFiles.length; i++) {
        this.formData.append('Image', selectedFiles[i]);

        const file = selectedFiles[i];
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.selectedImages.push(e.target.result);
        };
        reader.readAsDataURL(file);
      }
    }
  }

  UpdatePost() {
    if (this.createPost.content == '') {
      this.toastr.info("You haven't written anything yet!");
      return;
    }

    this.formData.append('Id', this.createPost.id.toString());
    this.formData.append('Content', this.createPost.content);
    console.log(this.createPost);
    this.postService.UpdatePost(this.formData).subscribe(
      (data: any) =>{
        console.log(data);
        if(data.isSuccessed){
          const user$ = this.accountService.currentUser$;
          user$.pipe(take(1)).subscribe((user)=>{
            if(user){ 
              this.modalService.hide();
              window.location.reload();
              setTimeout(() => {
                this.toastr.success('Congratulations! You have updated dating profile success')
              }, 1500);
            }
          })
        }
      },
      (error: any) =>{
        console.log(error);
      }
    )
  }
}
