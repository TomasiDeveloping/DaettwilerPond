import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {FishingLicense} from "../models/fishingLicense.model";

@Injectable({
  providedIn: 'root'
})
export class FishingLicenseService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + '/FishingLicenses/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getFishingLicenses(): Observable<FishingLicense[]> {
    return this._httpClient.get<FishingLicense[]>(this._serviceUrl);
  }

  getFishingLicense(fishingLicenseId: string): Observable<FishingLicense> {
    return this._httpClient.get<FishingLicense>(this._serviceUrl + fishingLicenseId);
  }

  getUserLicenses(userId: string): Observable<FishingLicense[]> {
    return this._httpClient.get<FishingLicense[]>(this._serviceUrl + 'Users/' + userId);
  }

  getCurrentUserLicence(userId: string): Observable<FishingLicense> {
    return this._httpClient.get<FishingLicense>(this._serviceUrl + 'Users/GetCurrentUserLicense/' + userId);
  }

  createFishingLicence(fishingLicence: FishingLicense): Observable<FishingLicense> {
    return this._httpClient.post<FishingLicense>(this._serviceUrl, fishingLicence);
  }

  updateFishingLicense(fishingLicenseId: string, fishingLicence: FishingLicense): Observable<FishingLicense> {
    return this._httpClient.put<FishingLicense>(this._serviceUrl + fishingLicenseId, fishingLicence);
  }

  deleteFishingLicence(fishingLicenseId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + fishingLicenseId);
  }
}
