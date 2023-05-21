import {Component, inject, OnInit} from '@angular/core';
import {UserService} from "../../services/user.service";
import {ToastrService} from "ngx-toastr";
import {User} from "../../models/user.model";
import {AuthenticationService} from "../../services/authentication.service";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit{

  public currentUser: User | undefined;

  private _userService: UserService = inject(UserService);
  private _authenticationService: AuthenticationService = inject(AuthenticationService);
  private _toast: ToastrService = inject(ToastrService);

  ngOnInit(): void {
    const userId = this._authenticationService.getUserIdFromToken();
    if (userId) {
      this.getUser(userId);
    }
  }

  getUser(userId: string) {
    this._userService.getUserById(userId).subscribe({
      next: ((response) => {
        if (response) {
          this.currentUser = response;
        }
      })
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
