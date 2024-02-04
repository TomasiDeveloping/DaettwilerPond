import {Component, inject} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {AuthenticationService} from "../../services/authentication.service";
import {ForgotPassword} from "../../models/forgotPassword.model";
import {environment} from "../../../environments/environment";
import {MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-forgot-password-dialog',
  templateUrl: './forgot-password-dialog.component.html'
})
export class ForgotPasswordDialogComponent {

  // Initialize the form group for the forgot password form with email validation
  public forgotPasswordForm: FormGroup = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email])
  });

  // Inject AuthenticationService and MatDialogRef for service and dialog functionality
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);
  private readonly _dialogRef: MatDialogRef<ForgotPasswordDialogComponent> = inject(MatDialogRef<ForgotPasswordDialogComponent>);

  // Getter for email control to access it easily in the template
  get email() {
    return this.forgotPasswordForm.get('email');
  }

  // Handle form submission
  onSubmit(): void {
    // Check if the form is invalid before proceeding
    if (this.forgotPasswordForm.invalid) {
      return;
    }

    // Create a ForgotPassword object with email and client URI
    const forgotPasswordRequest: ForgotPassword = {
      email: this.email?.value,
      clientUri: environment.resetPasswordUri
    };

    // Call the AuthenticationService method for forgot password
    this._authenticationService.forgotPassword(forgotPasswordRequest);

    // Close the dialog after submission
    this.onClose();
  }

  // Close the dialog
  onClose(): void {
    this._dialogRef.close();
  }
}
