import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpResponse} from "@angular/common/http";
import {map, Observable} from "rxjs";
import {FishingLicenseCreateBill} from "../models/fishingLicenseCreateBill.model";

@Injectable({
  providedIn: 'root'
})
export class PdfService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + '/Services/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getMemberPdf(): Observable<{ image: Blob, filename: string | null }> {
    return this._httpClient.get(this._serviceUrl + 'GetMemberPdf/', {observe: 'response', responseType: 'blob'})
      .pipe(map((response: HttpResponse<Blob>) => {
        return {
          image: new Blob([response.body!], {type: response.headers.get('Content-Type')!}),
          filename: response.headers.get('x-file-name')
        }
      }));
  }

  getFishingRulesPdf(): Observable<{ image: Blob, filename: string | null }> {
    return this._httpClient.get(this._serviceUrl + 'GetFishingRulesPdf/', {observe: 'response', responseType: 'blob'})
      .pipe(map((response: HttpResponse<Blob>) => {
        return {
          image: new Blob([response.body!], {type: response.headers.get('Content-Type')!}),
          filename: response.headers.get('x-file-name')
        }
      }));
  }

  getFishOpenSeasonPdf(): Observable<{ image: Blob, filename: string | null }> {
    return this._httpClient.get(this._serviceUrl + 'GetFishOpenSeasonPdf/', {observe: 'response', responseType: 'blob'})
      .pipe(map((response: HttpResponse<Blob>) => {
        return {
          image: new Blob([response.body!], {type: response.headers.get('Content-Type')!}),
          filename: response.headers.get('x-file-name')
        }
      }));
  }

  getUserInvoiceFishingLicense(fishingLicenseId: string): Observable<{ image: Blob, filename: string | null }> {
    return this._httpClient.get(this._serviceUrl + 'GetUserInvoiceFishingLicense/' + fishingLicenseId, {
      observe: 'response',
      responseType: 'blob'
    })
      .pipe(map((response: HttpResponse<Blob>) => {
        return {
          image: new Blob([response.body!], {type: response.headers.get('Content-Type')!}),
          filename: response.headers.get('x-file-name')
        }
      }));
  }

  sendFishingLicenseBill(createFishingLicenseBill: FishingLicenseCreateBill): Observable<boolean> {
    return this._httpClient.post<boolean>(this._serviceUrl + 'SendFishingLicenseInvoice', createFishingLicenseBill);
  }
}
