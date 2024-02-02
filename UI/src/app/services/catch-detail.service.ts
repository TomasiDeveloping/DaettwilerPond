import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {CatchDetailModel} from "../models/catchDetail.model";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CatchDetailService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + '/CatchDetails/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  createCatchDetail(catchDetail: CatchDetailModel): Observable<CatchDetailModel> {
    return this._httpClient.post<CatchDetailModel>(this._serviceUrl, catchDetail);
  }

  updateCatchDetail(catchDetailId: string, catchDetail: CatchDetailModel): Observable<CatchDetailModel> {
    return this._httpClient.put<CatchDetailModel>(this._serviceUrl + catchDetailId, catchDetail);
  }

  deleteCatchDetail(catchDetailId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + catchDetailId);
  }
}
