import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {FishingRegulation} from "../models/fishingRegulation.model";

@Injectable({
  providedIn: 'root'
})
export class FishingRegulationService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + '/FishingRegulations/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getFishingRegulations(): Observable<FishingRegulation[]> {
    return this._httpClient.get<FishingRegulation[]>(this._serviceUrl);
  }

  createFishingRegulation(fishingRegulation: FishingRegulation): Observable<FishingRegulation> {
    return this._httpClient.post<FishingRegulation>(this._serviceUrl, fishingRegulation);
  }

  updateFishingRegulation(fishingRegulationId: string, fishingRegulation: FishingRegulation): Observable<FishingRegulation> {
    return this._httpClient.put<FishingRegulation>(this._serviceUrl + fishingRegulationId, fishingRegulation);
  }

  deleteFishingRegulation(fishingRegulationId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + fishingRegulationId);
  }
}
