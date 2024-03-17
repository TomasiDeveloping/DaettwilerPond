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

  // Injecting MatDialogRef for dialog reference
  private readonly _dialogRef: MatDialogRef<OverseerValidationResultComponent> = inject(MatDialogRef<OverseerValidationResultComponent>);

  constructor(@Inject(MAT_DIALOG_DATA) public data: {validation: OverseerLicenseValidationModel}) {
    // Assigning validation data received through dialog
    this.validation = data.validation;
  }

  // Method to close the dialog
  onClose(): void {
    this._dialogRef.close();
  }
}
