import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Member } from '../_models/member';
import { environment } from 'src/environments/environment';
import { UserParams } from '../_models/userParams';
import { getPaginationHeaders, getPaginationResult } from './paginationHelper';
import { AccountService } from './account.service';
import { map, of, take } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  user: User | undefined;
  userParams: UserParams | undefined;
  memberCache = new Map();

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user =>{
        if(user){
          this.userParams = new UserParams(user);
          this.user = user;
        }
      }
    })
  }

  getUserParams(){
    return this.userParams;
  }

  setUserParams(userParams: UserParams){
    this.userParams = userParams;
  }

  resetUserParams(){
    if(this.user){
      this.userParams = new UserParams(this.user);
      return this.userParams;
    }
    return;
  }


  getMembers(userParams: UserParams) {

    const response = this.memberCache.get(Object.values(userParams).join('-'));
    if(response) return of(response);


    let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return getPaginationResult<Member[]>(this.baseUrl + 'users', params, this.http).pipe(
      map(response =>{
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response;
      })
    )
  }

  getMember(userName: string){

    const member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member: Member) => member.userName == userName)
    
    if(member) return of(member);

    return this.http.get<Member>(this.baseUrl + 'users/' + userName)
  }

  upDateMember(member: Member){
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(()=>{
        const index = this.members.indexOf(member);
        this.members[index] = {...this.members[index], ...member};
      })
    );
  }

  setMainPhoto(photoId: number){
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number){
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  addLike(username: string){
    return this.http.post(this.baseUrl + 'likes/' + username, {});
  }

  getLike(predicate: string, pageNumber: number, pageSize: number){
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('predicate', predicate);

    return getPaginationResult<Member[]>(this.baseUrl + 'likes', params, this.http);
  }
}
