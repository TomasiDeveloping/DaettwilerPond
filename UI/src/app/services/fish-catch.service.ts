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

  // API base URL obtained from environment
  private readonly _serviceUrl: string = environment.apiBaseUrl + '/Catches/';

  // Injecting necessary services
  private readonly _httpClient: HttpClient = inject(HttpClient);

  // Method to get the catch for the current day
  getCatchForCurrentDay(licenceId: string): Observable<FishCatchModel> {
    return this._httpClient.get<FishCatchModel>(this._serviceUrl + 'GetCatchForCurrentDay/' + licenceId);
  }

  // Method to check if a catch date exists for a specific licence and date
  checkCatchDateExists(licenceId: string, catchDate: string): Observable<boolean> {
    return this._httpClient.get<boolean>(this._serviceUrl + 'CheckCatchDateExists/' + licenceId + '/' + catchDate);
  }

  // Method to get the yearly catch for a specific licence
  getYearlyCatch(licenceId: string): Observable<YearlyCatchModel> {
    return this._httpClient.get<YearlyCatchModel>(this._serviceUrl + 'GetYearlyCatch/' + licenceId);
  }

  // Method to get detailed yearly catches for a specific licence
  getDetailYearlyCatches(licenceId: string): Observable<DetailYearlyCatchModel[]> {
    return this._httpClient.get<DetailYearlyCatchModel[]>(this._serviceUrl + 'GetDetailYearlyCatches/' + licenceId);
  }

  // Method to get catches for a specific month
  getCatchesForMonth(licenceId: string, month: number): Observable<FishCatchModel[]> {
    return this._httpClient.get<FishCatchModel[]>(this._serviceUrl + 'GetCatchesForMonth/' + licenceId + '/' + month);
  }

  // Method to update an existing fish catch
  updateCatch(fishCatchId: string, fishCatch: FishCatchModel): Observable<FishCatchModel> {
    return this._httpClient.put<FishCatchModel>(this._serviceUrl + fishCatchId, fishCatch);
  }

  // Method to create a new fish catch (manual entry)
  createFishCatch(manualCatch: ManualCatchModel): Observable<FishCatchModel> {
    return this._httpClient.post<FishCatchModel>(this._serviceUrl, manualCatch);
  }

  // Method to start a fishing day
  startFishCatch(licenceId: string): Observable<FishCatchModel> {
    return this._httpClient.get<FishCatchModel>(this._serviceUrl + 'StartFishingDay/' + licenceId);
  }

  // Method to stop a fishing day
  stopFishingCatch(catchId: string): Observable<FishCatchModel> {
    return this._httpClient.get<FishCatchModel>(this._serviceUrl + 'StopFishingDay/' + catchId);
  }

  // Method to continue a fishing day
  continueFishingCatch(catchId: string): Observable<FishCatchModel> {
    return this._httpClient.get<FishCatchModel>(this._serviceUrl + 'ContinueFishingDay/' + catchId);
  }

  // Method to delete a fish catch
  deleteFishCatch(fishCatchId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + fishCatchId);
  }
}
