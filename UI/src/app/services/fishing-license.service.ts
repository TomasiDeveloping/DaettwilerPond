import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {FishingLicense} from "../models/fishingLicense.model";
import {ValidateLicenseModel} from "../models/validateLicense.model";

@Injectable({
  providedIn: 'root'
})
export class FishingLicenseService {

  // API base URL obtained from environment
  private readonly _serviceUrl: string = environment.apiBaseUrl + '/FishingLicenses/';

  // Injecting necessary services
  private readonly _httpClient: HttpClient = inject(HttpClient);


  // Method to get all fishing licenses
  getFishingLicenses(): Observable<FishingLicense[]> {
    return this._httpClient.get<FishingLicense[]>(this._serviceUrl);
  }

  // Method to get fishing licenses for a specific user
  getUserLicenses(userId: string): Observable<FishingLicense[]> {
    return this._httpClient.get<FishingLicense[]>(this._serviceUrl + 'Users/' + userId);
  }

  // Method to get the current user's fishing license
  getCurrentUserLicence(userId: string): Observable<FishingLicense> {
    return this._httpClient.get<FishingLicense>(this._serviceUrl + 'Users/GetCurrentUserLicense/' + userId);
  }

  // Method to create a new fishing license
  createFishingLicence(fishingLicence: FishingLicense): Observable<FishingLicense> {
    return this._httpClient.post<FishingLicense>(this._serviceUrl, fishingLicence);
  }

  // Method to update an existing fishing license
  updateFishingLicense(fishingLicenseId: string, fishingLicence: FishingLicense): Observable<FishingLicense> {
    return this._httpClient.put<FishingLicense>(this._serviceUrl + fishingLicenseId, fishingLicence);
  }

  // Method to delete a fishing license
  deleteFishingLicence(fishingLicenseId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + fishingLicenseId);
  }

  validateLicense(licenseId: string): Observable<ValidateLicenseModel> {
    return this._httpClient.get<ValidateLicenseModel>(this._serviceUrl + 'Validate/' + licenseId);
  }
}
