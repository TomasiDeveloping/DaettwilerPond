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

  // Flag to toggle between text and password input for password visibility
  public isText: boolean = false;

  // Initialize the login form with email and password controls
  public loginForm: FormGroup = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    password: new FormControl<string>('', [Validators.required])
  });

  // Inject AuthenticationService and MatDialog for service and dialog functionality
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);
  private readonly _dialog: MatDialog = inject(MatDialog);

  // Getter for email control to access it easily in the template
  get email() {
    return this.loginForm.get('email');
  }

  // Getter for password control to access it easily in the template
  get password() {
    return this.loginForm.get('password');
  }

  // Handle form submission for login
  onSubmit(): void {
    // Check if the form is invalid before proceeding
    if (this.loginForm.invalid) {
      return;
    }

    // Create a LoginRequest object from form values
    const loginRequest: LoginRequest = this.loginForm.value as LoginRequest;

    // Call the AuthenticationService method for login
    this._authenticationService.login(loginRequest);
  }

  // Open the forgot password dialog
  onForgotPassword(): void {
    this._dialog.open(ForgotPasswordDialogComponent, {
      width: '60%',
      height: 'auto',
      autoFocus: false
    });
  }
}
