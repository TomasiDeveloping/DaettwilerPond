import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {FishingRegulation} from "../models/fishingRegulation.model";

@Injectable({
  providedIn: 'root'
})
export class FishingRegulationService {

  // API base URL obtained from environment
  private readonly _serviceUrl: string = environment.apiBaseUrl + '/FishingRegulations/';

  // Injecting necessary services
  private readonly _httpClient: HttpClient = inject(HttpClient);


  // Method to get all fishing regulations
  getFishingRegulations(): Observable<FishingRegulation[]> {
    return this._httpClient.get<FishingRegulation[]>(this._serviceUrl);
  }

  // Method to create a new fishing regulation
  createFishingRegulation(fishingRegulation: FishingRegulation): Observable<FishingRegulation> {
    return this._httpClient.post<FishingRegulation>(this._serviceUrl, fishingRegulation);
  }

  // Method to update an existing fishing regulation
  updateFishingRegulation(fishingRegulationId: string, fishingRegulation: FishingRegulation): Observable<FishingRegulation> {
    return this._httpClient.put<FishingRegulation>(this._serviceUrl + fishingRegulationId, fishingRegulation);
  }

  // Method to delete a fishing regulation
  deleteFishingRegulation(fishingRegulationId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + fishingRegulationId);
  }
}
