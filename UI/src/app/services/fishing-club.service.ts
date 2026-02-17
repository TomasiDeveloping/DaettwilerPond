import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import {Observable} from "rxjs";
import {FishingClub} from "../models/fishingClub.model";

@Injectable({
  providedIn: 'root'
})
export class FishingClubService {

  // API base URL obtained from environment
  private readonly _serviceUrl: string = environment.apiBaseUrl + '/FishingClubs/';

  // Injecting necessary services
  private readonly _httpClient: HttpClient = inject(HttpClient);

  // Method to get all fishing clubs
  getFishingClubs(): Observable<FishingClub[]> {
    return this._httpClient.get<FishingClub[]>(this._serviceUrl);
  }

  // Method to create a new fishing club
  createFishingClub(fishingClub: FishingClub): Observable<FishingClub> {
    return this._httpClient.post<FishingClub>(this._serviceUrl, fishingClub);
  }

  // Method to update an existing fishing club
  updateFishingClub(fishingClubId: string, fishingClub: FishingClub): Observable<FishingClub> {
    return this._httpClient.put<FishingClub>(this._serviceUrl + fishingClubId, fishingClub);
  }
}
