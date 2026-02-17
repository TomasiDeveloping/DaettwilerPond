import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import { HttpClient, HttpResponse } from "@angular/common/http";
import {map, Observable} from "rxjs";
import {OverseerCatchDetailsYearModel} from "../models/overseerCatchDetailsYear.model";
import {OverseerMemberDetailsModel} from "../models/overseerMemberDetails.model";
import {
  OverseerValidateLicenseComponent
} from "../pages/overseer/overseer-validate-license/overseer-validate-license.component";

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

  public getMemberDetails(userId: string, year: number): Observable<OverseerMemberDetailsModel> {
    return this._httpClient.get<OverseerMemberDetailsModel>(this._serviceUrl + 'GetMemberDetail/' + year + '/' + userId);
  }

  public getYearlyExcelReport(year: number): Observable<{ image: Blob, filename: string | null }>  {
    return this._httpClient.get(this._serviceUrl + 'GetYearlyExcelReport/' + year, {observe: 'response', responseType: 'blob'})
      .pipe(map((response: HttpResponse<Blob>) => {
        return {
          image: new Blob([response.body!], {type: response.headers.get('Content-Type')!}),
          filename: response.headers.get('x-file-name')
        }
      }));
  }

  public getYearlyMemberExcelReport(year: number, userId: string): Observable<{ image: Blob, filename: string | null }>  {
    return this._httpClient.get(this._serviceUrl + 'GetYearlyUserExcelReport/' + year + '/' + userId, {observe: 'response', responseType: 'blob'})
      .pipe(map((response: HttpResponse<Blob>) => {
        return {
          image: new Blob([response.body!], {type: response.headers.get('Content-Type')!}),
          filename: response.headers.get('x-file-name')
        }
      }));
  }

  public validateLicense(licenseId: string): Observable<OverseerMemberDetailsModel> {
    return this._httpClient.get<OverseerMemberDetailsModel>(this._serviceUrl + 'ValidateLicense/' + licenseId);
  }
}
