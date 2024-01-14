import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {FishCatchModel} from "../models/fishCatch.model";
import {Observable} from "rxjs";
import {YearlyCatchModel} from "../models/yearlyCatch.model";
import {ManualCatchModel} from "../models/manualCatch.model";
import {DetailYearlyCatchModel} from "../models/detailYearlyCatch.model";

@Injectable({
  providedIn: 'root'
})
export class FishCatchService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + '/Catches/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getCatchForCurrentDay(licenceId: string): Observable<FishCatchModel> {
    return this._httpClient.get<FishCatchModel>(this._serviceUrl + 'GetCatchForCurrentDay/' + licenceId);
  }

  checkCatchDateExists(licenceId: string, catchDate: string): Observable<boolean> {
    return this._httpClient.get<boolean>(this._serviceUrl + 'CheckCatchDateExists/' + licenceId + '/' + catchDate);
  }

  getYearlyCatch(licenceId: string): Observable<YearlyCatchModel> {
    return this._httpClient.get<YearlyCatchModel>(this._serviceUrl + 'GetYearlyCatch/' + licenceId);
  }

  getDetailYearlyCatches(licenceId: string): Observable<DetailYearlyCatchModel[]> {
    return this._httpClient.get<DetailYearlyCatchModel[]>(this._serviceUrl + 'GetDetailYearlyCatches/' + licenceId);
  }

  getCatchesForMonth(licenceId: string, month: number): Observable<FishCatchModel[]> {
    return this._httpClient.get<FishCatchModel[]>(this._serviceUrl + 'GetCatchesForMonth/' + licenceId + '/' + month);
  }

  updateCatch(fishCatch: FishCatchModel): Observable<FishCatchModel> {
    return this._httpClient.put<FishCatchModel>(this._serviceUrl + fishCatch.id, fishCatch);
  }

  createFishCatch(manualCatch: ManualCatchModel): Observable<FishCatchModel> {
    return this._httpClient.post<FishCatchModel>(this._serviceUrl, manualCatch);
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
