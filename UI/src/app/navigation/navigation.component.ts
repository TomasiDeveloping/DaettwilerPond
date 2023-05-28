import {Component, inject, OnInit} from '@angular/core';
import {environment} from "../../environments/environment";
import {AuthenticationService} from "../services/authentication.service";
import {User} from "../models/user.model";
import {UserService} from "../services/user.service";

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit{
  public isUserLoggedIn: boolean = false;
  public isShown: boolean = false;
  public isAdminUser: boolean = false;


  public version: string = environment.version;

  private readonly _userService: UserService = inject(UserService);
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);
  public currentUser: User | undefined;

  ngOnInit(): void {
    this._authenticationService.autoLogin();
    this._authenticationService.authChangeNotification.subscribe({
      next: ((isLoggedIn) => {
        this.isUserLoggedIn = isLoggedIn;
        this.isAdminUser = this._authenticationService.isUserAdministrator();
        const userId = this._authenticationService.getUserIdFromToken();
        if (userId) {
          this.getUser(userId);
        }
      })
    });
  }

  getUser(userId: string) {
    this._userService.getUserById(userId).subscribe({
      next: ((response) => {
        if (response) {
          this.currentUser = response;
        }
      }),
      error: error => {
        console.log(error);
      }
    });
  }

  onLogout() {
    this.isShown = false;
    this._authenticationService.logout();
  }
}
