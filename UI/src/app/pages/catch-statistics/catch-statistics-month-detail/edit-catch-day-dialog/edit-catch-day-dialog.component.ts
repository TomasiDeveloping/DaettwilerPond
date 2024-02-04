import {Component, inject, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FishCatchModel} from "../../../../models/fishCatch.model";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {FishCatchService} from "../../../../services/fish-catch.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-edit-catch-day-dialog',
  templateUrl: './edit-catch-day-dialog.component.html'
})
export class EditCatchDayDialogComponent {

  public catchForm: FormGroup;
  public currentCatchDay: FishCatchModel;
  public hours: number[] = [];
  public minutes: number[] = [0, 25, 50, 75];

  // Dependency Injection using inject function
  private readonly _dialogRef: MatDialogRef<EditCatchDayDialogComponent> = inject(MatDialogRef<EditCatchDayDialogComponent>);
  private readonly _fishCatchService: FishCatchService = inject(FishCatchService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  // Injecting data using @Inject(MAT_DIALOG_DATA)
  constructor(@Inject(MAT_DIALOG_DATA) public data: {fishCatch: FishCatchModel}) {
    // Initialize hours array
    for (let i: number = 0; i <= 23; i++) {
      this.hours.push(i);
    }

    // Extract data from the dialog
    this.currentCatchDay = data.fishCatch;
    const splitTime: string[] = data.fishCatch.hoursSpent.toString().split('.');
    const hours: number = +splitTime[0];
    let minutes: number = isNaN(+splitTime[1]) ? 0 : +splitTime[1];

    // Adjust minutes if needed
    if (minutes === 5) {
      minutes = 50;
    }

    // Create the form group
    this.catchForm = new FormGroup({
      id: new FormControl<string>(data.fishCatch.id, [Validators.required]),
      hoursSpent: new FormControl<number>(hours),
      minutesSpent: new FormControl<number>(minutes)
    });
  }

  // Getter for form controls
  get f() {
    return this.catchForm;
  }


  // Handle form submission
  onSubmit(): void {
    if (this.catchForm.invalid) {
      return;
    }

    // Extract hours and minutes from form controls
    const hoursSpent: number = +this.catchForm.get('hoursSpent')?.value;
    const minutesSpent: number = +this.catchForm.get('minutesSpent')?.value;

    // Validate hours and minutes combination
    if (hoursSpent === 0 && minutesSpent === 0) {
      this._toastr.info('Studen und Minuten dürfen nicht beide 0 sein');
      this.catchForm.get('hoursSpent')?.setValue(null);
      this.catchForm.get('minutesSpent')?.setValue(null);
      return;
    }

    // Create FishCatchModel object for update
    const fishCatch: FishCatchModel = {
      id: this.currentCatchDay.id,
      fishingLicenseId: this.currentCatchDay.fishingLicenseId,
      catchDate: this.currentCatchDay.catchDate,
      hoursSpent: +(hoursSpent + '.' + minutesSpent)
    }

    // Call the updateCatch method in the FishCatchService
    this._fishCatchService.updateCatch(fishCatch.id, fishCatch).subscribe({
      next: ((response: FishCatchModel): void => {
        if (response) {
          // Show success toastr and close the dialog
          this._toastr.success('Angeltag wurde erfolgreich bearbeitet', 'Angeltag bearbeiten');
          this.onClose(true);
        }
      }),
      error: (): void => {
        // Show error toastr in case of failure
        this._toastr.error('Angeltag konnte nicht bearbeitet', 'Angeltag bearbeiten');
      }
    });
  }

  // Close the dialog with optional reload parameter
  onClose(reload: boolean): void {
    this._dialogRef.close(reload);
  }

}
