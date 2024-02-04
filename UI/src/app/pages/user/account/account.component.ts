import {Component, inject, OnInit} from '@angular/core';
import {AuthenticationService} from "../../../services/authentication.service";
import {UserService} from "../../../services/user.service";
import {User} from "../../../models/user.model";
import {ToastrService} from "ngx-toastr";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Address} from "../../../models/address.model";
import {AddressService} from "../../../services/address.service";
import {ToolbarItems} from "@syncfusion/ej2-angular-grids";
import {FishingLicense} from "../../../models/fishingLicense.model";
import {FishingLicenseService} from "../../../services/fishing-license.service";

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html'
})
export class AccountComponent implements OnInit {

  // Public variables
  public currentUser: User | undefined;
  public currentUserAddress: Address | undefined;
  public userForm!: FormGroup;
  public addressForm!: FormGroup;
  public isEditUser: boolean = false;
  public isEditAddress: boolean = false;
  public licenses: FishingLicense[] = [];
  public pageSettings = {pageSizes: true, pageSize: 5};
  public toolbarOptions: ToolbarItems[] = ['Search'];

  // Private services
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);
  private readonly _userService: UserService = inject(UserService);
  private readonly _addressService: AddressService = inject(AddressService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _fishingLicenseService: FishingLicenseService = inject(FishingLicenseService);

  // Getters for form controls
  get firstName() {
    return this.userForm.get('firstName');
  }

  get lastName() {
    return this.userForm.get('lastName');
  }

  get email() {
    return this.userForm.get('email');
  }

  get street() {
    return this.addressForm.get('street');
  }

  get city() {
    return this.addressForm.get('city');
  }

  get houseNumber() {
    return this.addressForm.get('houseNumber');
  }

  get postalCode() {
    return this.addressForm.get('postalCode');
  }

  ngOnInit(): void {
    // Retrieve the current user's ID from the authentication service
    const currentUserId: string | null = this._authenticationService.getUserIdFromToken();

    // Check if user ID exists
    if (!currentUserId) {
      this._toastr.error('Fehler beim laden', 'Fehler');
      return;
    }

    // Fetch user details, address, and fishing licenses
    this.getUser(currentUserId);
    this.getFishingLicenses(currentUserId);
  }

  // Fetch user details by ID
  getUser(userId: string): void {
    this._userService.getUserById(userId).subscribe({
      next: ((response: User): void => {
        if (response) {
          this.currentUser = response;

          // Create and initialize user form
          this.createUserForm(response, userId);

          // Fetch user's address
          this.getUserAddress(userId);
        }
      })
    });
  }

  // Fetch user's fishing licenses
  getFishingLicenses(userId: string): void {
    this._fishingLicenseService.getUserLicenses(userId).subscribe({
      next: ((response: FishingLicense[]): void => {
        if (response) {
          this.licenses = response;
        }
      })
    });
  }

  // Fetch user's address
  getUserAddress(userId: string): void {
    this._addressService.getUserAddresses(userId).subscribe({
      next: ((response: Address[]): void => {
        if (response) {
          this.currentUserAddress = response[0];

          // Create and initialize address form
          this.createAddressForm(response[0]);
        }
      })
    });
  }

  // Create user form with initial values
  createUserForm(user: User, userId: string): void {
    this.userForm = new FormGroup({
      id: new FormControl<string>(userId),
      firstName: new FormControl<string>(user.firstName, [Validators.required]),
      lastName: new FormControl<string>(user.lastName, [Validators.required]),
      email: new FormControl<string>(user.email, [Validators.required, Validators.email])
    });

    // Disable the user form initially
    this.userForm.disable();
  }

  // Create address form with initial values
  createAddressForm(address: Address): void {
    this.addressForm = new FormGroup({
      id: new FormControl<string>(address.id),
      userId: new FormControl<string>(address.userId),
      street: new FormControl<string>(address.street, [Validators.required]),
      houseNumber: new FormControl<string>(address.houseNumber, [Validators.required]),
      city: new FormControl<string>(address.city, [Validators.required]),
      postalCode: new FormControl<string>(address.postalCode, [Validators.required]),
      country: new FormControl<string>(address.country),
      phone: new FormControl<string>(address.phone),
      mobile: new FormControl<string>(address.mobile)
    });

    // Disable the address form initially
    this.addressForm.disable();
  }

  // Enable user form for editing
  onEditUser() {
    this.isEditUser = true;
    this.userForm.enable();
  }

  // Submit user form data
  onSubmitUser() {
    if (this.userForm.invalid) {
      return;
    }

    // Extract user data from the form
    const user: User = this.userForm.value as User;

    // Update user data via the user service
    this._userService.updateUser(user.id, user).subscribe({
      next: ((response: User): void => {
        if (response) {
          this.currentUser = response;
          this._toastr.success('Benutzer erfolgreich geupdated', 'Update Benutzer');
          this.onCancelEditUser();
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Benutzer konnte nicht geupdated werden', 'Update Benutzer');
      }
    });
  }

  // Cancel user editing and reset form
  onCancelEditUser(): void {
    this.isEditUser = false;

    // Reset user form with current user's data
    this.userForm.patchValue({
      firstName: this.currentUser?.firstName,
      lastName: this.currentUser?.lastName,
      email: this.currentUser?.email
    });

    // Disable the user form
    this.userForm.disable();
  }

  // Submit address form data
  onSubmitAddress(): void {
    if (this.addressForm.invalid) {
      return;
    }

    // Extract address data from the form
    const address: Address = this.addressForm.value;

    // Update address data via the address service
    this._addressService.updateAddress(address.id, address).subscribe({
      next: ((response: Address): void => {
        if (response) {
          this.currentUserAddress = response;
          this._toastr.success('Adresse erfolgreich geupdatet', 'Adresse');
          this.onCancelEditAddress();
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Adresse konnte nicht geupdated werden', 'Adresse');
      }
    });
  }

  // Enable address form for editing
  onEditAddress(): void {
    this.isEditAddress = true;
    this.addressForm.enable();
  }

  // Cancel address editing and reset form
  onCancelEditAddress(): void {
    this.isEditAddress = false;

    // Reset address form with current user's address data
    this.addressForm.patchValue({
      id: this.currentUserAddress?.id,
      userId: this.currentUserAddress?.userId,
      street: this.currentUserAddress?.street,
      houseNumber: this.currentUserAddress?.houseNumber,
      city: this.currentUserAddress?.city,
      postalCode: this.currentUserAddress?.postalCode,
      country: this.currentUserAddress?.country,
      phone: this.currentUserAddress?.phone,
      mobile: this.currentUserAddress?.mobile
    });

    // Disable the address form
    this.addressForm.disable();
  }
}
