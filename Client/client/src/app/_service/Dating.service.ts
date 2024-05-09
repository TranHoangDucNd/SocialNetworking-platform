import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DatingService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  updateDatingProfile(datingProfile: FormData) {
    return this.http.post(this.baseUrl + 'DatingProfile', datingProfile);
  }

  GetUserInterests() {
    return this.http.get<any>(this.baseUrl + 'DatingProfile/GetUserInterests');
  }
  GetWhereToDate() {
    return this.http.get<any>(this.baseUrl + 'DatingProfile/GetWhereToDate');
  }
  GetHeight() {
    return this.http.get<any>(this.baseUrl + 'DatingProfile/GetHeight');
  }
  GetDatingObject() {
    return this.http.get<any>(this.baseUrl + 'DatingProfile/GetDatingObject');
  }
}
