import {Component, inject, Input, OnChanges, SimpleChanges} from '@angular/core';
import {FishingLicense} from "../../../models/fishingLicense.model";
import * as moment from "moment";
import {PdfService} from "../../../services/pdf.service";
import {ToastrService} from "ngx-toastr";
import {MatDialog} from "@angular/material/dialog";
import {EFishingLicenseComponent} from "./efishing-license/efishing-license.component";
import {AuthenticationService} from "../../../services/authentication.service";

@Component({
  selector: 'app-fishing-license',
  templateUrl: './fishing-license.component.html',
  styleUrls: ['./fishing-license.component.scss']
})
export class FishingLicenseComponent implements OnChanges {
  constructor() {
    this.isAdmin = this._authService.isUserAdministrator();
  }

  // Input property to receive the fishing license data
  @Input() public fishingLicence: FishingLicense | undefined;

  // Properties to store the remaining days and hours until the fishing license expires
  public expiresInDays: number | undefined;
  public expiresInHours: number | undefined;

  public isAdmin: boolean = false;

  // Private properties for PdfService and ToastrService using Angular DI
  private readonly _pdfService: PdfService = inject(PdfService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _dialog: MatDialog = inject(MatDialog);
  private readonly _authService: AuthenticationService = inject(AuthenticationService);

  // OnChanges lifecycle hook to execute logic when input properties change
  ngOnChanges(_: SimpleChanges): void {
    // Checking if fishingLicence is available and not expired
    if (this.fishingLicence) {
      if (new Date(this.fishingLicence.expiresOn) > new Date()) {
        // Calculating remaining days until expiration
        this.expiresInDays = moment(this.fishingLicence?.expiresOn).diff(Date.now(), 'days');

        // Checking if the remaining days is 0 to calculate remaining hours
        if (this.expiresInDays === 0) {
          this.expiresInHours = moment(this.fishingLicence?.expiresOn).diff(new Date(), 'hours');
        }
      }
    }
  }

  // Method to download the invoice for the fishing license
  onDownloadInvoice(fishingLicenseId: string) {
    this._pdfService.getUserInvoiceFishingLicense(fishingLicenseId).subscribe({
      next: ((response: { image: Blob, filename: string | null }): void => {
        // Creating a download link for the PDF invoice and triggering the download
        const fileUrl: string = URL.createObjectURL(response.image);
        const anchorElement: HTMLAnchorElement = document.createElement('a');
        anchorElement.href = fileUrl;
        anchorElement.target = '_blank';
        anchorElement.download = response.filename!;
        document.body.appendChild(anchorElement);
        anchorElement.click();
      }),
      // Handling errors and displaying toastr messages
      error: (): void => {
        this._toastr.error('Fehler beim Download', 'PDF Rechnung');
      }
    });
  }

  onOpenELicense(fishingLicence: FishingLicense) {
    this._dialog.open(EFishingLicenseComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {fishingLicense: fishingLicence}
    })
  }
}
