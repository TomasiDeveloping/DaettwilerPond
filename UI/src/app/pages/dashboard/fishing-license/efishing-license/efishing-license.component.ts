import {Component, Inject, inject} from '@angular/core';
import {FishingLicense} from "../../../../models/fishingLicense.model";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {environment} from "../../../../../environments/environment";


@Component({
  selector: 'app-efishing-license',
  templateUrl: './efishing-license.component.html',
  styleUrl: './efishing-license.component.scss'
})
export class EFishingLicenseComponent {

  public currentFishingLicense: FishingLicense;

  private readonly _dialogRef: MatDialogRef<EFishingLicenseComponent> = inject(MatDialogRef<EFishingLicenseComponent>);

  constructor(@Inject(MAT_DIALOG_DATA) public data: {fishingLicense: FishingLicense}) {
    this.currentFishingLicense = data.fishingLicense;
    if (!this.currentFishingLicense) {
      this.onClose();
    }
  }
  onClose() {
    this._dialogRef.close();
  }

  protected readonly environment = environment;
}
