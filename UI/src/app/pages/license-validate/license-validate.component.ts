import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {ValidateLicenseModel} from "../../models/validateLicense.model";
import {FishingLicenseService} from "../../services/fishing-license.service";

@Component({
    selector: 'app-license-validate',
    templateUrl: './license-validate.component.html',
    styleUrl: './license-validate.component.scss',
    standalone: false
})
export class LicenseValidateComponent implements OnInit{

  // Properties to hold license validation information
  public licenseId: string | null = null;
  public isValid: boolean = true;
  public errorReason: string = '';
  public isLoading: boolean = true;
  public validationResponse: ValidateLicenseModel | undefined;
  public isApiError: boolean = false;

  // Injecting ActivatedRoute and FishingLicenseService
  private readonly _activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  private readonly _licenseService: FishingLicenseService = inject(FishingLicenseService);

  ngOnInit(): void {
    // Get the license id from the route parameter
    this.licenseId = this._activatedRoute.snapshot.paramMap.get('id');
    if (!this.licenseId) {
      // If no license id provided, set validation status to false and show error message
      this.isValid = false;
      this.errorReason = "Keine Lizenznummer";
      this.isLoading = false;
    } else {
      // If license id is provided, validate the license
      this.validateLicense(this.licenseId);
    }
  }

  // Method to validate the license
  validateLicense(licenseId: string): void {
    // Call the service to validate the license
    this._licenseService.validateLicense(licenseId).subscribe({
      next: ((response: ValidateLicenseModel): void => {
        if (response.isValid) {
          // If license is valid, set validation status to true and store response
          this.isValid = true;
          this.validationResponse = response;
        } else {
          // If license is invalid, set validation status to false and store error message
          this.isValid = false;
          this.errorReason = response.errorMessage;
        }

        // Update loading status
        this.isLoading = false;
      }),
      error: (): void => {
        // If there's an error in API call, set validation status to false, show error message, and update loading status
        this.isLoading = false;
        this.isValid = false;
        this.isApiError = true;
        this.errorReason = "Lizenz konnte nicht validiert werden. Bitte versuchen Sie es später nochmals.";
      }
    });
  }
}
