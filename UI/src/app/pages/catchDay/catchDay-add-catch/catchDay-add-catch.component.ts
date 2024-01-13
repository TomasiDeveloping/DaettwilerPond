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
  templateUrl: './catchDay-add-catch.component.html',
  styleUrl: './catchDay-add-catch.component.scss'
})
export class CatchDayAddCatchComponent {

  catchForm!: FormGroup;
  public fishTypes: FishType[] = [];
  private readonly _dialogRef: MatDialogRef<CatchDayAddCatchComponent> = inject(MatDialogRef<CatchDayAddCatchComponent>);
  private readonly _fishTypeService: FishTypeService = inject(FishTypeService);
  private readonly _catchDetailService: CatchDetailService = inject(CatchDetailService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { catchId: string }) {
    this.catchForm = new FormGroup({
      catchId: new FormControl<string>(data.catchId, [Validators.required]),
      fishTypeId: new FormControl<string>('', [Validators.required]),
      amount: new FormControl<number>(1, [Validators.required]),
      hadCrabs: new FormControl<boolean>(false, [Validators.required])
    });
    this._fishTypeService.getFishTypes().subscribe({
      next: ((response: FishType[]) => {
        if (response) {
          this.fishTypes = response;
        }
      })
    });
  }

  onSubmit() {
    if (this.catchForm.invalid) {
      return;
    }
    const catchDetail: CatchDetailModel = this.catchForm.value as CatchDetailModel;
    this._catchDetailService.createCatchDetail(catchDetail).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Fang wurde erfolgreich erfasst', 'Fang hinzufügen');
          this.onClose(true);
        }
      }),
      error: () => {
        this._toastr.error('Fang konnte nicht erfasst werden', 'Fang hinzufügen');
      }
    });
  }

  onClose(reload: boolean) {
    this._dialogRef.close(reload);
  }
}
