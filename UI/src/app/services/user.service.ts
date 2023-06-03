import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {User, UserWithAddress} from "../models/user.model";
import {ChangePassword} from "../models/changePassword.model";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + '/Users/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getUsers(): Observable<User[]> {
    return this._httpClient.get<User[]>(this._serviceUrl);
  }

  getUsersWithAddresses(): Observable<UserWithAddress[]> {
    return this._httpClient.get<UserWithAddress[]>(this._serviceUrl + 'Addresses');
  }

  getUserById(userId: string): Observable<User> {
    return this._httpClient.get<User>(this._serviceUrl + userId);
  }

  changeUserPassword(changePassword: ChangePassword): Observable<boolean> {
    return this._httpClient.post<boolean>(this._serviceUrl + 'ChangeUserPassword', changePassword);
  }

  updateUserWithAddress(userWithAddress: UserWithAddress): Observable<UserWithAddress> {
    return this._httpClient.put<UserWithAddress>(this._serviceUrl + 'Addresses/' + userWithAddress.userId, userWithAddress);
  }

  updateUser(userId: string, user: User): Observable<User> {
    return this._httpClient.put<User>(this._serviceUrl + userId, user);
  }

  deleteUser(userId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + userId);
  }
}
