import {Component, inject} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {AuthenticationService} from "../../services/authentication.service";
import {LoginRequest} from "../../models/loginRequest.modell";
import {MatDialog} from "@angular/material/dialog";
import {ForgotPasswordDialogComponent} from "../forgot-password-dialog/forgot-password-dialog.component";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  public isText: boolean = false;
  public loginForm: FormGroup = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    password: new FormControl<string>('', [Validators.required])
  });

  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);
  private readonly _dialog: MatDialog = inject(MatDialog);

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      return;
    }
    const loginRequest: LoginRequest = this.loginForm.value as LoginRequest;
    this._authenticationService.login(loginRequest);
  }

  onForgotPassword() {
    this._dialog.open(ForgotPasswordDialogComponent, {
      width: '60%',
      height: 'auto',
      autoFocus: false
    });
  }
}
