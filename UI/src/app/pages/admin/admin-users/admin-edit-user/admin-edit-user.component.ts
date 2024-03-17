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

  // Public properties
  public currentUser: UserWithAddress;
  public isUpdate: boolean;
  public userForm: FormGroup;

  // Dependency injection
  private readonly _dialogRef: MatDialogRef<AdminEditUserComponent> = inject(MatDialogRef<AdminEditUserComponent>);
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);
  private readonly _userService: UserService = inject(UserService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _dialog: MatDialog = inject(MatDialog);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { isUpdate: boolean, user: UserWithAddress }) {
    // Initialize public properties
    this.isUpdate = data.isUpdate;
    this.currentUser = data.user;

    // Initialize user form with default values
    const date: Date = new Date(this.currentUser.dateOfBirth);
    const userDateOfBirth: string = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate())).toISOString().substring(0,10);

    // Initialize form controls
    this.userForm = new FormGroup({
      userId: new FormControl<string>(this.currentUser.userId),
      firstName: new FormControl<string>(this.currentUser.firstName, [Validators.required]),
      lastName: new FormControl<string>(this.currentUser.lastName, [Validators.required]),
      email: new FormControl<string>(this.currentUser.email, [Validators.required, Validators.email]),
      role: new FormControl<string>(this.currentUser.role ?? ''),
      isActive: new FormControl<boolean>(this.currentUser.isActive),
      saNaNumber: new FormControl<string | undefined>(this.currentUser.saNaNumber),
      dateOfBirth: new FormControl<string>(userDateOfBirth, [Validators.required]),
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

  // Getter function to easily access form controls
  get f() {
    return this.userForm.controls;
  }

  // Function called when form is submitted
  onSubmit(): void {
    // Check if the form is invalid
    if (this.userForm.invalid) {
      return;
    }

    // If it's an update operation
    if (this.isUpdate) {
      // Extract user data from the form
      const userWithAddress: UserWithAddress = this.userForm.value as UserWithAddress;
      // Convert date of birth to a Date object
      userWithAddress.dateOfBirth = new Date(userWithAddress.dateOfBirth);
      // Call method to update user with address
      this.updateUserWithAddress(userWithAddress);
    } else {
      // Extract registration data from the form
      const registration: Registration = this.userForm.value as Registration;
      // Convert date of birth to a Date object
      registration.dateOfBirth = new Date(registration.dateOfBirth);
      // Call method to register new user
      this.registerNewUser(registration);
    }
  }

  // Method to update user with address
  updateUserWithAddress(userWithAddress: UserWithAddress): void {
    // Call the updateUserWithAddress method from the userService, which returns an Observable
    this._userService.updateUserWithAddress(userWithAddress).subscribe({
      next: ((response: UserWithAddress): void => {
        // Check if response is valid
        if (response) {
          // Show success message using Toastr service
          this._toastr.success(`User ${response.firstName} erfolgreich geupdated`, 'Update User');
          // Close the dialog with a success flag set to true
          this.onClose(true);
        }
      }),
      // Handle error response from the API
      error: error => {
        // Show error message using Toastr service
        this._toastr.error(error.error ?? 'User konnte nicht geupdated werden', 'Update User');
      }
    });
  }

  // Method to register a new user
  registerNewUser(registration: Registration): void {
    // Call the register method from the authenticationService, which returns an Observable
    this._authenticationService.register(registration).subscribe({
      // Handle successful response from the API
      next: ((response: {isSuccessful: boolean, errors: string[]}): void => {
        // Check if registration was successful
        if (response.isSuccessful) {
          // Show success message using Toastr service
          this._toastr.success(`Neuer User ${registration.firstName} wurde erfolgreich erstellt`, 'Neuer User');
          // Close the dialog with a success flag set to true
          this.onClose(true);
        } else {
          // Show error message with the list of errors using Toastr service
          this._toastr.error(response.errors.join(), 'Neuer User');
        }
      }),
      // Handle error response from the API
      error: error => {
        // Show error message using Toastr service
        this._toastr.error(error.error ?? 'Neuer User konnte nicht ertsellt werden', 'Neuer User');
      }
    });
  }

  // Method to close the dialog and optionally reload data
  onClose(reload: boolean): void {
    // Close the dialog and pass the result object
    this._dialogRef.close(reload);
  }

  // Method to open the image upload dialog
  unUploadImage(): void {
    // Open the image upload dialog with necessary parameters
    const dialogRef = this._dialog.open(UserImageUploadComponent, {
      width: '60%',
      height: 'auto',
      data: {userId: this.currentUser.userId, imageUrl: this.currentUser.imageUrl}
    });

    // Subscribe to the afterClosed event of the dialog
    dialogRef.afterClosed().subscribe({
      // Handle the response from the image upload dialog
      next: ((reload: boolean): void => {
        // Check if reload flag is set
        if (reload) {
          // If reload flag is true, call onClose method with reload flag
          this.onClose(reload);
        }
      })
    });
  }
}
