import {Component, inject, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {CatchDetailModel} from "../../../models/catchDetail.model";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {FishTypeService} from "../../../services/fish-type.service";
import {ToastrService} from "ngx-toastr";
import {CatchDetailService} from "../../../services/catch-detail.service";
import {FishType} from "../../../models/fishType.model";

@Component({
  selector: 'app-catch-day-edit-catch',
  templateUrl: './catch-day-edit-catch.component.html'
})
export class CatchDayEditCatchComponent {

  // Public properties for the current catch detail, form group, and fish types
  public currentCatchDetail: CatchDetailModel;
  public catchDetailForm: FormGroup;
  public fishTypes: FishType[] = [];

  // Injected services for fish type, dialog reference, toastr, and catch detail
  private readonly _fishTypeService: FishTypeService = inject(FishTypeService);
  private readonly _dialogRef: MatDialogRef<CatchDayEditCatchComponent> = inject(MatDialogRef<CatchDayEditCatchComponent>);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _catchDetailService: CatchDetailService = inject(CatchDetailService);

  // Constructor with injected data for the catch detail
  constructor(@Inject(MAT_DIALOG_DATA) public data: { catchDetail: CatchDetailModel }) {
    // Initialize current catch detail with data from the dialog
    this.currentCatchDetail = data.catchDetail;

    // Create the form group with form controls for fish type and had crabs
    this.catchDetailForm = new FormGroup({
      fishTypeId: new FormControl<string>(data.catchDetail.fishTypeId, [Validators.required]),
      hadCrabs: new FormControl<boolean>(data.catchDetail.hadCrabs, [Validators.required])
    });

    // Fetch the list of fish types from the FishTypeService
    this._fishTypeService.getFishTypes().subscribe({
      next: ((response: FishType[]): void => {
        if (response) {
          this.fishTypes = response;
        }
      })
    });
  }

  // Method called when the form is submitted
  onSubmit(): void {
    // Check if the form is invalid
    if (this.catchDetailForm.invalid) {
      return;
    }

    // Update the current catch detail with values from the form
    this.currentCatchDetail.fishTypeId = this.catchDetailForm.get('fishTypeId')?.value;
    this.currentCatchDetail.hadCrabs = this.catchDetailForm.get('hadCrabs')?.value;

    // Call the CatchDetailService to update the catch detail
    this._catchDetailService.updateCatchDetail(this.currentCatchDetail.id, this.currentCatchDetail).subscribe({
      next: ((response: CatchDetailModel): void => {
        if (response) {
          // Display success toastr message and close the dialog with reload parameter set to true
          this._toastr.success('Fang wurde erfolgreich bearbeitet', 'Fang bearbeiten');
          this.onClose(true);
        }
      }),
      error: (): void => {
        // Display error toastr message if the update fails
        this._toastr.error('Fang konnte nicht bearbeitet werden', 'Fang bearbeiten');
      }
    });
  }

  // Method called to close the dialog with an optional reload parameter
  onClose(reload: boolean | null): void {
    this._dialogRef.close(reload);
  }
}
