import {Component, inject, OnInit} from '@angular/core';
import {FishCatchService} from "../../services/fish-catch.service";
import {DetailYearlyCatchModel} from "../../models/detailYearlyCatch.model";
import {AuthenticationService} from "../../services/authentication.service";
import {FishingLicenseService} from "../../services/fishing-license.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-catch-statistics',
  templateUrl: './catch-statistics.component.html',
  styleUrl: './catch-statistics.component.scss'
})
export class CatchStatisticsComponent implements OnInit{

  public detailCatches: DetailYearlyCatchModel[] = [];
  public months: number[] = Array(12);
  private currentLicence: string | undefined;
  private readonly _fishCatchService: FishCatchService = inject(FishCatchService);
  private readonly _authService: AuthenticationService = inject(AuthenticationService);
  private readonly _licenceService: FishingLicenseService = inject(FishingLicenseService);
  private readonly _router: Router = inject(Router);

  ngOnInit() {
   const userId = this._authService.getUserIdFromToken();
   if (!userId) {
     return;
   }
   this._licenceService.getCurrentUserLicence(userId).subscribe({
     next: ((response) => {
       if (response) {
         this.currentLicence = response.id;
         this.getDetailCatches(response.id);
       }
     })
   });
  }

  getDetailCatches(licenceId: string) {
    this._fishCatchService.getDetailYearlyCatches(licenceId).subscribe({
      next: ((response) => {
        if (response) {
          this.detailCatches = response;
        }
      })
    });
  }

  checkCatch(month: number): DetailYearlyCatchModel | null {
    if (this.detailCatches.length < 0) {
      return null;
    } else  {
      const detailCatch = this.detailCatches.find((x) => x.month === month);
      return detailCatch ? detailCatch : null;
    }

  }

  onMonthDetail(month: number) {
    this._router.navigate(['monatstatistik', this.currentLicence, month]).then();
  }
}
