import {Component, inject} from '@angular/core';
import {PdfService} from "../../services/pdf.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-download',
  templateUrl: './download.component.html',
  styleUrls: ['./download.component.scss']
})
export class DownloadComponent {

  private readonly _pdfService: PdfService = inject(PdfService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  onDownloadMember() {
    this._pdfService.getMemberPdf().subscribe({
      next: ((response: {image: Blob, filename: string | null}) => {
        this.downloadPdf(response);
      }),
      error: () => {
        this._toastr.error('Fehler beim Download', 'Mitgliederliste');
      }
    });
  }

  onDownloadOpenSeason() {
    this._pdfService.getFishOpenSeasonPdf().subscribe({
      next: ((response: {image: Blob, filename: string | null}) => {
        this.downloadPdf(response);
      }),
      error: () => {
        this._toastr.error('Fehler beim Download', 'Schonmasse');
      }
    });
  }
  onDownloadFishingRules() {
    this._pdfService.getFishingRulesPdf().subscribe({
      next: ((response: {image: Blob, filename: string | null}) => {
        this.downloadPdf(response);
      }),
      error: () =>{
        this._toastr.error('Fehler beim Download', 'Vorschriften');
      }
    });
  }

  onDownloadCrabPlague() {
    const anchorElement = document.createElement('a');
    anchorElement.download = "Vorsichtsmassnahmen_gegen_die_Krebspest.pdf";
    anchorElement.href = '../../../assets/documents/CrabPlaque.pdf';
    anchorElement.click();
  }


  private downloadPdf(data: any){
    const fileUrl = URL.createObjectURL(data.image);
    const anchorElement = document.createElement('a');
    anchorElement.href = fileUrl;
    anchorElement.target = '_blank';
    anchorElement.download = data.filename;
    document.body.appendChild(anchorElement);
    anchorElement.click();
  }

}
