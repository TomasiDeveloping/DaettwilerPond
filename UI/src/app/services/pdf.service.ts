import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpResponse} from "@angular/common/http";
import {map, Observable} from "rxjs";
import {FishingLicenseCreateBill} from "../models/fishingLicenseCreateBill.model";
import {MembersEmailModel} from "../models/membersEmail.model";

@Injectable({
  providedIn: 'root'
})
export class PdfService {

  // API base URL obtained from environment
  private readonly _serviceUrl: string = environment.apiBaseUrl + '/Services/';

  // Injecting necessary services
  private readonly _httpClient: HttpClient = inject(HttpClient);


  // Method to get member PDF
  getMemberPdf(): Observable<{ image: Blob, filename: string | null }> {
    return this._httpClient.get(this._serviceUrl + 'GetMemberPdf/', {observe: 'response', responseType: 'blob'})
      .pipe(map((response: HttpResponse<Blob>) => {
        return {
          image: new Blob([response.body!], {type: response.headers.get('Content-Type')!}),
          filename: response.headers.get('x-file-name')
        }
      }));
  }

  // Method to get fishing rules PDF
  getFishingRulesPdf(): Observable<{ image: Blob, filename: string | null }> {
    return this._httpClient.get(this._serviceUrl + 'GetFishingRulesPdf/', {observe: 'response', responseType: 'blob'})
      .pipe(map((response: HttpResponse<Blob>) => {
        return {
          image: new Blob([response.body!], {type: response.headers.get('Content-Type')!}),
          filename: response.headers.get('x-file-name')
        }
      }));
  }

  // Method to get fish open season PDF
  getFishOpenSeasonPdf(): Observable<{ image: Blob, filename: string | null }> {
    return this._httpClient.get(this._serviceUrl + 'GetFishOpenSeasonPdf/', {observe: 'response', responseType: 'blob'})
      .pipe(map((response: HttpResponse<Blob>) => {
        return {
          image: new Blob([response.body!], {type: response.headers.get('Content-Type')!}),
          filename: response.headers.get('x-file-name')
        }
      }));
  }

  // Method to get user invoice for a fishing license
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

  // Method to send fishing license invoice
  sendFishingLicenseBill(createFishingLicenseBill: FishingLicenseCreateBill): Observable<boolean> {
    return this._httpClient.post<boolean>(this._serviceUrl + 'SendFishingLicenseInvoice', createFishingLicenseBill);
  }

  // Method to send member emails
  sendMemberEmails(membersEmail: MembersEmailModel): Observable<boolean> {
    const formData = new FormData();
    formData.append('MailContent', membersEmail.mailContent);
    formData.append('Subject', membersEmail.subject);
    membersEmail.receiverAddresses.forEach((address: string): void => {
      formData.append('ReceiverAddresses', address)
    });
    membersEmail.attachments.forEach((file): void => {
      formData.append('Attachments', file, file.name)
    });
    return this._httpClient.post<boolean>(this._serviceUrl + 'SendMembersEmail', formData);
  }
}
