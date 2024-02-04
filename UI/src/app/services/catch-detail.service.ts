import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {CatchDetailModel} from "../models/catchDetail.model";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CatchDetailService {

  // API base URL obtained from environment
  private readonly _serviceUrl: string = environment.apiBaseUrl + '/CatchDetails/';

  // Injecting necessary services
  private readonly _httpClient: HttpClient = inject(HttpClient);

  // Method to create a new catch detail
  createCatchDetail(catchDetail: CatchDetailModel): Observable<CatchDetailModel> {
    return this._httpClient.post<CatchDetailModel>(this._serviceUrl, catchDetail);
  }

  // Method to update an existing catch detail by ID
  updateCatchDetail(catchDetailId: string, catchDetail: CatchDetailModel): Observable<CatchDetailModel> {
    return this._httpClient.put<CatchDetailModel>(this._serviceUrl + catchDetailId, catchDetail);
  }

  // Method to delete a catch detail by ID
  deleteCatchDetail(catchDetailId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + catchDetailId);
  }
}
