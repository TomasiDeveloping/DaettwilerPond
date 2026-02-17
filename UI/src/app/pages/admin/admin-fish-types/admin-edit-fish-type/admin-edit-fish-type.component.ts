import {Component, inject, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FishType} from "../../../../models/fishType.model";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {FishTypeService} from "../../../../services/fish-type.service";
import {ToastrService} from "ngx-toastr";


@Component({
    selector: 'app-admin-edit-fish-type',
    templateUrl: './admin-edit-fish-type.component.html',
    standalone: false
})
export class AdminEditFishTypeComponent {
  public isUpdate: boolean;
  public currentFishType: FishType;
  public fihTypeForm: FormGroup;
  public days: number[] = [];
  public months: { name: string, value: number }[] = [];

  private readonly _fishTypeService: FishTypeService = inject(FishTypeService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _dialogRef: MatDialogRef<AdminEditFishTypeComponent> = inject(MatDialogRef<AdminEditFishTypeComponent>);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { isUpdate: boolean, fishType: FishType }) {
    this.isUpdate = data.isUpdate;
    this.currentFishType = data.fishType;
    this.fihTypeForm = new FormGroup({
      id: new FormControl<string>(this.currentFishType.id),
      name: new FormControl<string>(this.currentFishType.name, [Validators.required]),
      closedSeasonFromDay: new FormControl<number | undefined>(this.currentFishType.closedSeasonFromDay),
      closedSeasonFromMonth: new FormControl<number | undefined>(this.currentFishType.closedSeasonFromMonth),
      closedSeasonToDay: new FormControl<number | undefined>(this.currentFishType.closedSeasonToDay),
      closedSeasonToMonth: new FormControl<number | undefined>(this.currentFishType.closedSeasonToMonth),
      minimumSize: new FormControl<number | undefined>(this.currentFishType.minimumSize),
      hasClosedSeason: new FormControl<boolean>(this.currentFishType.hasClosedSeason),
      hasMinimumSize: new FormControl<boolean>(this.currentFishType.hasMinimumSize)
    });
    this.createDays();
    this.createMonths();
  }

  get name() {
    return this.fihTypeForm.get('name');
  }

  get hasClosedSeason() {
    return this.fihTypeForm.get('hasClosedSeason')?.value;
  }

  get hasMinimumSize() {
    return this.fihTypeForm.get('hasMinimumSize')?.value;
  }

  createMonths() {
    this.months.push({name: 'Januar', value: 1});
    this.months.push({name: 'Februar', value: 2});
    this.months.push({name: 'März', value: 3});
    this.months.push({name: 'April', value: 4});
    this.months.push({name: 'Mai', value: 5});
    this.months.push({name: 'Juni', value: 6});
    this.months.push({name: 'Juli', value: 7});
    this.months.push({name: 'August', value: 8});
    this.months.push({name: 'September', value: 9});
    this.months.push({name: 'Oktober', value: 10});
    this.months.push({name: 'November', value: 11});
    this.months.push({name: 'Dezember', value: 12});
  }

  createDays() {
    for (let i = 1; i <= 31; i++) {
      this.days.push(i);
    }
  }

  onSubmit() {
    if (this.fihTypeForm.invalid) {
      return;
    }
    const fishType: FishType = this.fihTypeForm.value as FishType;
    if (!this.hasClosedSeason) {
      fishType.closedSeasonFromDay = undefined;
      fishType.closedSeasonFromMonth = undefined;
      fishType.closedSeasonToDay = undefined;
      fishType.closedSeasonToMonth = undefined;
    }
    if (!this.hasMinimumSize) {
      fishType.minimumSize = undefined;
    }
    this.isUpdate ? this.updateFihType(fishType) : this.createFishType(fishType);
  }

  createFishType(fishType: FishType) {
    this._fishTypeService.createFishType(fishType).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success(`Fischart ${response.name} wurde erfolgreich erstellt`, 'Fischart hinzufügen');
          this.onClose(true);
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Fischart konnte nicht erstellt werden', 'Fischart hinzufügen');
      }
    });
  }

  updateFihType(fishType: FishType) {
    this._fishTypeService.updateFishType(fishType.id, fishType).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success(`Fischart ${response.name} wurde erfolgreich geupdated`, 'Fischart updaten');
          this.onClose(true);
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Fischart konnte nicht geupdated werden', 'Fischart Updaten');
      }
    });
  }

  onClose(reload: boolean) {
    const response = {reload: reload};
    this._dialogRef.close(response);
  }
}
