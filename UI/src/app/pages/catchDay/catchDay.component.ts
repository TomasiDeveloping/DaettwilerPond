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
import {FishingLicense} from "../../models/fishingLicense.model";

@Component({
    selector: 'app-catchDay',
    templateUrl: './catchDay.component.html',
    styleUrl: './catchDay.component.scss',
    standalone: false
})
export class CatchDayComponent implements OnInit {

  // Properties
  public dayStart: boolean = false;
  public isDayStopped: boolean = false;
  public userLicence!: string;
  public currentCatch: FishCatchModel | undefined;
  public hasNoLicence: boolean = false;

  // Services and Dialog
  private readonly _dialog: MatDialog = inject(MatDialog);
  private readonly _catchService: FishCatchService = inject(FishCatchService);
  private readonly _catchDetailService: CatchDetailService = inject(CatchDetailService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _licenceService: FishingLicenseService = inject(FishingLicenseService);
  private readonly _authService: AuthenticationService = inject(AuthenticationService);

  ngOnInit(): void {
    // Get the current user ID from the authentication service
    const currentUserId: string | null = this._authService.getUserIdFromToken();

    if (!currentUserId) {
      // Handle the case where there is no user ID
      return;
    }

    // Fetch the user's fishing license based on the user ID
    this._licenceService.getCurrentUserLicence(currentUserId).subscribe({
      next: ((response: FishingLicense): void => {
        if (response) {
          // If a fishing license is found, update properties and get daily catch details
          this.userLicence = response.id;
          this.getDailyCatch();
        } else {
          // If no fishing license is found, set the 'hasNoLicence' flag and show an error toast
          this.hasNoLicence = true;
          this._toastr.error('Keine Lizenz gefunden', 'Keine Lizenz');
        }
      }),
      error: (): void => {
        // Handle errors during fetching the fishing license
        this.hasNoLicence = true;
        this._toastr.error('Keine Lizenz gefunden', 'Keine Lizenz');
      }
    });
  }

  // Fetch daily catch details for the current user's fishing license
  getDailyCatch(): void {
    this._catchService.getCatchForCurrentDay(this.userLicence).subscribe({
      next: ((response: FishCatchModel): void => {
        if (response) {
          // If daily catch details are found, update the currentCatch property
          this.currentCatch = response;
          this.dayStart = true;

          // Check if the fishing day is stopped
          if (response.endFishing) {
            this.isDayStopped = true;
          }

          // Calculate hours spent if the fishing day is ongoing
          if (response.startFishing && !this.isDayStopped) {
            const spendHoursSinceStart: number = +(moment(Date.now()).diff(response.startFishing, 'minutes') / 60).toFixed(2);
            this.currentCatch.hoursSpent = +(response.hoursSpent + spendHoursSinceStart).toFixed(2);
          }
        }
      })
    });
  }

  // Start the fishing day by making a request to start the fish catch
  onStartDay(): void {
    this._catchService.startFishCatch(this.userLicence).subscribe({
      next: ((response: FishCatchModel): void => {
        if (response) {
          // If the fishing day is successfully started, update properties
          this.dayStart = true;
          this.currentCatch = response;
        }
      }),
      error: (): void => {
        // Handle errors when starting the fishing day
        this._toastr.error('Angeltag konnte nicht gestartet werden', 'Angeltag starten');
      }
    });
  }

  // Stop the fishing day by making a request to stop the fish catch
  onStopDay(): void {
    if (!this.currentCatch) {
      // Check if there is a current catch available
      return;
    }

    this._catchService.stopFishingCatch(this.currentCatch?.id).subscribe({
      next: ((response: FishCatchModel): void => {
        if (response) {
          // If the fishing day is successfully stopped, update hoursSpent and isDayStopped properties
          this.currentCatch!.hoursSpent = response.hoursSpent;
          this.isDayStopped = true;
        }
      }),
      error: (): void => {
        // Handle errors when stopping the fishing day
        this._toastr.error('Angeltag konnte nicht gestoppt werden', 'Angeltag stoppen');
      }
    });
  }

  // Open the manual recording dialog to add manual catch details
  onAddManual(): void {
    this._dialog.open(CatchDayManualRecordingComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      data: {licenceId: this.userLicence}
    });
  }

  // Open the add catch dialog to add new catch details
  onAddCatch(): void {
    // Check if there is a current catch available
    if (!this.currentCatch) {
      return;
    }

    // Open the add catch dialog with necessary data
    const dialogRef = this._dialog.open(CatchDayAddCatchComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      data: {catchId: this.currentCatch.id}
    });

    // Subscribe to the dialog's closed event to handle the result
    dialogRef.afterClosed().subscribe({
      next: ((reload: boolean): void => {
        if (reload) {
          // If the dialog indicates a reload, refresh daily catch details
          this.getDailyCatch();
        }
      })
    });
  }

  // Continue the fishing day by making a request to continue the fish catch
  onContinue(): void {
    if (!this.currentCatch) {
      // Check if there is a current catch available
      return;
    }

    this._catchService.continueFishingCatch(this.currentCatch.id).subscribe({
      next: ((response: FishCatchModel): void => {
        if (response) {
          // If the fishing day is successfully continued, update properties
          this.isDayStopped = false;
          this.dayStart = true;
        }
      }),
      error: (): void => {
        // Handle errors when continuing the fishing day
        this._toastr.error('Angeltag konnte nicht wieder gestartet werden', 'Weiter Angeln');
      }
    });
  }

  // Delete a catch detail based on its ID
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
        // If the user confirms the deletion, make a request to delete the catch detail
        this._catchDetailService.deleteCatchDetail(catchDetailId).subscribe({
          next: ((response: boolean): void => {
            if (response) {
              // If the catch detail is successfully deleted, show success toast and refresh daily catch
              this._toastr.success(`Fang wurde erfolgreich gelöscht`, 'Fang Löschen');
              this.getDailyCatch();
            }
          }),
          error: error => {
            // Handle errors when deleting the catch detail
            this._toastr.error(error.error ?? 'Fang konnte nicht gelöscht werden', 'Fang Löschen');
          }
        });
      }
    });
  }

  // Edit a catch detail based on the provided catch detail model
  onEditCatch(catchDetail: CatchDetailModel): void {
    const dialogRef = this._dialog.open(CatchDayEditCatchComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      disableClose: true,
      data: {catchDetail: catchDetail}
    });

    // Subscribe to the dialog's closed event to handle the result
    dialogRef.afterClosed().subscribe({
      next: ((reload: boolean): void => {
        if (reload) {
          // If the dialog indicates a reload, refresh daily catch details
          this.getDailyCatch();
        }
      })
    });
  }
}
