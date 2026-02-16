import {Component, Inject, inject} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ToastrService} from "ngx-toastr";
import {FishingRegulationService} from "../../../../services/fishing-regulation.service";
import {FishingRegulation} from "../../../../models/fishingRegulation.model";


@Component({
    selector: 'app-admin-edit-fishing-regulation',
    templateUrl: './admin-edit-fishing-regulation.component.html',
    standalone: false
})
export class AdminEditFishingRegulationComponent {
  public isUpdate: boolean;
  public fishingRegulationForm: FormGroup;

  private currentFishingRegulation: FishingRegulation;

  private readonly _dialogRef: MatDialogRef<AdminEditFishingRegulationComponent> = inject(MatDialogRef<AdminEditFishingRegulationComponent>);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _fishingRegulationService: FishingRegulationService = inject(FishingRegulationService);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { isUpdate: boolean, fishingRegulation: FishingRegulation }) {
    this.isUpdate = data.isUpdate;
    this.currentFishingRegulation = data.fishingRegulation;
    this.fishingRegulationForm = new FormGroup({
      id: new FormControl<string>(this.currentFishingRegulation.id),
      regulation: new FormControl<string>(this.currentFishingRegulation.regulation, [Validators.required])
    });
  }

  get regulation() {
    return this.fishingRegulationForm.get('regulation');
  }

  onSubmit() {
    if (this.fishingRegulationForm.invalid) {
      return;
    }
    const fishingRegulation: FishingRegulation = this.fishingRegulationForm.value as FishingRegulation;
    this.isUpdate ? this.updateFishingRegulation(fishingRegulation.id, fishingRegulation) : this.createFishingRegulation(fishingRegulation);
  }

  createFishingRegulation(fishingRegulation: FishingRegulation) {
    this._fishingRegulationService.createFishingRegulation(fishingRegulation).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Neue Vorschrift wurde erfolgreich erstellt', 'Neue Vorschrift');
          this.onClose(true);
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Neue Vorschrift konnte nicht erstellt werden', 'Neue Vorschrift');
      }
    });
  }

  updateFishingRegulation(fishingRegulationId: string, fishingRegulation: FishingRegulation) {
    this._fishingRegulationService.updateFishingRegulation(fishingRegulationId, fishingRegulation).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Vorschrift wurde erfolgreich geupdated', 'Update Vorschrift');
          this.onClose(true);
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Vorschrift konnte nicht geupdated werdeb', 'Update Vorschrift');
      }
    });
  }

  onClose(reload: boolean) {
    const response = {reload: reload};
    this._dialogRef.close(response);
  }
}
