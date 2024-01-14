import {Component, inject, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FishCatchModel} from "../../../../models/fishCatch.model";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {FishCatchService} from "../../../../services/fish-catch.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-edit-catch-day-dialog',
  templateUrl: './edit-catch-day-dialog.component.html',
  styleUrl: './edit-catch-day-dialog.component.scss'
})
export class EditCatchDayDialogComponent {

  public catchForm: FormGroup;
  public currentCatchDay: FishCatchModel;
  public hours: number[] = [];
  public minutes: number[] = [0, 25, 50, 75];
  private readonly _dialogRef: MatDialogRef<EditCatchDayDialogComponent> = inject(MatDialogRef<EditCatchDayDialogComponent>);
  private readonly _fishCatchService: FishCatchService = inject(FishCatchService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  constructor(@Inject(MAT_DIALOG_DATA) public data: {fishCatch: FishCatchModel}) {
    for (let i: number = 0; i <= 23; i++) {
      this.hours.push(i);
    }
    this.currentCatchDay = data.fishCatch;
    const splitTime: string[] = data.fishCatch.hoursSpent.toString().split('.');
    const hours: number = +splitTime[0];
    let minutes: number = isNaN(+splitTime[1]) ? 0 : +splitTime[1];
    if (minutes === 5) {
      minutes = 50;
    }
    this.catchForm = new FormGroup({
      id: new FormControl<string>(data.fishCatch.id, [Validators.required]),
      hoursSpent: new FormControl<number>(hours),
      minutesSpent: new FormControl<number>(minutes)
    });
  }

  get f() {
    return this.catchForm;
  }


  onSubmit() {
    if (this.catchForm.invalid) {
      return;
    }
    const hoursSpent: number = +this.catchForm.get('hoursSpent')?.value;
    const minutesSpent: number = +this.catchForm.get('minutesSpent')?.value;
    if (hoursSpent === 0 && minutesSpent === 0) {
      this._toastr.info('Studen und Minuten dürfen nicht beide 0 sein');
      this.catchForm.get('hoursSpent')?.setValue(null);
      this.catchForm.get('minutesSpent')?.setValue(null);
      return;
    }
    const fishCatch: FishCatchModel = {
      id: this.currentCatchDay.id,
      fishingLicenseId: this.currentCatchDay.fishingLicenseId,
      catchDate: this.currentCatchDay.catchDate,
      hoursSpent: +(hoursSpent + '.' + minutesSpent)
    }
    this._fishCatchService.updateCatch(fishCatch.id, fishCatch).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Angeltag wurde erfolgreich bearbeitet', 'Angeltag bearbeiten');
          this.onClose(true);
        }
      }),
      error: () => {
        this._toastr.error('Angeltag konnte nicht bearbeitet', 'Angeltag bearbeiten');
      }
    });
  }

  onClose(reload: boolean) {
    this._dialogRef.close(reload);
  }

}
