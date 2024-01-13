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
  templateUrl: './catch-day-edit-catch.component.html',
  styleUrl: './catch-day-edit-catch.component.scss'
})
export class CatchDayEditCatchComponent {

  public currentCatchDetail: CatchDetailModel;
  public catchDetailForm: FormGroup;
  public fishTypes: FishType[] = [];

  private readonly _fishTypeService: FishTypeService = inject(FishTypeService);
  private readonly _dialogRef: MatDialogRef<CatchDayEditCatchComponent> = inject(MatDialogRef<CatchDayEditCatchComponent>);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _catchDetailService: CatchDetailService = inject(CatchDetailService);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { catchDetail: CatchDetailModel }) {
    this.currentCatchDetail = data.catchDetail;
    this.catchDetailForm = new FormGroup({
      fishTypeId: new FormControl<string>(data.catchDetail.fishTypeId, [Validators.required]),
      hadCrabs: new FormControl<boolean>(data.catchDetail.hadCrabs, [Validators.required])
    });
    this._fishTypeService.getFishTypes().subscribe({
      next: ((response) => {
        if (response) {
          this.fishTypes = response;
        }
      })
    });
  }

  onSubmit() {
    if (this.catchDetailForm.invalid) {
      return;
    }
    this.currentCatchDetail.fishTypeId = this.catchDetailForm.get('fishTypeId')?.value;
    this.currentCatchDetail.hadCrabs = this.catchDetailForm.get('hadCrabs')?.value;
    this._catchDetailService.updateCatchDetail(this.currentCatchDetail.id, this.currentCatchDetail).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Fang wurde erfolgreich bearbeitet', 'Fang bearbeiten');
          this.onClose(true);
        }
      }),
      error: () => {
        this._toastr.error('Fang konnte nicht bearbeitet werden', 'Fang bearbeiten');
      }
    });
  }


  onClose(reload: boolean | null) {
    this._dialogRef.close(reload);
  }
}
