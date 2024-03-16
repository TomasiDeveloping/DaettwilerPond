import {Component, inject, OnInit} from '@angular/core';
import {FishTypeService} from "../../../services/fish-type.service";
import {FishType} from "../../../models/fishType.model";
import {MatDialog} from "@angular/material/dialog";
import {AdminEditFishTypeComponent} from "./admin-edit-fish-type/admin-edit-fish-type.component";
import Swal from "sweetalert2";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-admin-fish-types',
  templateUrl: './admin-fish-types.component.html'
})
export class AdminFishTypesComponent implements OnInit {

  public fishTypes: FishType[] = [];
  public pageSettings: { pageSizes: boolean, pageSize: number } = {pageSizes: true, pageSize: 10};

  private readonly _fishTypeService: FishTypeService = inject(FishTypeService);
  private readonly _dialog: MatDialog = inject(MatDialog);
  private readonly _toastr: ToastrService = inject(ToastrService);

  ngOnInit() {
    this.getFishTypes();
  }

  getFishTypes() {
    this.fishTypes = [];
    this._fishTypeService.getFishTypes().subscribe({
      next: ((response) => {
        if (response) {
          this.fishTypes = response;
        }
      })
    });
  }

  onEdit(event: any) {
    const fishType: FishType = event.rowData as FishType;
    this.openEditFishTypeDialog(true, fishType);
  }

  onAddFishType() {
    const fishType: FishType = {
      id: '',
      name: '',
      hasMinimumSize: false,
      hasClosedSeason: false,
      minimumSize: undefined,
      closedSeasonToMonth: undefined,
      closedSeasonToDay: undefined,
      closedSeasonFromMonth: undefined,
      closedSeasonFromDay: undefined,
    };
    this.openEditFishTypeDialog(false, fishType);
  }

  openEditFishTypeDialog(isUpdate: boolean, fishType: FishType) {
    const dialogRef = this._dialog.open(AdminEditFishTypeComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      disableClose: true,
      data: {isUpdate: isUpdate, fishType: fishType}
    });
    dialogRef.afterClosed().subscribe((result: { reload: boolean }) => {
      if (result.reload) {
        this.getFishTypes();
      }
    });
  }

  onDeleteFishType(fishType: FishType) {
    Swal.fire({
      title: 'Bist Du sicher?',
      text: `Fischart ${fishType.name} wirklisch löschen?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Ja, bitte löschen!',
      cancelButtonText: 'Abbrechen'
    }).then((result) => {
      if (result.isConfirmed) {
        this._fishTypeService.deleteFishType(fishType.id).subscribe({
          next: ((response) => {
            if (response) {
              this._toastr.success(`Fischart ${fishType.name} wurde erfolgreich gelöscht`, 'Fischart Löschen');
              this.getFishTypes();
            }
          }),
          error: error => {
            this._toastr.error(error.error ?? 'Fischart konnte nicht gelöscht werden', 'Fischart Löschen');
          }
        });
      }
    });
  }
}
