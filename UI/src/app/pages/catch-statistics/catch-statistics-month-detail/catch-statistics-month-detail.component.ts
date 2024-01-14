import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {FishCatchService} from "../../../services/fish-catch.service";
import {FishCatchModel} from "../../../models/fishCatch.model";
import {CatchDetailModel} from "../../../models/catchDetail.model";
import {MatDialog} from "@angular/material/dialog";
import {CatchDayEditCatchComponent} from "../../catchDay/catch-day-edit-catch/catch-day-edit-catch.component";
import {EditCatchDayDialogComponent} from "./edit-catch-day-dialog/edit-catch-day-dialog.component";
import {CatchDayAddCatchComponent} from "../../catchDay/catchDay-add-catch/catchDay-add-catch.component";
import Swal, {SweetAlertResult} from "sweetalert2";
import {CatchDetailService} from "../../../services/catch-detail.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-catch-statistics-month-detail',
  templateUrl: './catch-statistics-month-detail.component.html',
  styleUrl: './catch-statistics-month-detail.component.scss'
})
export class CatchStatisticsMonthDetailComponent implements OnInit{

  public currentMonth: number | null = null;
  public currentLicence: string | null = null;
  public monthCatches: FishCatchModel[] = [];
  private readonly _activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  private readonly _fishCatchService: FishCatchService = inject(FishCatchService);
  private readonly _dialog: MatDialog = inject(MatDialog);
  private readonly _catchDetailService: CatchDetailService = inject(CatchDetailService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  ngOnInit() {
    this.currentLicence = this._activatedRoute.snapshot.paramMap.get('licenceId');
    const monthString: string | null = this._activatedRoute.snapshot.paramMap.get('month') ?? null;
    if (monthString) {
      this.currentMonth = +monthString;
    }

    if (!this.currentLicence || this.currentMonth == null) {
      return;
    }
    this.getMonthCatches(this.currentLicence, this.currentMonth);
  }

  getMonthCatches(licenceId: string, month: number) {
    this._fishCatchService.getCatchesForMonth(licenceId, month + 1).subscribe({
      next: ((response) => {
        if (response) {
          this.monthCatches = response;
        }
      })
    });
  }

  onEditDay(monthCatch: FishCatchModel) {
    const dialogRef = this._dialog.open(EditCatchDayDialogComponent, {
      width: '80%',
      height: 'auto',
      data: {fishCatch: monthCatch}
    });
    dialogRef.afterClosed().subscribe({
      next: ((reload) => {
        if (reload) {
          this.getMonthCatches(this.currentLicence!, this.currentMonth!);
        }
      })
    });
  }

  onDeleteDay(monthCatch: FishCatchModel) {
    Swal.fire({
      title: 'Bist Du sicher?',
      text: 'Angeltag mit wirklisch löschen?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Ja, bitte löschen!',
      cancelButtonText: 'Abbrechen'
    }).then((result: SweetAlertResult): void => {
      if (result.isConfirmed) {
        this._fishCatchService.deleteFishCatch(monthCatch.id).subscribe({
          next: ((response: boolean): void => {
            if (response) {
              this._toastr.success(`Angeltag wurde erfolgreich gelöscht`, 'Angeltag Löschen');
              this.getMonthCatches(this.currentLicence!, this.currentMonth!);
            }
          }),
          error: error => {
            this._toastr.error(error.error ?? 'Angeltag konnte nicht gelöscht werden', 'Angeltag Löschen');
          }
        });
      }
    });
  }

  onEditDetail(catchDetail: CatchDetailModel) {
    const dialogRef = this._dialog.open(CatchDayEditCatchComponent, {
      width: '80%',
      height: 'auto',
      data: {catchDetail: catchDetail}
    });
    dialogRef.afterClosed().subscribe({
      next: ((reload: boolean) => {
        if (reload) {
          this.getMonthCatches(this.currentLicence!, this.currentMonth!);
        }
      })
    });
  }

  onDeleteDetail(catchDetail: CatchDetailModel) {
    Swal.fire({
      title: 'Bist Du sicher?',
      text: 'Fang wirklisch löschen?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Ja, bitte löschen!',
      cancelButtonText: 'Abbrechen'
    }).then((result: SweetAlertResult): void => {
      if (result.isConfirmed) {
        this._catchDetailService.deleteCatchDetail(catchDetail.id).subscribe({
          next: ((response: boolean): void => {
            if (response) {
              this._toastr.success(`Fang wurde erfolgreich gelöscht`, 'Fang Löschen');
              this.getMonthCatches(this.currentLicence!, this.currentMonth!);
            }
          }),
          error: error => {
            this._toastr.error(error.error ?? 'Fang konnte nicht gelöscht werden', 'Fang Löschen');
          }
        });
      }
    });
  }

  onAddCatchDetail(catchId: string) {
    const dialogRef = this._dialog.open(CatchDayAddCatchComponent, {
      width: '80%',
      height: 'auto',
      data: {catchId: catchId}
    });
    dialogRef.afterClosed().subscribe({
      next: ((reload: boolean) => {
        if (reload) {
          this.getMonthCatches(this.currentLicence!, this.currentMonth!);
        }
      })
    });
  }
}
