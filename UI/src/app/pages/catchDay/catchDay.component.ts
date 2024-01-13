import {Component, inject, OnInit} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {CatchDayManualRecordingComponent} from "./catchDay-manual-recording/catchDay-manual-recording.component";
import {CatchDayAddCatchComponent} from "./catchDay-add-catch/catchDay-add-catch.component";
import {FishCatchService} from "../../services/fish-catch.service";
import {FishCatchModel} from "../../models/fishCatch.model";
import * as moment from "moment";
import {CatchDetailModel} from "../../models/catchDetail.model";
import {CatchDayEditCatchComponent} from "./catch-day-edit-catch/catch-day-edit-catch.component";
import Swal, {SweetAlertResult} from "sweetalert2";
import {CatchDetailService} from "../../services/catch-detail.service";
import {ToastrService} from "ngx-toastr";
import {FishingLicenseService} from "../../services/fishing-license.service";
import {AuthenticationService} from "../../services/authentication.service";

@Component({
  selector: 'app-catchDay',
  templateUrl: './catchDay.component.html',
  styleUrl: './catchDay.component.scss'
})
export class CatchDayComponent implements OnInit {

  public dayStart: boolean = false;
  public isDayStopped: boolean = false;
  public userLicence!: string;
  public currentCatch: FishCatchModel | undefined;
  public hasNoLicence: boolean = false;
  private readonly _dialog: MatDialog = inject(MatDialog);
  private readonly _catchService: FishCatchService = inject(FishCatchService);
  private readonly _catchDetailService: CatchDetailService = inject(CatchDetailService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _licenceService: FishingLicenseService = inject(FishingLicenseService);
  private readonly _authService: AuthenticationService = inject(AuthenticationService);

  ngOnInit(): void {
    const currentUserId: string | null = this._authService.getUserIdFromToken();
    if (!currentUserId) {
      return;
    }
    this._licenceService.getCurrentUserLicence(currentUserId).subscribe({
      next: ((response) => {
        if (response) {
          this.userLicence = response.id;
          this.getDailyCatch();
        } else {
          this.hasNoLicence = true;
          this._toastr.error('Keine Lizenz gefunden', 'Keine Lizenz');
          return;
        }
      }),
      error: () => {
        this.hasNoLicence = true;
        this._toastr.error('Keine Lizenz gefunden', 'Keine Lizenz');
        return;
      }
    });
  }

  getDailyCatch(): void {
    this._catchService.getCatchForCurrentDay(this.userLicence).subscribe({
      next: ((response: FishCatchModel): void => {
        if (response) {
          this.currentCatch = response;
          this.dayStart = true;
          if (response.endFishing) {
            this.isDayStopped = true;
          }
          if (response.startFishing && !this.isDayStopped) {
            const spendHoursSinceStart: number = +(moment(Date.now()).diff(response.startFishing, 'minutes') / 60).toFixed(2);
            this.currentCatch.hoursSpent = +(response.hoursSpent + spendHoursSinceStart).toFixed(2);
          }
        }
      })
    });
  }

  onStartDay(): void {
    this._catchService.startFishCatch(this.userLicence).subscribe({
      next: ((response: FishCatchModel): void => {
        if (response) {
          this.dayStart = true;
          this.currentCatch = response;
        }
      }),
      error: () => {
        this._toastr.error('Angeltag konnte nicht gestartet werden', 'Angeltag starten');
      }
    });
  }

  onStopDay(): void {
    if (!this.currentCatch) {
      return;
    }
    this._catchService.stopFishingCatch(this.currentCatch?.id).subscribe({
      next: ((response: FishCatchModel): void => {
        if (response) {
          this.currentCatch!.hoursSpent = response.hoursSpent;
          this.isDayStopped = true;
        }
      }),
      error: (): void => {
        this._toastr.error('Angeltag konnte nicht gestoppt werden', 'Angeltag stoppen');
      }
    });
  }

  onAddManual(): void {
    this._dialog.open(CatchDayManualRecordingComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      data: {licenceId: this.userLicence}
    });
  }

  onAddCatch(): void {
    if (!this.currentCatch) {
      return;
    }
    const dialogRef = this._dialog.open(CatchDayAddCatchComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      data: {catchId: this.currentCatch.id}
    });
    dialogRef.afterClosed().subscribe({
      next: ((reload: boolean): void => {
        if (reload) {
          this.getDailyCatch();
        }
      })
    });
  }

  onContinue(): void {
    if (!this.currentCatch) {
      return;
    }
    this._catchService.continueFishingCatch(this.currentCatch.id).subscribe({
      next: ((response: FishCatchModel): void => {
        if (response) {
          this.isDayStopped = false;
          this.dayStart = true;
        }
      }),
      error: () => {
        this._toastr.error('Angeltag konnte nicht wieder gestartet werden', 'Weiter Angeln');
      }
    });
  }

  onDeleteCatch(catchDetailId: string): void {
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
        this._catchDetailService.deleteCatchDetail(catchDetailId).subscribe({
          next: ((response: boolean): void => {
            if (response) {
              this._toastr.success(`Fang wurde erfolgreich gelöscht`, 'Fang Löschen');
              this.getDailyCatch();
            }
          }),
          error: error => {
            this._toastr.error(error.error ?? 'Fang konnte nicht gelöscht werden', 'Fang Löschen');
          }
        });
      }
    });
  }

  onEditCatch(catchDetail: CatchDetailModel): void {
    const dialogRef = this._dialog.open(CatchDayEditCatchComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      disableClose: true,
      data: {catchDetail: catchDetail}
    });
    dialogRef.afterClosed().subscribe({
      next: ((reload: boolean): void => {
        if (reload) {
          this.getDailyCatch();
        }
      })
    });
  }
}
