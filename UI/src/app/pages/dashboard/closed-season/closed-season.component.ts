import {Component, inject, OnInit} from '@angular/core';
import {FishType} from "../../../models/fishType.model";
import {ToastrService} from "ngx-toastr";
import {FishTypeService} from "../../../services/fish-type.service";
import * as moment from "moment/moment";

@Component({
  selector: 'app-closed-season',
  templateUrl: './closed-season.component.html',
  styleUrls: ['./closed-season.component.scss']
})
export class ClosedSeasonComponent implements OnInit {
  public fishTypes: FishType[] = [];
  public fishTypeWithClosedSeason: FishType[] = [];

  private _toast: ToastrService = inject(ToastrService);
  private readonly _fishTypeService: FishTypeService = inject(FishTypeService);

  ngOnInit(): void {
    this.getFishTypes();
  }

  getFishTypes() {
    this._fishTypeService.getFishTypes().subscribe({
      next: ((response) => {
        if (response) {
          this.fishTypes = response;
          response.forEach((fishType) => this.checkFishTypeHasClosedSeason(fishType));
        }
      }),
      error: error => {
        this._toast.error(error.error ?? 'Fehler beim laden der Fischarten', 'Fischarten');
      }
    });
  }

  private checkFishTypeHasClosedSeason(fishType: FishType) {
    if (fishType.hasClosedSeason) {
      const today = new Date();
      const dateToCheck = new Date(today.getFullYear(), today.getMonth(), today.getDate());
      const closedSeasonFrom = new Date(today.getFullYear(), fishType.closedSeasonFromMonth! - 1, fishType.closedSeasonFromDay!);
      const closedSeasonTo = new Date(today.getFullYear(), fishType.closedSeasonToMonth! - 1, fishType.closedSeasonToDay!);
      if (moment(dateToCheck).isBetween(closedSeasonFrom, closedSeasonTo, undefined, '[]')) {
        fishType.closedSeasonsInDays = moment(closedSeasonTo).diff(dateToCheck, 'days');
        this.fishTypeWithClosedSeason.push(fishType);
      }
    }
  }
}
