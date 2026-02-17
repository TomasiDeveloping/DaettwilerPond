import {Component, inject, OnInit} from '@angular/core';
import {UserService} from "../../services/user.service";
import {ToastrService} from "ngx-toastr";
import {User} from "../../models/user.model";
import {AuthenticationService} from "../../services/authentication.service";
import {FishingLicense} from "../../models/fishingLicense.model";
import {FishingLicenseService} from "../../services/fishing-license.service";


@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
    standalone: false
})
export class DashboardComponent implements OnInit {

  // Public properties to store current user and fishing license information
  public currentUser: User | undefined;
  public currentFishingLicence: FishingLicense | undefined;

  // Private properties for UserService, AuthenticationService, ToastrService, and FishingLicenseService using Angular DI
  private readonly _userService: UserService = inject(UserService);
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);
  private readonly _toast: ToastrService = inject(ToastrService);
  private readonly _fishingLicenceService: FishingLicenseService = inject(FishingLicenseService);


  ngOnInit(): void {
    // Retrieve the user ID from the authentication service
    const userId: string | null = this._authenticationService.getUserIdFromToken();

    // If user ID is available, fetch user and fishing license information
    if (userId) {
      this.getUser(userId);
      this.getCurrentFishingLicence(userId);
    }
  }

  // Method to fetch user information by user ID
  getUser(userId: string) {
    this._userService.getUserById(userId).subscribe({
      next: ((response: User): void => {
        if (response) {
          // Assigning fetched user information to the public property
          this.currentUser = response;
        }
      }),
      // Handling errors and displaying toastr messages
      error: error => {
        this._toast.error(error.error, 'Error');
      }
    });
  }

  // Method to fetch the current fishing license by user ID
  getCurrentFishingLicence(userId: string) {
    this._fishingLicenceService.getCurrentUserLicence(userId).subscribe({
      next: ((response: FishingLicense): void => {
        if (response) {
          // Assigning fetched fishing license information to the public property
          this.currentFishingLicence = response;
        }
      }),
      // Handling errors and displaying toastr messages
      error: error => {
        this._toast.error(error.error ?? 'Fischerlizenz konnte nicht geladen werden');
    }
    });
  }

  // Method to determine the appropriate greeting based on the current hour
  getGreeting(): string {
    const hour: number = new Date().getHours();
    if (hour >= 5 && hour <= 11) {
      return 'Guten Morgen,';
    }
    if (hour >= 11 && hour <= 18) {
      return 'Guten Tag,';
    }
    if (hour >= 18 && hour <= 22) {
      return 'Guten Abend,';
    }
    return 'Hy,'
  }

}
