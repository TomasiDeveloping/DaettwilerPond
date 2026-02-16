import {Component, Inject, inject} from '@angular/core';
import {FishingLicense} from "../../../../models/fishingLicense.model";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {environment} from "../../../../../environments/environment";


@Component({
    selector: 'app-efishing-license',
    templateUrl: './efishing-license.component.html',
    styleUrl: './efishing-license.component.scss',
    standalone: false
})
export class EFishingLicenseComponent {

  // Property to store the current fishing license
  public currentFishingLicense: FishingLicense;

  // Injecting MatDialogRef for dialog reference
  private readonly _dialogRef: MatDialogRef<EFishingLicenseComponent> = inject(MatDialogRef<EFishingLicenseComponent>);

  constructor(@Inject(MAT_DIALOG_DATA) public data: {fishingLicense: FishingLicense}) {
    // Initialize currentFishingLicense with the provided fishing license data
    this.currentFishingLicense = data.fishingLicense;

    // Check if fishing license data is available, if not, close the dialog
    if (!this.currentFishingLicense) {
      this.onClose();
    }
  }

  // Method to close the dialog
  onClose(): void {
    this._dialogRef.close();
  }

  // Accessing environment variables
  protected readonly environment = environment;
}
