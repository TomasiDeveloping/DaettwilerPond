import {Component, inject} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {PasswordValidators} from "../../../helpers/password-validators";
import {AuthenticationService} from "../../../services/authentication.service";
import {ChangePassword} from "../../../models/changePassword.model";
import {UserService} from "../../../services/user.service";
import {ToastrService} from "ngx-toastr";
import Swal from "sweetalert2";

@Component({
    selector: 'app-change-password',
    templateUrl: './change-password.component.html',
    styleUrls: ['./change-password.component.scss'],
    standalone: false
})
export class ChangePasswordComponent {

  public changePasswordForm: FormGroup = new FormGroup({
    userId: new FormControl<string>(''),
    currentPassword: new FormControl<string>('', [Validators.required]),
    confirmPassword: new FormControl<string>('', [Validators.required]),
    password: new FormControl<string>('', Validators.compose([
      Validators.required,
      PasswordValidators.patternValidator(RegExp("(?=.*[0-9])"), {hasNumber: true}),
      PasswordValidators.patternValidator(RegExp("(?=.*[A-Z])"), {hasCapitalCase: true}),
      PasswordValidators.patternValidator(RegExp("(?=.*[a-z])"), {hasSmallCase: true}),
      PasswordValidators.patternValidator(RegExp("(?=.*[$@^!%*?&+])"), {hasSpecialCharacters: true}),
      Validators.minLength(8)
    ]))
  }, {
    validators: PasswordValidators.passwordMatch('password', 'confirmPassword')
  });
  public isInputText: boolean = false;
  private readonly _AuthenticationService: AuthenticationService = inject(AuthenticationService);
  private readonly _userService: UserService = inject(UserService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  get password() {
    return this.changePasswordForm.get('password');
  }

  get confirmPassword() {
    return this.changePasswordForm.get('confirmPassword');
  }

  get currentPassword() {
    return this.changePasswordForm.get('currentPassword');
  }

  onSubmit() {
    if (this.changePasswordForm.invalid) {
      return;
    }
    const currentUserId = this._AuthenticationService.getUserIdFromToken();
    if (!currentUserId) {
      return;
    }
    const changePassword: ChangePassword = this.changePasswordForm.value;
    changePassword.userId = currentUserId;
    this._userService.changeUserPassword(changePassword).subscribe({
      next: ((response) => {
        if (response) {
          Swal.fire('Neues Passwort', 'Passwort wurde erfolgreich geändert, Du wirst automatisch ausgeloogt', 'info').then(() => {
            this._AuthenticationService.logout();
          })
        } else {
          this._toastr.error('Passwort konnte nicht geändert werden', 'Neues Passwort');
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Passwort konnte nicht geändert werden', 'Neues Passwort');
      }
    });
  }
}
