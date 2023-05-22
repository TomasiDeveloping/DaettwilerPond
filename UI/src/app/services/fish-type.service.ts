import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {FishType} from "../models/fishType.model";

@Injectable({
  providedIn: 'root'
})
export class FishTypeService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + '/fishTypes/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getFishTypes(): Observable<FishType[]> {
    return this._httpClient.get<FishType[]>(this._serviceUrl);
  }

  createFishType(fishType: FishType): Observable<FishType> {
    return this._httpClient.post<FishType>(this._serviceUrl, fishType);
  }

  updateFishType(fishTypeId: string, fishType: FishType): Observable<FishType> {
    return this._httpClient.put<FishType>(this._serviceUrl + fishTypeId, fishType);
  }

  deleteFishType(fishTypeId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + fishTypeId);
  }
}
