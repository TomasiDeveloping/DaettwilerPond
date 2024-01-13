import {Component, inject, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FormArray, FormControl, FormGroup, Validators} from "@angular/forms";
import {FishCatchService} from "../../../services/fish-catch.service";
import {FishTypeService} from "../../../services/fish-type.service";
import {FishType} from "../../../models/fishType.model";
import {ToastrService} from "ngx-toastr";
import {ManualCatchModel} from "../../../models/manualCatch.model";
import {CatchDetailService} from "../../../services/catch-detail.service";
import {CatchDetailModel} from "../../../models/catchDetail.model";
import {forkJoin, Observable} from "rxjs";
import {catchDayExistsValidator} from "../../../validators/catchDayExistsValidator";

@Component({
  selector: 'app-catchDay-manual-recording',
  templateUrl: './catchDay-manual-recording.component.html',
  styleUrl: './catchDay-manual-recording.component.scss'
})
export class CatchDayManualRecordingComponent {

  public fishTypes: FishType[] = [];
  public manualCatchForm: FormGroup;
  public hours: number[] = [];
  public minutes: number[] = [0, 25, 50, 75];
  public maxDateAllowed: string = new Date(new Date().setDate(new Date().getDate() - 1)).toISOString().substring(0, 10);
  public minDateAllowed: string = new Date(new Date().getFullYear(), 0, 1).toISOString().substring(0, 10);
  private readonly _fishCatchService: FishCatchService = inject(FishCatchService);
  private readonly _catchDetailService: CatchDetailService = inject(CatchDetailService);
  private readonly _dialogRef: MatDialogRef<CatchDayManualRecordingComponent> = inject(MatDialogRef<CatchDayManualRecordingComponent>);
  private readonly _fishTypeService: FishTypeService = inject(FishTypeService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { licenceId: string }) {
    for (let i: number = 0; i <= 23; i++) {
      this.hours.push(i);
    }
    this.manualCatchForm = new FormGroup({
      licenceId: new FormControl<string>(data.licenceId, [Validators.required]),
      catchDate: new FormControl<Date | null>(null, [Validators.required], catchDayExistsValidator(data.licenceId, this._fishCatchService)),
      hoursSpent: new FormControl<number | null>(null, [Validators.required]),
      minutesSpent: new FormControl<number | null>(null, [Validators.required]),
      catchDetails: new FormArray([])
    });
    this.getFishTypes();
  }

  get catchDetails() {
    return this.manualCatchForm.get('catchDetails') as FormArray;
  }

  get f() {
    return this.manualCatchForm;
  }

  onSubmit(): void {
    if (this.manualCatchForm.invalid) {
      return;
    }
    const hoursSpent: number = +this.manualCatchForm.get('hoursSpent')?.value;
    const minutesSpent: number = +this.manualCatchForm.get('minutesSpent')?.value;
    if (hoursSpent === 0 && minutesSpent === 0) {
      this._toastr.info('Studen und Minuten dürfen nicht beide 0 sein');
      this.manualCatchForm.get('hoursSpent')?.setValue(null);
      this.manualCatchForm.get('minutesSpent')?.setValue(null);
      return;
    }
    const time: number = +(hoursSpent + '.' + minutesSpent);
    const catchDate: Date = new Date(this.manualCatchForm.get('catchDate')?.value);
    catchDate.setHours(12);
    catchDate.setMinutes(0);

    const fishCatch: ManualCatchModel = {
      hoursSpent: time,
      catchDate: catchDate,
      fishingLicenseId: this.manualCatchForm.get('licenceId')?.value,
      catchDetails: this.catchDetails.value
    };
    this._fishCatchService.createFishCatch(fishCatch).subscribe({
      next: ((response) => {
        if (response) {
          if (fishCatch.catchDetails.length > 0) {
            this.insertCatchDetail(response.id, fishCatch.catchDetails);
          } else {
            this._toastr.success('Angeltag wurde erfolgreich erfasst', 'Manuell erfassen');
            this.onClose();
          }
        }
      }),
      error: () => {
        this._toastr.error('Angeltag konnte nicht erfasst werden', 'Manuell erfassen');
      }
    });
  }

  onClose(): void {
    this._dialogRef.close();
  }

  onAddCatch() {
    this.catchDetails.push(this.createCatchForm());
  }

  createCatchForm(): FormGroup {
    return new FormGroup({
      id: new FormControl<string>(''),
      catchId: new FormControl<string>(''),
      fishTypeId: new FormControl<string>('', [Validators.required]),
      hadCrabs: new FormControl<boolean>(false)
    })
  }

  onRemoveCatch(index: number) {
    this.catchDetails.removeAt(index);
  }

  private getFishTypes() {
    this._fishTypeService.getFishTypes().subscribe({
      next: ((response: FishType[]): void => {
        if (response) {
          this.fishTypes = response;
        }
      })
    });
  }

  private insertCatchDetail(catchId: string, catchDetails: CatchDetailModel[]) {
    const apiCalls: Observable<CatchDetailModel>[] = [];
    catchDetails.forEach((detail) => {
      detail.catchId = catchId;
      apiCalls.push(this._catchDetailService.createCatchDetail(detail));
    });
    forkJoin(apiCalls).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Angeltag wurde erfolgreich erfasst', 'Manuell erfassen');
          this.onClose();
        }
      })
    });
  }
}
