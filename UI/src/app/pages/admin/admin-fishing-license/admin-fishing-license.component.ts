import {Component, inject, OnInit} from '@angular/core';
import {FishingLicense} from "../../../models/fishingLicense.model";
import {FishingLicenseService} from "../../../services/fishing-license.service";
import {ToastrService} from "ngx-toastr";
import {MatDialog} from "@angular/material/dialog";
import {AdminEditFishingLicenseComponent} from "./admin-edit-fishing-license/admin-edit-fishing-license.component";
import Swal from "sweetalert2";
import {FishingLicenseCreateBill} from "../../../models/fishingLicenseCreateBill.model";
import {CreateLicenseBillComponent} from "./create-license-bill/create-license-bill.component";

@Component({
  selector: 'app-admin-fishing-license',
  templateUrl: './admin-fishing-license.component.html'
})
export class AdminFishingLicenseComponent implements OnInit {
  public licenses: FishingLicense[] = [];
  public pageSettings: { pageSizes: boolean, pageSize: number } = {pageSizes: true, pageSize: 10};

  private readonly _fishingLicenseService: FishingLicenseService = inject(FishingLicenseService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _dialog: MatDialog = inject(MatDialog);


  ngOnInit() {
    this.getFishingLicenses();
  }

  getFishingLicenses() {
    this._fishingLicenseService.getFishingLicenses().subscribe({
      next: ((response) => {
        if (response) {
          this.licenses = response;
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Fehler beim Abrufen der Lizenzen', 'Lizenzen');
      }
    });
  }

  onAddLicense() {
    const licence: FishingLicense = {
      isPaid: false,
      isActive: false,
      expiresOn: new Date(new Date().getFullYear(), 11, 31),
      year: new Date().getFullYear(),
      userId: '',
      id: '',
      issuedBy: '',
      updatedAt: new Date(),
      createdAt: new Date(),
      userFullName: ''
    };
    this.openFishingLicenseDialog(false, licence);
  }

  onEdit(event: any) {
    const license = event.rowData as FishingLicense;
    this.openFishingLicenseDialog(true, license);
  }

  onDeleteLicense(license: FishingLicense) {
    Swal.fire({
      title: 'Bist Du sicher?',
      text: `Lizenz wirklisch löschen?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Ja, bitte löschen!',
      cancelButtonText: 'Abbrechen'
    }).then((result) => {
      if (result.isConfirmed) {
        this._fishingLicenseService.deleteFishingLicence(license.id).subscribe({
          next: ((response) => {
            if (response) {
              this._toastr.success(`Lizenz wurde erfolgreich gelöscht`, 'Lizenz Löschen');
              this.getFishingLicenses();
            }
          }),
          error: error => {
            this._toastr.error(error.error ?? 'Lizenz konnte nicht gelöscht werden', 'Lizenz Löschen');
          }
        });
      }
    });
  }

  openFishingLicenseDialog(isUpdate: boolean, fishingLicence: FishingLicense) {
    const dialogRef = this._dialog.open(AdminEditFishingLicenseComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      disableClose: true,
      data: {isUpdate: isUpdate, license: fishingLicence}
    });
    dialogRef.afterClosed().subscribe((result: { reload: boolean }) => {
      if (result.reload) {
        this.getFishingLicenses();
      }
    });
  }

  onCreateFishingLicenseBillEmail() {
    const fishingLicenceCreateBill: FishingLicenseCreateBill = {
      userIds: [],
      emailMessage: '',
      licenseYear: undefined,
      createLicense: false
    };
    const dialogRef = this._dialog.open(CreateLicenseBillComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      disableClose: true,
      data: {licenseCreateBill: fishingLicenceCreateBill}
    });
    dialogRef.afterClosed().subscribe((response: { reload: boolean }) => {
      if (response.reload) {
        this.getFishingLicenses();
      }
    });
  }
}
