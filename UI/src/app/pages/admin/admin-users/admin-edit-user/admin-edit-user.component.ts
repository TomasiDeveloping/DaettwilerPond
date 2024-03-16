import {Component, inject, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from "@angular/material/dialog";
import {UserWithAddress} from "../../../../models/user.model";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ToastrService} from "ngx-toastr";
import {Registration} from "../../../../models/registration.model";
import {AuthenticationService} from "../../../../services/authentication.service";
import {UserService} from "../../../../services/user.service";
import {UserImageUploadComponent} from "./user-image-upload/user-image-upload.component";

@Component({
  selector: 'app-admin-edit-user',
  templateUrl: './admin-edit-user.component.html'
})
export class AdminEditUserComponent {

  public currentUser: UserWithAddress;
  public isUpdate: boolean;
  public userForm: FormGroup;

  private readonly _dialogRef: MatDialogRef<AdminEditUserComponent> = inject(MatDialogRef<AdminEditUserComponent>);
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);
  private readonly _userService: UserService = inject(UserService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _dialog: MatDialog = inject(MatDialog);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { isUpdate: boolean, user: UserWithAddress }) {
    this.isUpdate = data.isUpdate;
    this.currentUser = data.user;
    this.userForm = new FormGroup({
      userId: new FormControl<string>(this.currentUser.userId),
      firstName: new FormControl<string>(this.currentUser.firstName, [Validators.required]),
      lastName: new FormControl<string>(this.currentUser.lastName, [Validators.required]),
      email: new FormControl<string>(this.currentUser.email, [Validators.required, Validators.email]),
      role: new FormControl<string>(this.currentUser.role ?? ''),
      isActive: new FormControl<boolean>(this.currentUser.isActive),
      saNaNumber: new FormControl<string | undefined>(this.currentUser.saNaNumber),
      address: new FormGroup({
        street: new FormControl<string>(this.currentUser.address.street),
        houseNumber: new FormControl<string>(this.currentUser.address.houseNumber),
        postalCode: new FormControl<string>(this.currentUser.address.postalCode),
        city: new FormControl<string>(this.currentUser.address.city),
        country: new FormControl<string>('CH'),
        phone: new FormControl<string>(this.currentUser.address.phone),
        mobile: new FormControl<string>(this.currentUser.address.mobile)
      })
    });
  }

  get firstName() {
    return this.userForm.get('firstName');
  }

  get lastName() {
    return this.userForm.get('lastName');
  }

  get email() {
    return this.userForm.get('email');
  }

  onSubmit() {
    if (this.userForm.invalid) {
      return;
    }
    if (this.isUpdate) {
      const userWithAddress: UserWithAddress = this.userForm.value as UserWithAddress;
      this.updateUserWithAddress(userWithAddress);
    } else {
      const registration: Registration = this.userForm.value as Registration;
      this.registerNewUser(registration);
    }
  }

  updateUserWithAddress(userWithAddress: UserWithAddress) {
    this._userService.updateUserWithAddress(userWithAddress).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success(`User ${response.firstName} erfolgreich geupdated`, 'Update User');
          this.onClose(true);
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'User konnte nicht geupdated werden', 'Update User');
      }
    });
  }

  registerNewUser(registration: Registration) {
    this._authenticationService.register(registration).subscribe({
      next: ((response) => {
        if (response.isSuccessful) {
          this._toastr.success(`Neuer User ${registration.firstName} wurde erfolgreich erstellt`, 'Neuer User');
          this.onClose(true);
        } else {
          this._toastr.error(response.errors.join(), 'Neuer User');
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Neuer User konnte nicht ertsellt werden', 'Neuer User');
      }
    });
  }

  onClose(reload: boolean) {
    const result = {reload: reload};
    this._dialogRef.close(result);
  }

  unUploadImage() {
    const dialogRef = this._dialog.open(UserImageUploadComponent, {
      width: '60%',
      height: 'auto',
      data: {userId: this.currentUser.userId, imageUrl: this.currentUser.imageUrl}
    });

    dialogRef.afterClosed().subscribe({
      next: ((reload: boolean) => {
        if (reload) {
          this.onClose(reload);
        }
      })
    });
  }
}
