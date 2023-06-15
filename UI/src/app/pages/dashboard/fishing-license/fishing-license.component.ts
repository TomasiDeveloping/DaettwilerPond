import {Component, inject, Input, OnChanges, SimpleChanges} from '@angular/core';
import {FishingLicense} from "../../../models/fishingLicense.model";
import * as moment from "moment";
import {PdfService} from "../../../services/pdf.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-fishing-license',
  templateUrl: './fishing-license.component.html',
  styleUrls: ['./fishing-license.component.scss']
})
export class FishingLicenseComponent implements OnChanges {

  @Input() public fishingLicence: FishingLicense | undefined;
  public expiresInDays: number | undefined;

  private readonly _pdfService: PdfService = inject(PdfService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  ngOnChanges(changes: SimpleChanges) {
    if (this.fishingLicence) {
      if (new Date(this.fishingLicence.expiresOn) > new Date()) {
        this.expiresInDays = moment(this.fishingLicence?.expiresOn).diff(Date.now(), 'days');
      }
    }
  }

  onDownloadInvoice(fishingLicenseId: string) {
    this._pdfService.getUserInvoiceFishingLicense(fishingLicenseId).subscribe({
      next: ((response: {image: Blob, filename: string | null}) => {
        const fileUrl = URL.createObjectURL(response.image);
        const anchorElement = document.createElement('a');
        anchorElement.href = fileUrl;
        anchorElement.target = '_blank';
        anchorElement.download = response.filename!;
        document.body.appendChild(anchorElement);
        anchorElement.click();
      }),
      error: () =>{
        this._toastr.error('Fehler beim Download', 'PDF Rechnung');
      }
    });
  }
}
