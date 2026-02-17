import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import {Observable} from "rxjs";
import {FishType} from "../models/fishType.model";

@Injectable({
  providedIn: 'root'
})
export class FishTypeService {

  // API base URL obtained from environment
  private readonly _serviceUrl: string = environment.apiBaseUrl + '/fishTypes/';

  // Injecting necessary services
  private readonly _httpClient: HttpClient = inject(HttpClient);


  // Method to get all fish types
  getFishTypes(): Observable<FishType[]> {
    return this._httpClient.get<FishType[]>(this._serviceUrl);
  }

  // Method to create a new fish type
  createFishType(fishType: FishType): Observable<FishType> {
    return this._httpClient.post<FishType>(this._serviceUrl, fishType);
  }

  // Method to update an existing fish type
  updateFishType(fishTypeId: string, fishType: FishType): Observable<FishType> {
    return this._httpClient.put<FishType>(this._serviceUrl + fishTypeId, fishType);
  }

  // Method to delete a fish type
  deleteFishType(fishTypeId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + fishTypeId);
  }
}
