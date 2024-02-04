import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Address} from "../models/address.model";

@Injectable({
  providedIn: 'root'
})
export class AddressService {

  // API base URL obtained from environment
  private readonly _serviceUrl: string = environment.apiBaseUrl + '/Addresses/';

  // Injecting the HttpClient service
  private readonly _httpClient: HttpClient = inject(HttpClient);

  // Retrieve addresses for a specific user
  getUserAddresses(userId: string): Observable<Address[]> {
    return this._httpClient.get<Address[]>(this._serviceUrl + `users/${userId}`);
  }

  // Update an existing address
  updateAddress(addressId: string, address: Address): Observable<Address> {
    return this._httpClient.put<Address>(this._serviceUrl + addressId, address);
  }
}
