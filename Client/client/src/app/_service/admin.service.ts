import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { PostReportDto } from '../_models/PostModels';
import { LockUser, MembersLock, Reports } from '../_models/admin';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  findUsersWithRoles(username: string){
    return this.http.get<User[]>(this.baseUrl + 'admin/users-with-roles?userName=' + username);
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
    return this.http.delete(this.baseUrl + 'admin/delete-post-report/' + postId)
  }

  getMembersLockAdmin(username: string){
    return this.http.get<MembersLock[]>(this.baseUrl + 'admin/GetMembersByAmin?username=' + username);
  }
  setLockMember(setLock: LockUser){
    return this.http.put(this.baseUrl + 'admin/LockAndUnlockAccount', setLock);
  }
}
