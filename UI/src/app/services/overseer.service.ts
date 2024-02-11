import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {OverseerCatchDetailsYearModel} from "../models/overseerCatchDetailsYear.model";
import {OverseerMemberDetailsModel} from "../models/overseerMemberDetails.model";

@Injectable({
  providedIn: 'root'
})
export class OverseerService {

  // API base URL obtained from environment
  private readonly _serviceUrl: string = environment.apiBaseUrl + '/overseers/';

  // Injecting necessary services
  private readonly _httpClient: HttpClient = inject(HttpClient);

  public getYearlyDetails(year: number): Observable<OverseerCatchDetailsYearModel> {
    return this._httpClient.get<OverseerCatchDetailsYearModel>(this._serviceUrl + 'GetDetailYearlyCatch/' + year);
  }

  public getMemberDetails(userId: string): Observable<OverseerMemberDetailsModel> {
    return this._httpClient.get<OverseerMemberDetailsModel>(this._serviceUrl + 'GetMemberDetail/' + userId);
  }
}
