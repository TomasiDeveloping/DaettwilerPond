import {Component, Inject, inject} from '@angular/core';
import {FormControl, FormGroup} from "@angular/forms";
import {FishingLicense} from "../../../../models/fishingLicense.model";
import {FishingLicenseService} from "../../../../services/fishing-license.service";
import {ToastrService} from "ngx-toastr";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {User} from "../../../../models/user.model";
import {UserService} from "../../../../services/user.service";

@Component({
  selector: 'app-admin-edit-fishing-license',
  templateUrl: './admin-edit-fishing-license.component.html',
  styleUrls: ['./admin-edit-fishing-license.component.scss']
})
export class AdminEditFishingLicenseComponent {
  public isUpdate: boolean;
  public licenseForm: FormGroup;
  public users: User[] = [];

  private currentLicense: FishingLicense;

  private readonly _fishingLicenseService: FishingLicenseService = inject(FishingLicenseService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _dialogRef: MatDialogRef<AdminEditFishingLicenseComponent> = inject(MatDialogRef<AdminEditFishingLicenseComponent>);
  private readonly _userService: UserService = inject(UserService);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { isUpdate: boolean, license: FishingLicense }) {
    this.isUpdate = data.isUpdate;
    this.currentLicense = data.license;
    this.getUsers();
    const expireDate = new Date(this.currentLicense.expiresOn);
    this.licenseForm = new FormGroup({
      id: new FormControl<string>(this.currentLicense.id),
      userFullName: new FormControl<string>(this.currentLicense.userFullName),
      userId: new FormControl<string>(this.currentLicense.userId),
      createdAt: new FormControl<Date>(this.currentLicense.createdAt),
      updatedAt: new FormControl<Date>(this.currentLicense.updatedAt),
      year: new FormControl<number>(this.currentLicense.year),
      isPaid: new FormControl<boolean>(this.currentLicense.isPaid),
      issuedBy: new FormControl<string>(this.currentLicense.issuedBy),
      isActive: new FormControl<boolean>(this.currentLicense.isActive),
      expiresOn: new FormControl<string>(new Date(
        Date.UTC(expireDate.getFullYear(),
          expireDate.getMonth(),
          expireDate.getDate()))
        .toISOString().substring(0, 10)
      )
    });
  }

  getUsers() {
    this._userService.getUsers().subscribe({
      next: ((response) => {
        if (response) {
          this.users = response;
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Fehler beim Abrufen der Mitglieder', 'Fehler Users');
      }
    });
  }

  onSubmit() {
    if (this.licenseForm.invalid) {
      return;
    }
    const fishingLicence: FishingLicense = this.licenseForm.value;
    this.isUpdate ? this.updateFishingLicense(fishingLicence.id, fishingLicence) : this.createFishingLicense(fishingLicence);
  }

  updateFishingLicense(fishingLicenceId: string, fishingLicence: FishingLicense) {
    this._fishingLicenseService.updateFishingLicense(fishingLicenceId, fishingLicence).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Lizenz wurde erfolgreich geändert', 'Update Lizenz');
          this.onClose(true);
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Lizenz konnte nicht geändert werden', 'Update Lizenz');
      }
    });
  }

  createFishingLicense(fishingLicense: FishingLicense) {
    this._fishingLicenseService.createFishingLicence(fishingLicense).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Lizenz wurde erfolgreich erstellt', 'Neue Lizenz');
          this.onClose(true);
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Lizez konnte nicht erstellt werden', 'Neue Lizenz');
      }
    });
  }

  onClose(reload: boolean) {
    const response = {reload: reload};
    this._dialogRef.close(response);
  }
}
