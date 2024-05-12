import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';

import {environment} from 'src/environments/environment';
import {
  CommentPostDto,
  CountLikesAndPosts,
  CreateCommentDto,
  IReactionRequest,
  PostFpkDto,
  PostReportDto,
  UserShortDto
} from '../_models/PostModels';
import {of} from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class PostService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  #allUserShort: any;
  #userShort: any;

  constructor(private http: HttpClient) {
  }

  getUserShort() {
    if (!this.#userShort) {
      return this.http.get<any>(this.baseUrl + 'Post/UserShort')
    }
    return of(this.#userShort);
  }

  setUserShort(userShort: any) {
    this.#userShort = userShort;
  }

  getAllUsersShort() {
    if (!this.#allUserShort) {
      return this.http.get<UserShortDto[]>(this.baseUrl + 'Post/AllUserShorts')
    }
    return of(this.#allUserShort);
  }

  setAllUsersShort(allUserShort: any) {
    this.#allUserShort = allUserShort;
  }

  getAll() {
    return this.http.get(this.baseUrl + 'Post/MyPost');
  }

  getPostUpdate(id: number) {
    return this.http.get(this.baseUrl + 'Post/' + id);
  }

  getPosts() {
    return this.http.get(this.baseUrl + 'Post');
  }

  UpdatePost(requestDto: FormData) {
    return this.http.put(this.baseUrl + 'Post', requestDto);
  }

  CreatePost(requestDto: FormData) {
    return this.http.post(this.baseUrl + 'Post/create-post', requestDto);
  }

  deletePost(id: number) {
    return this.http.delete(this.baseUrl + 'Post/delete/' + id);
  }

  updatePostReaction(request: IReactionRequest) {
    return this.http.post(this.baseUrl + 'Post/update-post-reaction', request);
  }

  updateCommentReaction(request: IReactionRequest) {
    return this.http.post(this.baseUrl + 'Post/update-comment-reaction', request);
  }

  getDetailReaction(targetId: number) {
    return this.http.get(this.baseUrl + 'Post/get-detail-reaction?targetId=' + targetId);
  }

  getPostComment(postId: any) {
    return this.http.get(this.baseUrl + 'Post/Chat?PostId=' + postId);
  }

  getChatSignalR(): string {
    return this.hubUrl + 'commentHub';
  }

  CreatePostComment(data: CommentPostDto) {
    return this.http.post(this.baseUrl + 'Post/Chat', data)
  }

  UpdatePostComment(data: CommentPostDto) {
    return this.http.put(this.baseUrl + 'Post/Chat', data);
  }

  DeleteComment(id: number) {
    return this.http.delete(this.baseUrl + 'Post/Chat?commentId=' + id);
  }

  likeOrDisLike(postFpk: PostFpkDto) {
    const params = new HttpParams()
      .set('postId', postFpk.postId)
      .set('userId', postFpk.userId)
    return this.http.post(this.baseUrl + 'Post/Like', postFpk);
  }

  countLikesAndCommentsOfPost(postId: number) {
    return this.http.get<CountLikesAndPosts>(this.baseUrl + 'Post/LikesAndComments/' + postId);
  }

  createComment(input: CreateCommentDto) {
    return this.http.post(this.baseUrl + 'Post/create-comment', input);
  }

  deleteComment(commentId: number) {
    return this.http.delete(this.baseUrl + 'Post/delete-comment?commentId=' + commentId);
  }

  getAllCommentsOfPost(postId: number) {
    return this.http.get<any>(this.baseUrl + 'Post/get-comments-of-post?postId=' + postId);
  }

  Report(report: PostReportDto) {
    const postReport = new FormData();
    postReport.append('userId', report.userId.toString());
    postReport.append('postId', report.postId.toString());
    postReport.append('description', report.description);
    postReport.append('reportDate', report.reportDate.toISOString());
    postReport.append('checked', report.checked.toString());
    postReport.append('report', report.report.toString());

    return this.http.post(this.baseUrl + 'Post/Report', postReport);

  }

  GetContentReport() {
    return this.http.get(this.baseUrl + 'Post/ContentReport');
  }


}
