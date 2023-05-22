import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Address} from "../models/address.model";

@Injectable({
  providedIn: 'root'
})
export class AddressService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + '/Addresses/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getAddresses(): Observable<Address[]> {
    return this._httpClient.get<Address[]>(this._serviceUrl);
  }

  getAddress(addressId: string): Observable<Address> {
    return this._httpClient.get<Address>(this._serviceUrl + addressId);
  }

  getUserAddresses(userId: string): Observable<Address[]> {
    return this._httpClient.get<Address[]>(this._serviceUrl + `users/${userId}`);
  }
}
