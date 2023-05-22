import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {User} from "../models/user.model";
import {ChangePassword} from "../models/changePassword.model";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + '/Users/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getUserById(userId: string): Observable<User> {
    return this._httpClient.get<User>(this._serviceUrl + userId);
  }

  changeUserPassword(changePassword: ChangePassword): Observable<boolean> {
    return this._httpClient.post<boolean>(this._serviceUrl + 'ChangeUserPassword', changePassword);
  }
}
