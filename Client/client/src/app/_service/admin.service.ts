import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { PostReportDto } from '../_models/PostModels';
import { Reports } from '../_models/admin';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  findUsersWithRoles(username: string){
    return this.http.get<User[]>(this.baseUrl + 'admin/users-with-roles?userName=' + username);
  }
  getUsersWithRoles(){
    return this.http.get<User[]>(this.baseUrl + 'admin/users-with-roles');
  }
  updateUserRoles(username: string, roles: string[]){
    return this.http.post<string[]>(this.baseUrl + 'admin/edit-roles/' + username + '?roles=' + roles, {});
  }

  getPostReports(){
    return this.http.get(this.baseUrl + 'admin/get-reports');
  }

  getPostReportDetail(postId: number){
    return this.http.get(this.baseUrl + 'admin/get-detail-post-report/' + postId)
  }

  deletePost(postId: number){
    return this.http.delete(this.baseUrl + 'admin/delete-post-report/' +postId)
  }
}
