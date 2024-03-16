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

  // API base URL obtained from environment
  private readonly _serviceUrl: string = environment.apiBaseUrl + '/Users/';

  // Injecting necessary services
  private readonly _httpClient: HttpClient = inject(HttpClient);


  // Method to get all users
  getUsers(): Observable<User[]> {
    return this._httpClient.get<User[]>(this._serviceUrl);
  }

  // Method to get users with addresses
  getUsersWithAddresses(): Observable<UserWithAddress[]> {
    return this._httpClient.get<UserWithAddress[]>(this._serviceUrl + 'Addresses');
  }

  // Method to get user by ID
  getUserById(userId: string): Observable<User> {
    return this._httpClient.get<User>(this._serviceUrl + userId);
  }

  // Method to change user password
  changeUserPassword(changePassword: ChangePassword): Observable<boolean> {
    return this._httpClient.post<boolean>(this._serviceUrl + 'ChangeUserPassword', changePassword);
  }

  // Method to update user with address
  updateUserWithAddress(userWithAddress: UserWithAddress): Observable<UserWithAddress> {
    return this._httpClient.put<UserWithAddress>(this._serviceUrl + 'Addresses/' + userWithAddress.userId, userWithAddress);
  }

  // Method to update user
  updateUser(userId: string, user: User): Observable<User> {
    return this._httpClient.put<User>(this._serviceUrl + userId, user);
  }

  uploadUserProfile(formData: FormData) {
    return this._httpClient.post(this._serviceUrl + 'UploadProfile', formData, {reportProgress: true, observe: 'events'});
  }

  // Method to delete user
  deleteUser(userId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + userId);
  }
}
