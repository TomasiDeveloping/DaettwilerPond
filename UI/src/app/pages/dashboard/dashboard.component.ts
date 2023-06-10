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
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  public currentUser: User | undefined;
  public currentFishingLicence: FishingLicense | undefined;

  private readonly _userService: UserService = inject(UserService);
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);
  private readonly _toast: ToastrService = inject(ToastrService);
  private readonly _fishingLicenceService: FishingLicenseService = inject(FishingLicenseService);


  ngOnInit(): void {
    const userId = this._authenticationService.getUserIdFromToken();
    if (userId) {
      this.getUser(userId);
      this.getCurrentFishingLicence(userId);
    }
  }

  getUser(userId: string) {
    this._userService.getUserById(userId).subscribe({
      next: ((response) => {
        if (response) {
          this.currentUser = response;
        }
      }),
      error: error => {
        this._toast.error(error.error, 'Error');
      }
    });
  }

  getCurrentFishingLicence(userId: string) {
    this._fishingLicenceService.getCurrentUserLicence(userId).subscribe({
      next: ((response) => {
        if (response) {
          this.currentFishingLicence = response;
        }
      }),
      error: error => {
        this._toast.error(error.error ?? 'Fischerlizenz konnte nicht geladen werden');
    }
    });
  }

  getGreeting(): string {
    const hour = new Date().getHours();
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
