import {Component, inject, OnInit} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {StatisticManualRecordingComponent} from "./statistic-manual-recording/statistic-manual-recording.component";
import {StatisticAddCatchComponent} from "./statistic-add-catch/statistic-add-catch.component";
import {FishCatchService} from "../../services/fish-catch.service";
import {FishCatchModel} from "../../models/fishCatch.model";
import * as moment from "moment";
import {YearlyCatchModel} from "../../models/yearlyCatch.model";

@Component({
  selector: 'app-statistic',
  templateUrl: './statistic.component.html',
  styleUrl: './statistic.component.scss'
})
export class StatisticComponent implements OnInit{

  private readonly _dialog: MatDialog = inject(MatDialog);
  private readonly _catchService: FishCatchService = inject(FishCatchService);

  public dayStart: boolean = false;
  public isDayStopped: boolean = false;
  public userLicence: string = '34CD82FB-93F0-4AB6-BBE7-08DBF8D03614';
  public currentCatch: FishCatchModel | undefined;
  public yearlyCatch: YearlyCatchModel = { fishCatches: 0, hoursSpent: 0};

ngOnInit() {
  this._catchService.getYearlyCatch(this.userLicence).subscribe({
    next: ((response) => {
      if (response) {
        this.yearlyCatch = response;
      }
    })
  });
    this._catchService.getCatchForCurrentDay(this.userLicence).subscribe({
      next: ((response) => {
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

  onStartDay() {
    this._catchService.startFishCatch(this.userLicence).subscribe({
      next: ((response) => {
        if (response) {
          this.dayStart = true;
          this.currentCatch = response;
        }
      })
    });
  }

  onStopDay() {
  if (!this.currentCatch) {
    return;
  }
  this._catchService.stopFishingCatch(this.currentCatch?.id).subscribe({
    next: ((response) => {
      if (response) {
        this.currentCatch = response;
        this.isDayStopped = true;
      }
    }),
    error: () => {

    }
  });
  }

  onAddManual() {
    const dialogRef = this._dialog.open(StatisticManualRecordingComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      data: {licenceId: this.userLicence}
    });
  }

  onAddCatch() {
  if (!this.currentCatch) {
    return;
  }
    const dialogRef = this._dialog.open(StatisticAddCatchComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      data: {catchId: this.currentCatch.id}
    });
  }

  onContinue() {
    if (!this.currentCatch) {
      return;
    }
    this._catchService.continueFishingCatch(this.currentCatch.id).subscribe({
      next: ((response) => {
        if (response) {
          this.currentCatch = response;
          this.isDayStopped = false;
          this.dayStart = true;
        }
      })
    });
  }
}
