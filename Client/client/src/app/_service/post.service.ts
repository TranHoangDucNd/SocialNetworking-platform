import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class PostService {
  baseUrl = environment.apiUrl;
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
  
}
