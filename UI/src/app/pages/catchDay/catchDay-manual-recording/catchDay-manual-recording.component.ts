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
import {FishCatchModel} from "../../../models/fishCatch.model";

@Component({
  selector: 'app-catchDay-manual-recording',
  templateUrl: './catchDay-manual-recording.component.html'
})
export class CatchDayManualRecordingComponent {

  // Declare public properties
  public fishTypes: FishType[] = [];
  public manualCatchForm: FormGroup;
  public hours: number[] = [];
  public minutes: number[] = [0, 25, 50, 75];
  public maxDateAllowed: string = new Date(new Date().setDate(new Date().getDate() - 1)).toISOString().substring(0, 10);
  public minDateAllowed: string = new Date(new Date().getFullYear(), 0, 1).toISOString().substring(0, 10);

  // Inject necessary services and dependencies
  private readonly _fishCatchService: FishCatchService = inject(FishCatchService);
  private readonly _catchDetailService: CatchDetailService = inject(CatchDetailService);
  private readonly _dialogRef: MatDialogRef<CatchDayManualRecordingComponent> = inject(MatDialogRef<CatchDayManualRecordingComponent>);
  private readonly _fishTypeService: FishTypeService = inject(FishTypeService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { licenceId: string }) {
    // Populate hours array
    for (let i: number = 0; i <= 23; i++) {
      this.hours.push(i);
    }

    // Initialize the manualCatchForm FormGroup
    this.manualCatchForm = new FormGroup({
      licenceId: new FormControl<string>(data.licenceId, [Validators.required]),
      catchDate: new FormControl<Date | null>(null, [Validators.required], catchDayExistsValidator(data.licenceId, this._fishCatchService)),
      hoursSpent: new FormControl<number | null>(null, [Validators.required]),
      minutesSpent: new FormControl<number | null>(null, [Validators.required]),
      catchDetails: new FormArray([])
    });

    // Fetch available fish types
    this.getFishTypes();
  }

  // Getter for catchDetails FormArray
  get catchDetails() {
    return this.manualCatchForm.get('catchDetails') as FormArray;
  }

  // Getter for manualCatchForm
  get f() {
    return this.manualCatchForm;
  }

  // Handle form submission
  onSubmit(): void {
    if (this.manualCatchForm.invalid) {
      return;
    }

    // Extract values from form
    const hoursSpent: number = +this.manualCatchForm.get('hoursSpent')?.value;
    const minutesSpent: number = +this.manualCatchForm.get('minutesSpent')?.value;

    // Check if both hours and minutes are not zero
    if (hoursSpent === 0 && minutesSpent === 0) {
      this._toastr.info('Studen und Minuten dürfen nicht beide 0 sein');
      this.manualCatchForm.get('hoursSpent')?.setValue(null);
      this.manualCatchForm.get('minutesSpent')?.setValue(null);
      return;
    }

    // Calculate total time in hours
    const time: number = +(hoursSpent + '.' + minutesSpent);

    // Create Date object for catchDate
    const catchDate: Date = new Date(this.manualCatchForm.get('catchDate')?.value);
    catchDate.setHours(12);
    catchDate.setMinutes(0);

    // Construct ManualCatchModel
    const fishCatch: ManualCatchModel = {
      hoursSpent: time,
      catchDate: catchDate,
      fishingLicenseId: this.manualCatchForm.get('licenceId')?.value,
      catchDetails: this.catchDetails.value
    };

    // Call service to create fish catch
    this._fishCatchService.createFishCatch(fishCatch).subscribe({
      next: ((response: FishCatchModel): void => {
        if (response) {
          // If there are catch details, insert them individually
          if (fishCatch.catchDetails.length > 0) {
            this.insertCatchDetail(response.id, fishCatch.catchDetails);
          } else {
            // Display success toastr message and close the dialog
            this._toastr.success('Angeltag wurde erfolgreich erfasst', 'Manuell erfassen');
            this.onClose();
          }
        }
      }),
      error: (): void => {
        // Display error toastr message if the fish catch couldn't be recorded
        this._toastr.error('Angeltag konnte nicht erfasst werden', 'Manuell erfassen');
      }
    });
  }

  // Close the dialog
  onClose(): void {
    this._dialogRef.close();
  }

  // Add a new catch detail to the form
  onAddCatch(): void {
    this.catchDetails.push(this.createCatchForm());
  }

  // Create a new FormGroup for catch detail
  createCatchForm(): FormGroup {
    return new FormGroup({
      id: new FormControl<string>(''),
      catchId: new FormControl<string>(''),
      fishTypeId: new FormControl<string>('', [Validators.required]),
      hadCrabs: new FormControl<boolean>(false),
      amount: new FormControl<number>(1)
    })
  }

  // Remove a catch detail from the form
  onRemoveCatch(index: number): void {
    this.catchDetails.removeAt(index);
  }

  // Fetch available fish types
  private getFishTypes(): void {
    this._fishTypeService.getFishTypes().subscribe({
      next: ((response: FishType[]): void => {
        if (response) {
          this.fishTypes = response;
        }
      })
    });
  }

  // Insert catch details individually
  private insertCatchDetail(catchId: string, catchDetails: CatchDetailModel[]): void {
    const apiCalls: Observable<CatchDetailModel>[] = [];

    // Update catchId for each catch detail and create individual API calls
    catchDetails.forEach((detail: CatchDetailModel): void => {
      detail.catchId = catchId;
      apiCalls.push(this._catchDetailService.createCatchDetail(detail));
    });

    // Execute all API calls in parallel
    forkJoin(apiCalls).subscribe({
      next: ((response: CatchDetailModel[]): void => {
        if (response) {
          // Display success toastr message and close the dialog
          this._toastr.success('Angeltag wurde erfolgreich erfasst', 'Manuell erfassen');
          this.onClose();
        }
      })
    });
  }
}
