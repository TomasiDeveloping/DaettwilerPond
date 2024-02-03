import {Component, Inject, inject} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {FishType} from "../../../models/fishType.model";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FishTypeService} from "../../../services/fish-type.service";
import {CatchDetailService} from "../../../services/catch-detail.service";
import {CatchDetailModel} from "../../../models/catchDetail.model";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-catchDay-add-catch',
  templateUrl: './catchDay-add-catch.component.html'
})
export class CatchDayAddCatchComponent {

  // Initialize FormGroup and other variables
  public catchForm: FormGroup;
  public fishTypes: FishType[] = [];

  // Inject necessary services and dependencies
  private readonly _dialogRef: MatDialogRef<CatchDayAddCatchComponent> = inject(MatDialogRef<CatchDayAddCatchComponent>);
  private readonly _fishTypeService: FishTypeService = inject(FishTypeService);
  private readonly _catchDetailService: CatchDetailService = inject(CatchDetailService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { catchId: string }) {
    // Initialize the reactive form with default values and validators
    this.catchForm = new FormGroup({
      catchId: new FormControl<string>(data.catchId, [Validators.required]),
      fishTypeId: new FormControl<string>('', [Validators.required]),
      amount: new FormControl<number>(1, [Validators.required]),
      hadCrabs: new FormControl<boolean>(false, [Validators.required])
    });

    // Fetch fish types from the service and populate the fishTypes array
    this._fishTypeService.getFishTypes().subscribe({
      next: ((response: FishType[]): void => {
        if (response) {
          this.fishTypes = response;
        }
      })
    });
  }

  // Handle form submission
  onSubmit(): void {
    // Check if the form is invalid before proceeding
    if (this.catchForm.invalid) {
      return;
    }

    // Convert form values to CatchDetailModel for submission
    const catchDetail: CatchDetailModel = this.catchForm.value as CatchDetailModel;

    // Call the service to create a new catch detail
    this._catchDetailService.createCatchDetail(catchDetail).subscribe({
      next: ((response: CatchDetailModel): void => {
        if (response) {
          // Display a success toastr message and close the dialog
          this._toastr.success('Fang wurde erfolgreich erfasst', 'Fang hinzufügen');
          this.onClose(true);
        }
      }),
      error: (): void => {
        // Display an error toastr message if there's an issue with creating the catch detail
        this._toastr.error('Fang konnte nicht erfasst werden', 'Fang hinzufügen');
      }
    });
  }

  // Close the dialog and optionally trigger a reload of the parent component
  onClose(reload: boolean): void {
    this._dialogRef.close(reload);
  }
}
