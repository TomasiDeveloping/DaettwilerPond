import {Component, inject, OnInit} from '@angular/core';
import {FishCatchService} from "../../services/fish-catch.service";
import {DetailYearlyCatchModel} from "../../models/detailYearlyCatch.model";
import {AuthenticationService} from "../../services/authentication.service";
import {FishingLicenseService} from "../../services/fishing-license.service";
import {Router} from "@angular/router";
import {YearlyCatchModel} from "../../models/yearlyCatch.model";
import {FishingLicense} from "../../models/fishingLicense.model";

@Component({
  selector: 'app-catch-statistics',
  templateUrl: './catch-statistics.component.html',
  styleUrl: './catch-statistics.component.scss'
})
export class CatchStatisticsComponent implements OnInit{

  // Array to store detailed yearly catch data for each month
  public detailCatches: DetailYearlyCatchModel[] = [];

  // Model to store yearly catch summary
  public yearlyCatch: YearlyCatchModel | undefined;

  // Array representing months (0-11)
  public months: number[] = Array(12);

  // Variable to store the current fishing license ID
  private currentLicence: string | undefined;

  // Angular services
  private readonly _fishCatchService: FishCatchService = inject(FishCatchService);
  private readonly _authService: AuthenticationService = inject(AuthenticationService);
  private readonly _licenceService: FishingLicenseService = inject(FishingLicenseService);
  private readonly _router: Router = inject(Router);

  ngOnInit(): void {
    // Get the user ID from the authentication service
    const userId: string | null = this._authService.getUserIdFromToken();

    // Check if the user ID is available
    if (!userId) {
     return;
   }

    // Fetch the current user's fishing license information
    this._licenceService.getCurrentUserLicence(userId).subscribe({
     next: ((response: FishingLicense): void => {
       if (response) {
         // Set the current fishing license ID
         this.currentLicence = response.id;

         // Fetch detailed yearly catches and yearly catch summary
         this.getDetailCatches(response.id);
         this.getYearlyCatch(response.id);
       }
     })
   });
  }

  // Fetch detailed yearly catches based on the fishing license ID
  getDetailCatches(licenceId: string): void {
    this._fishCatchService.getDetailYearlyCatches(licenceId).subscribe({
      next: ((response: DetailYearlyCatchModel[]): void => {
        if (response) {
          // Update the array with detailed yearly catch data
          this.detailCatches = response;
        }
      })
    });
  }

  // Fetch yearly catch summary based on the fishing license ID
  getYearlyCatch(licenceId: string): void {
    this._fishCatchService.getYearlyCatch(licenceId).subscribe({
      next: ((response: YearlyCatchModel): void => {
        if (response) {
          // Update the model with yearly catch summary
          this.yearlyCatch = response;
        }
      })
    });
  }

  // Check if detailed yearly catch data is available for a specific month
  checkCatch(month: number): DetailYearlyCatchModel | null {
    if (this.detailCatches.length <= 0) {
      return null;
    } else  {
      // Find and return the detailed catch data for the specified month
      const detailCatch: DetailYearlyCatchModel | undefined = this.detailCatches.find((x: DetailYearlyCatchModel): boolean => x.month === month);
      return detailCatch ?? null;
    }

  }

  // Navigate to the detailed monthly statistics page for a specific month
  onMonthDetail(month: number): void {
    this._router.navigate(['monatstatistik', this.currentLicence, month]).then();
  }
}
