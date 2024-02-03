import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
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

  // Properties for the current month and fishing license
  public currentMonth: number | null = null;
  public currentLicence: string | null = null;

  // Array to hold fish catches for the specified month
  public monthCatches: FishCatchModel[] = [];

  // Angular services and components injection
  private readonly _activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  private readonly _fishCatchService: FishCatchService = inject(FishCatchService);
  private readonly _dialog: MatDialog = inject(MatDialog);
  private readonly _catchDetailService: CatchDetailService = inject(CatchDetailService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _router: Router = inject(Router);

  ngOnInit(): void {
    // Initialize component properties based on route parameters
    this.currentLicence = this._activatedRoute.snapshot.paramMap.get('licenceId');
    const monthString: string | null = this._activatedRoute.snapshot.paramMap.get('month') ?? null;

    // Convert month string to number
    if (monthString) {
      this.currentMonth = +monthString;
    }

    // Check if required parameters are present, else return
    if (!this.currentLicence || this.currentMonth == null) {
      return;
    }

    // Fetch month catches based on parameters
    this.getMonthCatches(this.currentLicence, this.currentMonth);
  }

  // Fetches fish catches for the specified license and month
  getMonthCatches(licenceId: string, month: number): void {
    this._fishCatchService.getCatchesForMonth(licenceId, month + 1).subscribe({
      next: ((response: FishCatchModel[]): void => {
        if (response) {
          this.monthCatches = response;
        }
      })
    });
  }

  // Opens the dialog for editing a fish catch day
  onEditDay(monthCatch: FishCatchModel): void {
    const dialogRef = this._dialog.open(EditCatchDayDialogComponent, {
      width: '80%',
      height: 'auto',
      data: {fishCatch: monthCatch}
    });

    // Subscribe to dialog close event
    dialogRef.afterClosed().subscribe({
      next: ((reload): void => {
        // Reload month catches if needed
        if (reload) {
          this.getMonthCatches(this.currentLicence!, this.currentMonth!);
        }
      })
    });
  }

  // Deletes a fish catch day after confirmation
  onDeleteDay(monthCatch: FishCatchModel): void {
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
        // Call the service to delete the fish catch
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

  // Opens the dialog for editing a catch detail
  onEditDetail(catchDetail: CatchDetailModel): void {
    const dialogRef = this._dialog.open(CatchDayEditCatchComponent, {
      width: '80%',
      height: 'auto',
      data: {catchDetail: catchDetail}
    });

    // Subscribe to dialog close event
    dialogRef.afterClosed().subscribe({
      next: ((reload: boolean): void => {
        // Reload month catches if needed
        if (reload) {
          this.getMonthCatches(this.currentLicence!, this.currentMonth!);
        }
      })
    });
  }

  // Deletes a catch detail after confirmation
  onDeleteDetail(catchDetail: CatchDetailModel): void {
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
        // Call the service to delete the catch detail
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

  // Opens the dialog for adding a new catch detail
  onAddCatchDetail(catchId: string): void {
    const dialogRef = this._dialog.open(CatchDayAddCatchComponent, {
      width: '80%',
      height: 'auto',
      data: {catchId: catchId}
    });

    // Subscribe to dialog close event
    dialogRef.afterClosed().subscribe({
      next: ((reload: boolean): void => {
        // Reload month catches if needed
        if (reload) {
          this.getMonthCatches(this.currentLicence!, this.currentMonth!);
        }
      })
    });
  }

  // Navigates back to the catch statistics page
  onBack(): void {
    this._router.navigate(['fangstatistik']).then();
  }
}
