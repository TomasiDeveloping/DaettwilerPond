import {Component, inject, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {FishCatchService} from "../../../services/fish-catch.service";
import {FishCatchModel} from "../../../models/fishCatch.model";

@Component({
  selector: 'app-statistic-manual-recording',
  templateUrl: './statistic-manual-recording.component.html',
  styleUrl: './statistic-manual-recording.component.scss'
})
export class StatisticManualRecordingComponent {

  public manualCatchForm: FormGroup;
  public hours: number[] = [];
  public minutes: number[] = [0, 25, 50, 75];
  public maxDateAllowed = new Date().toISOString().substring(0,10);
  public minDateAllowed = new Date(new Date().getFullYear(), 0, 1).toISOString().substring(0,10);
  private readonly _fishCatchService = inject(FishCatchService);
  private readonly _dialogRef: MatDialogRef<StatisticManualRecordingComponent> = inject(MatDialogRef<StatisticManualRecordingComponent>);
  constructor(@Inject(MAT_DIALOG_DATA) public data: {licenceId: string}) {
    for (let i = 0; i <= 23; i++) {
      this.hours.push(i);
    }
    this.manualCatchForm = new FormGroup({
      licenceId: new FormControl<string>(data.licenceId, [Validators.required]),
      catchDate: new FormControl<Date>(new Date(), [Validators.required]),
      hoursSpent: new FormControl<number>(0, [Validators.required]),
      minutesSpent: new FormControl<number>(0, [Validators.required])
    });
  }

  onSubmit() {
    const time = +(this.manualCatchForm.get('hoursSpent')?.value + '.' +  this.manualCatchForm.get('minutesSpent')?.value)
    console.log(time);
    console.log(this.manualCatchForm.value);
    const catchDate =  new Date(this.manualCatchForm.get('catchDate')?.value);
    catchDate.setHours(12);
    catchDate.setMinutes(0);

    const fishCatch: FishCatchModel = {
      id: '',
      hoursSpent: time,
      catchDate: catchDate,
      fishingLicenseId: this.manualCatchForm.get('licenceId')?.value
    };
    console.log(fishCatch);
    this._fishCatchService.createFishCatch(fishCatch).subscribe();
  }

  onClose() {
    this._dialogRef.close();
  }
}
