import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {PasswordValidators} from "../../helpers/password-validators";
import {ToastrService} from "ngx-toastr";
import {ResetPassword} from "../../models/resetPassword.model";
import {AuthenticationService} from "../../services/authentication.service";

@Component({
  selector: 'app-password-reset',
  templateUrl: './password-reset.component.html',
  styleUrls: ['./password-reset.component.scss']
})
export class PasswordResetComponent implements OnInit{

  private _email: string | undefined;
  private _token: string | undefined;
  private readonly _route: ActivatedRoute = inject(ActivatedRoute);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _router: Router = inject(Router);
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);

  public resetForm: FormGroup = new FormGroup({
    confirmPassword: new FormControl<string>('', [Validators.required]),
    password: new FormControl<string>('', Validators.compose([
      Validators.required,
      PasswordValidators.patternValidator(new RegExp("(?=.*[0-9])"), {hasNumber: true}),
      PasswordValidators.patternValidator(new RegExp("(?=.*[A-Z])"), {hasCapitalCase: true}),
      PasswordValidators.patternValidator(new RegExp("(?=.*[a-z])"), {hasSmallCase: true}),
      PasswordValidators.patternValidator(new RegExp("(?=.*[$@^!%*?&+])"), {hasSpecialCharacters: true}),
      Validators.minLength(8)
    ]))
  }, {
    validators: PasswordValidators.passwordMatch('password', 'confirmPassword')
  });
  public isInputText: boolean = false;

  get password() {
    return this.resetForm.get('password');
  }

  get confirmPassword() {
    return this.resetForm.get('confirmPassword');
  }
  ngOnInit(): void {
    this._email = this._route.snapshot.queryParams['email'];
    this._token = this._route.snapshot.queryParams['token'];
  }

  onSubmit() {
    if (this.resetForm.invalid) {
      return;
    }
    if (!this._email || !this._token) {
      this._toastr.error('Es gibt einen Fehler. Bitte erneut Passwort vergessen anfordern', 'Fehler');
      this._router.navigate(['/login']).then();
      return;
    }
    const resetPasswordRequest: ResetPassword = {
      password: this.password?.value,
      token: this._token,
      email: this._email
    };
    this._authenticationService.resetPassword(resetPasswordRequest);
  }
}
