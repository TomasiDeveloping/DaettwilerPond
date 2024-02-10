import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {LoginRequest} from "../models/loginRequest.modell";
import {LoginResponse} from "../models/loginResponse.model";
import {ToastrService} from "ngx-toastr";
import {JwtHelperService} from "@auth0/angular-jwt";
import {Router} from "@angular/router";
import {BehaviorSubject, Observable} from "rxjs";
import {ForgotPassword} from "../models/forgotPassword.model";
import {ResetPassword} from "../models/resetPassword.model";
import {Registration} from "../models/registration.model";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  // BehaviorSubject for authentication state changes
  private _authChangeSubscription$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  // Observable for components to subscribe to authentication changes
  public authChangeNotification: Observable<boolean> = this._authChangeSubscription$.asObservable();

  // API base URL obtained from environment
  private readonly _serviceUrl: string = environment.apiBaseUrl + '/Authentications';

  // Injecting necessary services
  private readonly _httpClient: HttpClient = inject(HttpClient);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _jwtHelper: JwtHelperService = inject(JwtHelperService);
  private readonly _router: Router = inject(Router);

  // Method to handle user login
  public login(loginRequest: LoginRequest): void {
    this._httpClient.post<LoginResponse>(this._serviceUrl + '/Login', loginRequest).subscribe({
      next: ((response: LoginResponse): void => {
        if (response.isSuccessful) {
          // Set authentication token and notify components of login
          this.setToken(response.token);
          this._authChangeSubscription$.next(true);
          // Navigate to the dashboard on successful login
          this._router.navigate(['/dashboard']).then();
        } else {
          // Show toastr message on unsuccessful login
          this._toastr.error(response.errorMessage, 'Login');
        }
      }),
      error: error => {
        // Show toastr message on error during login
        this._toastr.error(error.error ?? 'Error', 'Login');
      }
    });
  }

  // Method to handle user registration
  public register(register: Registration): Observable<{ isSuccessful: boolean, errors: string[] }> {
    return this._httpClient.post<{ isSuccessful: boolean, errors: string[] }>(this._serviceUrl + '/Register', register);
  }

  // Method to handle user logout
  public logout(): void {
    // Remove authentication token, notify components of logout, and navigate to home
    this.removeToken();
    this._authChangeSubscription$.next(false);
    this._router.navigate(['/home']).then();
  }

  // Check if the user is authenticated
  public isUserAuthenticated(): boolean {
    const token: string | null = localStorage.getItem('DaettwilerPondToken');
    if (token) {
      return !this._jwtHelper.isTokenExpired(token);
    }
    return false;
  }

  // Check if the user is an administrator
  public isUserAdministrator(): boolean {
    const token: string | null = localStorage.getItem('DaettwilerPondToken');
    if (token) {
      const decodedToken = this._jwtHelper.decodeToken(token);
      return decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === 'Administrator';
    }
    return false;
  }

  public isUserOverseerOrHigher(): boolean {
    const token: string | null = localStorage.getItem('DaettwilerPondToken');
    if (token) {
      const decodedToken = this._jwtHelper.decodeToken(token);
      const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return role === 'Administrator' || role === 'Aufseher';
    }
    return false;
  }

  // Automatically log in the user if a valid token exists
  public autoLogin(): void {
    const token: string | null = localStorage.getItem('DaettwilerPondToken');
    if (token) {
      if (this._jwtHelper.isTokenExpired(token)) {
        // Token is expired, remove it, notify components of logout, and navigate to home
        this.removeToken();
        this._authChangeSubscription$.next(false);
        this._router.navigate(['/home']).then((): void => {
          this._toastr.info('Sitzung is abgelaufen, bitte erneut einloggen', 'Logout');
        })
      } else {
        this._authChangeSubscription$.next(true);
      }
    }
  }

  // Method to handle forgot password functionality
  public forgotPassword(forgotPassword: ForgotPassword): void {
    this._httpClient.post<{
      isSuccessful: boolean,
      errorMessage: string
    }>(this._serviceUrl + '/ForgotPassword', forgotPassword).subscribe({
      next: ((response:  {isSuccessful: boolean, errorMessage: string}): void => {
        if (response) {
          // Show success message for password reset link sent
          this._toastr.info('Der Link wurde gesendet. Bitte überprüfen Sie Ihre E-Mail (Spam), um Ihr Passwort zurückzusetzen.', 'Passwort Reset');
        }
      })
    });
  }

  // Method to handle user password reset
  public resetPassword(resetPasswordRequest: ResetPassword): void {
    this._httpClient.post<{
      isSuccessful: boolean,
      errorMessage: string[]
    }>(this._serviceUrl + '/resetPassword', resetPasswordRequest).subscribe({
      next: ((response:  {isSuccessful: boolean, errorMessage: string[]}): void => {
        if (response.isSuccessful) {
          // Navigate to login page on successful password reset
          this._router.navigate(['/login']).then((): void => {
            this._toastr.success('Passwort erfolgreich zurückgesetzt', 'Passwort zurücksetzen');
          });
        } else {
          // Show error message on unsuccessful password reset
          this._toastr.error(response.errorMessage.join(), 'Passwort zurücksetzen');
        }
      }), error: error => {
        // Show error message on server error during password reset
        this._router.navigate(['/login']).then((): void => {
          this._toastr.error(error.error.errors.join() ?? 'Es gab einen Fehler, versuche es erneut', 'Passwort zurücksetzen');
        });
      }
    });
  }

  // Get user ID from the stored token
  public getUserIdFromToken(): string | null {
    const token: string | null = localStorage.getItem('DaettwilerPondToken');
    if (token) {
      const decodedToken: {userId: string} = this._jwtHelper.decodeToken<{ userId: string }>(token)!;
      return decodedToken.userId;
    }
    return null;
  }

  // Set authentication token in local storage
  private setToken(token: string): void {
    localStorage.setItem('DaettwilerPondToken', token);
  }

  // Remove authentication token from local storage
  private removeToken(): void {
    localStorage.removeItem('DaettwilerPondToken');
  }
}
