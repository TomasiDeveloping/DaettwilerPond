import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {FishingClub} from "../models/fishingClub.model";

@Injectable({
  providedIn: 'root'
})
export class FishingClubService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + '/FishingClubs/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getFishingClubs(): Observable<FishingClub[]> {
    return this._httpClient.get<FishingClub[]>(this._serviceUrl);
  }

  createFishingClub(fishingClub: FishingClub): Observable<FishingClub> {
    return this._httpClient.post<FishingClub>(this._serviceUrl, fishingClub);
  }

  updateFishingClub(fishingClubId: string, fishingClub: FishingClub): Observable<FishingClub> {
    return this._httpClient.put<FishingClub>(this._serviceUrl + fishingClubId, fishingClub);
  }

  deleteFishingClub(fishingClubId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + fishingClubId);
  }
}
