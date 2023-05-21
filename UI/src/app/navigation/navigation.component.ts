import {Component, inject, OnInit} from '@angular/core';
import {environment} from "../../environments/environment";
import {AuthenticationService} from "../services/authentication.service";

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit{
  public isUserLoggedIn: boolean = false;
  public isShown: boolean = false;


  public version: string = environment.version;

  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);
  currentUser: any;

  ngOnInit(): void {
    this._authenticationService.autoLogin();
    this._authenticationService.authChangeNotification.subscribe({
      next: ((isLoggedIn) => {
        this.isUserLoggedIn = isLoggedIn;
      })
    });
  }

  onLogout() {
    this._authenticationService.logout();
  }
}
