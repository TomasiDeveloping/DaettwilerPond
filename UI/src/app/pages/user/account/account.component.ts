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
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {
  public currentUser: User | undefined;
  public currentUserAddress: Address | undefined;
  public userForm!: FormGroup;
  public addressForm!: FormGroup;
  public isEditUser: boolean = false;
  public isEditAddress: boolean = false;
  public licenses: FishingLicense[] = [];
  public pageSettings = {pageSizes: true, pageSize: 5};
  public toolbarOptions: ToolbarItems[] = ['Search'];

  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);
  private readonly _userService: UserService = inject(UserService);
  private readonly _addressService: AddressService = inject(AddressService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _fishingLicenseService: FishingLicenseService = inject(FishingLicenseService);

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
    const currentUserId = this._authenticationService.getUserIdFromToken();
    if (!currentUserId) {
      this._toastr.error('Fehler beim laden', 'Fehler');
      return;
    }
    this.getUser(currentUserId);
    this.getFishingLicenses(currentUserId);
  }

  getUser(userId: string) {
    this._userService.getUserById(userId).subscribe({
      next: ((response) => {
        if (response) {
          this.currentUser = response;
          this.createUserForm(response, userId);
          this.getUserAddress(userId);
        }
      })
    });
  }

  getFishingLicenses(userId: string) {
    this._fishingLicenseService.getUserLicenses(userId).subscribe({
      next: ((response) => {
        if (response) {
          this.licenses = response;
        }
      })
    });
  }

  getUserAddress(userId: string) {
    this._addressService.getUserAddresses(userId).subscribe({
      next: ((response) => {
        if (response) {
          this.currentUserAddress = response[0];
          this.createAddressForm(response[0]);
        }
      })
    });
  }

  createUserForm(user: User, userId: string) {
    this.userForm = new FormGroup({
      id: new FormControl<string>(userId),
      firstName: new FormControl<string>(user.firstName, [Validators.required]),
      lastName: new FormControl<string>(user.lastName, [Validators.required]),
      email: new FormControl<string>(user.email, [Validators.required, Validators.email])
    });
    this.userForm.disable();
  }

  createAddressForm(address: Address) {
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
    this.addressForm.disable();
  }

  onEditUser() {
    this.isEditUser = true;
    this.userForm.enable();
  }

  onSubmitUser() {
    if (this.userForm.invalid) {
      return;
    }
    const user: User = this.userForm.value as User;
    this._userService.updateUser(user.id, user).subscribe({
      next: ((response) => {
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

  onCancelEditUser() {
    this.isEditUser = false;
    this.userForm.patchValue({
      firstName: this.currentUser?.firstName,
      lastName: this.currentUser?.lastName,
      email: this.currentUser?.email
    });
    this.userForm.disable();
  }

  onSubmitAddress() {
    if (this.addressForm.invalid) {
      return;
    }
    const address: Address = this.addressForm.value;
    this._addressService.updateAddress(address.id, address).subscribe({
      next: ((response) => {
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

  onEditAddress() {
    this.isEditAddress = true;
    this.addressForm.enable();
  }

  onCancelEditAddress() {
    this.isEditAddress = false;
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
    this.addressForm.disable();
  }
}
