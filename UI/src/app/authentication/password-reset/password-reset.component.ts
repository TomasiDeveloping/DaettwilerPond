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
export class PasswordResetComponent implements OnInit {

  // Initialize the password reset form with password and confirm password controls
  public resetForm: FormGroup = new FormGroup({
    confirmPassword: new FormControl<string>('', [Validators.required]),
    password: new FormControl<string>('', Validators.compose([
      Validators.required,
      // Custom password validators for pattern and complexity
      PasswordValidators.patternValidator(new RegExp("(?=.*[0-9])"), {hasNumber: true}),
      PasswordValidators.patternValidator(new RegExp("(?=.*[A-Z])"), {hasCapitalCase: true}),
      PasswordValidators.patternValidator(new RegExp("(?=.*[a-z])"), {hasSmallCase: true}),
      PasswordValidators.patternValidator(new RegExp("(?=.*[$@^!%*?&+])"), {hasSpecialCharacters: true}),
      Validators.minLength(8)
    ]))
  }, {
    // Custom validator to ensure password and confirm password match
    validators: PasswordValidators.passwordMatch('password', 'confirmPassword')
  });

  // Flag to toggle between text and password input for password visibility
  public isInputText: boolean = false;

  // Private variables for email and token retrieved from route query parameters
  private _email: string | undefined;
  private _token: string | undefined;

  // Injected Angular services and components
  private readonly _route: ActivatedRoute = inject(ActivatedRoute);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _router: Router = inject(Router);
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);

  // Getter for password control to access it easily in the template
  get password() {
    return this.resetForm.get('password');
  }

  // Getter for confirm password control to access it easily in the template
  get confirmPassword() {
    return this.resetForm.get('confirmPassword');
  }

  // Lifecycle hook to initialize email and token from route query parameters
  ngOnInit(): void {
    this._email = this._route.snapshot.queryParams['email'];
    this._token = this._route.snapshot.queryParams['token'];
  }

  // Handle form submission for password reset
  onSubmit(): void {
    // Check if the form is invalid before proceeding
    if (this.resetForm.invalid) {
      return;
    }
    // Check if email and token are available
    if (!this._email || !this._token) {
      // Display an error message and navigate to the login page
      this._toastr.error('Es gibt einen Fehler. Bitte erneut Passwort vergessen anfordern', 'Fehler');
      this._router.navigate(['/login']).then();
      return;
    }

    // Create a ResetPassword object with password, token, and email
    const resetPasswordRequest: ResetPassword = {
      password: this.password?.value,
      token: this._token,
      email: this._email
    };

    // Call the AuthenticationService method for password reset
    this._authenticationService.resetPassword(resetPasswordRequest);
  }
}
