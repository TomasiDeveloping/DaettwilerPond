import {Component, inject, OnInit} from '@angular/core';
import {FishType} from "../../../models/fishType.model";
import {ToastrService} from "ngx-toastr";
import {FishTypeService} from "../../../services/fish-type.service";
import moment from "moment/moment";

@Component({
    selector: 'app-closed-season',
    templateUrl: './closed-season.component.html',
    styleUrls: ['./closed-season.component.scss'],
    standalone: false
})
export class ClosedSeasonComponent implements OnInit {

  // Public properties to store fish types and those with closed season
  public fishTypes: FishType[] = [];
  public fishTypeWithClosedSeason: FishType[] = [];

  // Private properties for ToastrService and FishTypeService using Angular DI
  private _toast: ToastrService = inject(ToastrService);
  private readonly _fishTypeService: FishTypeService = inject(FishTypeService);

  ngOnInit(): void {
    this.getFishTypes();
  }

  // Method to fetch fish types from the FishTypeService
  getFishTypes(): void {
    this._fishTypeService.getFishTypes().subscribe({
      next: ((response: FishType[]): void => {
        if (response) {
          // Assigning fetched fish types to the public property
          this.fishTypes = response;

          // Checking each fish type for closed season and updating the list accordingly
          response.forEach((fishType: FishType) => this.checkFishTypeHasClosedSeason(fishType));
        }
      }),
      // Handling errors and displaying toastr messages
      error: error => {
        this._toast.error(error.error ?? 'Fehler beim laden der Fischarten', 'Fischarten');
      }
    });
  }

  // Private method to check if a fish type has a closed season and update the list
  private checkFishTypeHasClosedSeason(fishType: FishType): void {
    if (fishType.hasClosedSeason) {
      // Creating Date objects for today, closed season start, and closed season end
      const today: Date = new Date();
      const dateToCheck: Date = new Date(today.getFullYear(), today.getMonth(), today.getDate());
      const closedSeasonFrom: Date = new Date(today.getFullYear(), fishType.closedSeasonFromMonth! - 1, fishType.closedSeasonFromDay);
      const closedSeasonTo: Date = new Date(today.getFullYear(), fishType.closedSeasonToMonth! - 1, fishType.closedSeasonToDay);

      // Checking if today is within the closed season range using moment.js
      if (moment(dateToCheck).isBetween(closedSeasonFrom, closedSeasonTo, undefined, '[]')) {
        // Calculating the remaining days in the closed season and updating the list
        fishType.closedSeasonsInDays = moment(closedSeasonTo).diff(dateToCheck, 'days');
        this.fishTypeWithClosedSeason.push(fishType);
      }
    }
  }
}
