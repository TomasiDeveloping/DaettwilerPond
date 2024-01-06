import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {FishCatchModel} from "../models/fishCatch.model";
import {Observable} from "rxjs";
import {YearlyCatchModel} from "../models/yearlyCatch.model";

@Injectable({
  providedIn: 'root'
})
export class FishCatchService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + '/Catches/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getCatchForCurrentDay(licenceId: string): Observable<FishCatchModel> {
    return this._httpClient.get<FishCatchModel>(this._serviceUrl + 'GetCatchForCurrentDay/' + licenceId);
  }

  getYearlyCatch(licenceId: string): Observable<YearlyCatchModel> {
    return this._httpClient.get<YearlyCatchModel>(this._serviceUrl + 'GetYearlyCatch/' + licenceId);
  }

  updateCatch(fishCatch: FishCatchModel): Observable<FishCatchModel> {
    return this._httpClient.put<FishCatchModel>(this._serviceUrl + fishCatch.id, fishCatch);
  }
  createFishCatch(fishCatch: FishCatchModel): Observable<FishCatchModel> {
    return this._httpClient.post<FishCatchModel>(this._serviceUrl, fishCatch);
  }

  startFishCatch(licenceId: string): Observable<FishCatchModel> {
    return this._httpClient.get<FishCatchModel>(this._serviceUrl + 'StartFishingDay/' + licenceId);
  }

  stopFishingCatch(catchId: string): Observable<FishCatchModel> {
    return this._httpClient.get<FishCatchModel>(this._serviceUrl + 'StopFishingDay/' + catchId);
  }

  continueFishingCatch(catchId: string): Observable<FishCatchModel> {
    return this._httpClient.get<FishCatchModel>(this._serviceUrl + 'ContinueFishingDay/' + catchId);
  }
}
