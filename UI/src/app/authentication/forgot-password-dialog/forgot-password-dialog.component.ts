import {Component, inject} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {AuthenticationService} from "../../services/authentication.service";
import {ForgotPassword} from "../../models/forgotPassword.model";
import {environment} from "../../../environments/environment";
import {MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-forgot-password-dialog',
  templateUrl: './forgot-password-dialog.component.html',
  styleUrls: ['./forgot-password-dialog.component.scss']
})
export class ForgotPasswordDialogComponent {

  public forgotPasswordForm: FormGroup = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email])
  });
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);
  private readonly _dialogRef: MatDialogRef<ForgotPasswordDialogComponent> = inject(MatDialogRef<ForgotPasswordDialogComponent>);

  get email() {
    return this.forgotPasswordForm.get('email');
  }

  onSubmit() {
    if (this.forgotPasswordForm.invalid) {
      return;
    }
    const forgotPasswordRequest: ForgotPassword = {
      email: this.email?.value,
      clientUri: environment.resetPasswordUri
    };
    this._authenticationService.forgotPassword(forgotPasswordRequest);
    this.onClose();
  }

  onClose() {
    this._dialogRef.close();
  }
}
