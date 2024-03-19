import {Component, inject, OnInit} from '@angular/core';
import {environment} from "../../environments/environment";
import {AuthenticationService} from "../services/authentication.service";
import {User} from "../models/user.model";
import {UserService} from "../services/user.service";
import {L10n, loadCldr, setCulture} from '@syncfusion/ej2-base';
import * as syncfusionGridTranslateDe from "../../assets/i18n/grid/de.json";

loadCldr(syncfusionGridTranslateDe);
setCulture('de');
L10n.load(syncfusionGridTranslateDe);

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit {
  public isUserLoggedIn: boolean = false;
  public isShown: boolean = false;
  public isAdminUser: boolean = false;
  public isOverseerOrHigher: boolean = false;

  // Application version from environment
  public version: string = environment.version;
  public currentUser: User | undefined;

  // Injecting necessary services
  private readonly _userService: UserService = inject(UserService);
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);

  ngOnInit(): void {
    // Auto login and subscribe to authentication changes
    this._authenticationService.autoLogin();
    this._authenticationService.authChangeNotification.subscribe({
      next: ((isLoggedIn: boolean): void => {
        // Update user login status and admin status on authentication change
        this.isUserLoggedIn = isLoggedIn;
        this.isAdminUser = this._authenticationService.isUserAdministrator();
        this.isOverseerOrHigher = this._authenticationService.isUserOverseerOrHigher();

        // Retrieve and update current user information if logged in
        const userId: string | null = this._authenticationService.getUserIdFromToken();
        if (userId) {
          this.getUser(userId);
        }
      })
    });
  }

  // Retrieve user information by user ID
  getUser(userId: string): void {
    this._userService.getUserById(userId).subscribe({
      next: ((response: User): void => {
        // Update current user if the user information is retrieved successfully
        if (response) {
          this.currentUser = response;
        }
      }),
      error: _ => {
        // Handle error if user information retrieval fails
      }
    });
  }

  // Logout function triggered on user click
  onLogout(): void {
    this.isShown = false;
    this._authenticationService.logout();
  }

  onVersion(): void {
    window.open('https://github.com/TomasiDeveloping/DaettwilerPond', '_blank');
  }
}
