import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from 'src/environments/environment';
import { CommentPostDto } from '../_models/PostModels';

@Injectable({
  providedIn: 'root',
})
export class PostService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  constructor(private http: HttpClient) {}

  getUserShort() {
    return this.http.get(this.baseUrl + 'Post/UserShort');
  }

  getAll() {
    return this.http.get(this.baseUrl + 'Post/MyPost');
  }

  getPostUpdate(id: number) {
    return this.http.get(this.baseUrl + 'Post/' + id);
  }

  getPosts(){
    return this.http.get(this.baseUrl + 'Post');
  }

  UpdatePost(requestDto: FormData) {
    return this.http.put(this.baseUrl + 'Post', requestDto);
  }
  CreatePost(requestDto: FormData) {
    return this.http.post(this.baseUrl + 'Post', requestDto);
  }
  deletePost(id: number){
    return this.http.delete(this.baseUrl + 'Post/delete/' + id);
  }

  getPostComment(postId: any){
    return this.http.get(this.baseUrl + 'Post/Chat?PostId=' + postId);
  }

  getChatSignalR(): string{
    return this.hubUrl + 'commentHub';
  }

  CreatePostComment(data: CommentPostDto){
    return this.http.post(this.baseUrl + 'Post/Chat', data)
  }

  UpdatePostComment(data: CommentPostDto){
    return this.http.put(this.baseUrl + 'Post/Chat', data);
  }

  DeleteComment(id: number){
    return this.http.delete(this.baseUrl + 'Post/Chat?commentId='+id);
  }
  
}
