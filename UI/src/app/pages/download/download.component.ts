import {Component, inject} from '@angular/core';
import {PdfService} from "../../services/pdf.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-download',
  templateUrl: './download.component.html',
  styleUrls: ['./download.component.scss']
})
export class DownloadComponent {

  // Private properties for PdfService and ToastrService using Angular DI
  private readonly _pdfService: PdfService = inject(PdfService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  // Method to download the Members list PDF
  onDownloadMember(): void {
    this._pdfService.getMemberPdf().subscribe({
      next: ((response: {image: Blob, filename: string | null}): void => {
        this.downloadPdf(response);
      }),
      // Handling errors and displaying toastr messages
      error: (): void => {
        this._toastr.error('Fehler beim Download', 'Mitgliederliste');
      }
    });
  }

  // Method to download the Open Season PDF
  onDownloadOpenSeason(): void {
    this._pdfService.getFishOpenSeasonPdf().subscribe({
      next: ((response: {image: Blob, filename: string | null}): void => {
        this.downloadPdf(response);
      }),
      // Handling errors and displaying toastr messages
      error: (): void => {
        this._toastr.error('Fehler beim Download', 'Schonmasse');
      }
    });
  }

  // Method to download the Fishing Rules PDF
  onDownloadFishingRules(): void {
    this._pdfService.getFishingRulesPdf().subscribe({
      next: ((response: {image: Blob, filename: string | null}): void => {
        this.downloadPdf(response);
      }),
      // Handling errors and displaying toastr messages
      error: (): void =>{
        this._toastr.error('Fehler beim Download', 'Vorschriften');
      }
    });
  }

  // Method to download the Crab Plague PDF from the assets folder
  onDownloadCrabPlague(): void {
    const anchorElement: HTMLAnchorElement = document.createElement('a');
    anchorElement.download = "Vorsichtsmassnahmen_gegen_die_Krebspest.pdf";
    anchorElement.href = '../../../assets/documents/CrabPlaque.pdf';
    anchorElement.click();
  }


  // Private method to handle the actual PDF download process
  private downloadPdf(data: any): void{
    const fileUrl: string = URL.createObjectURL(data.image);
    const anchorElement: HTMLAnchorElement = document.createElement('a');
    anchorElement.href = fileUrl;
    anchorElement.target = '_blank';
    anchorElement.download = data.filename;
    document.body.appendChild(anchorElement);
    anchorElement.click();
  }

}
