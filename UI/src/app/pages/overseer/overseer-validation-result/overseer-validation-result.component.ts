import {Component, inject, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {OverseerLicenseValidationModel} from "../../../models/overseerLicenseValidation.model";

@Component({
  selector: 'app-overseer-validation-result',
  templateUrl: './overseer-validation-result.component.html',
  styleUrl: './overseer-validation-result.component.scss'
})
export class OverseerValidationResultComponent {

  public validation: OverseerLicenseValidationModel;

  private readonly _dialogRef: MatDialogRef<OverseerValidationResultComponent> = inject(MatDialogRef<OverseerValidationResultComponent>);

  constructor(@Inject(MAT_DIALOG_DATA) public data: {validation: OverseerLicenseValidationModel}) {
    this.validation = data.validation;
  }

  onClose() {
    this._dialogRef.close();
  }
}
