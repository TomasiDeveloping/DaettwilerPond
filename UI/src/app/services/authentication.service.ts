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

  private _authChangeSubscription$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  public authChangeNotification: Observable<boolean> = this._authChangeSubscription$.asObservable();

  private readonly _serviceUrl: string = environment.apiBaseUrl + '/Authentications';
  private readonly _httpClient: HttpClient = inject(HttpClient);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _jwtHelper: JwtHelperService = inject(JwtHelperService);
  private readonly _router: Router = inject(Router);

  public login(loginRequest: LoginRequest) {
    this._httpClient.post<LoginResponse>(this._serviceUrl + '/Login', loginRequest).subscribe({
      next: ((response) => {
        if (response.isSuccessful) {
          this.setToken(response.token);
          this._authChangeSubscription$.next(true);
          this._router.navigate(['/dashboard']).then();
        } else {
          this._toastr.error(response.errorMessage, 'Login');
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Error', 'Login');
      }
    });
  }

  public register(register: Registration): Observable<{isSuccessful: boolean, errors: string[]}> {
    return this._httpClient.post<{isSuccessful: boolean, errors: string[]}>(this._serviceUrl + '/Register', register);
  }

  public logout() {
    this.removeToken();
    this._authChangeSubscription$.next(false);
    this._router.navigate(['/home']).then();
  }

  public isUserAuthenticated(): boolean {
    const token: string | null = localStorage.getItem('DaettwilerPondToken');
    if (token) {
      return !this._jwtHelper.isTokenExpired(token);
    }
    return false;
  }

  public isUserAdministrator(): boolean {
    const token: string | null = localStorage.getItem('DaettwilerPondToken');
    if (token) {
      const decodedToken = this._jwtHelper.decodeToken(token);
      return decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === 'Administrator';
    }
    return false;
  }

  public autoLogin() {
    const token = localStorage.getItem('DaettwilerPondToken');
    if (token) {
      if (this._jwtHelper.isTokenExpired(token)) {
        this.removeToken();
        this._authChangeSubscription$.next(false);
        this._router.navigate(['/home']).then(() => {
          this._toastr.info('Sitzung is abgelaufen, bitte erneut einloggen', 'Logout');
        })
      } else {
        this._authChangeSubscription$.next(true);
      }
    }
  }

  public forgotPassword(forgotPassword: ForgotPassword): void {
    this._httpClient.post<{
      isSuccessful: boolean,
      errorMessage: string
    }>(this._serviceUrl + '/ForgotPassword', forgotPassword).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.info('Der Link wurde gesendet. Bitte überprüfen Sie Ihre E-Mail (Spam), um Ihr Passwort zurückzusetzen.', 'Passwort Reset');
        }
      })
    });
  }

  public resetPassword(resetPasswordRequest: ResetPassword) {
    this._httpClient.post<{
      isSuccessful: boolean,
      errorMessage: string[]
    }>(this._serviceUrl + '/resetPassword', resetPasswordRequest).subscribe({
      next: ((response) => {
        if (response.isSuccessful) {
          this._router.navigate(['/login']).then(() => {
            this._toastr.success('Passwort erfolgreich zurückgesetzt', 'Passwort zurücksetzen');
          });
        } else {
          this._toastr.error(response.errorMessage.join(), 'Passwort zurücksetzen');
        }
      }), error: error => {
        this._router.navigate(['/login']).then(() => {
          this._toastr.error(error.error.errors.join() ?? 'Es gab einen Fehler, versuche es erneut', 'Passwort zurücksetzen');
        });
      }
    });
  }

  public getUserIdFromToken(): string | null {
    const token = localStorage.getItem('DaettwilerPondToken');
    if (token) {
      const decodedToken = this._jwtHelper.decodeToken<{ userId: string }>(token);
      return decodedToken!.userId;
    }
    return null;
  }

  private setToken(token: string) {
    localStorage.setItem('DaettwilerPondToken', token);
  }

  private removeToken() {
    localStorage.removeItem('DaettwilerPondToken');
  }
}
