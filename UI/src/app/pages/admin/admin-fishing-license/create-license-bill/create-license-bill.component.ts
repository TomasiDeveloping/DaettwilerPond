import {Component, inject, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FishingLicenseCreateBill} from "../../../../models/fishingLicenseCreateBill.model";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {UserService} from "../../../../services/user.service";
import {ToastrService} from "ngx-toastr";
import {PdfService} from "../../../../services/pdf.service";

@Component({
    selector: 'app-create-license-bill',
    templateUrl: './create-license-bill.component.html',
    styleUrls: ['./create-license-bill.component.scss'],
    standalone: false
})
export class CreateLicenseBillComponent implements OnInit {

  public createBillForm: FormGroup;
  public users: { userId: string, name: string }[] = [];

  private readonly _userService: UserService = inject(UserService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _dialogRef: MatDialogRef<CreateLicenseBillComponent> = inject(MatDialogRef<CreateLicenseBillComponent>);
  private readonly _pdfService: PdfService = inject(PdfService);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { licenseCreateBill: FishingLicenseCreateBill }) {
    const licenceCreateBill: FishingLicenseCreateBill = this.data.licenseCreateBill;
    this.createBillForm = new FormGroup({
      userIds: new FormControl<string[]>(licenceCreateBill.userIds, [Validators.required]),
      licenseYear: new FormControl<number | undefined>(licenceCreateBill.licenseYear, [Validators.required]),
      emailMessage: new FormControl<string>(licenceCreateBill.emailMessage, [Validators.required]),
      createLicense: new FormControl<boolean>(licenceCreateBill.createLicense)
    });
  }

  get userIds() {
    return this.createBillForm.get('userIds');
  }

  get licenseYear() {
    return this.createBillForm.get('licenseYear');
  }

  get emailMessage() {
    return this.createBillForm.get('emailMessage');
  }

  ngOnInit() {
    this._userService.getUsers().subscribe({
      next: ((response) => {
        if (response) {
          response.forEach((user) => {
            if (user.firstName !== 'System') {
              this.users.push({name: user.firstName + ' ' + user.lastName, userId: user.id});
            }
          })
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Fehler beim abrufen der Mitglieder', 'Mitglieder');
      }
    });
  }

  onSubmit() {
    if (this.createBillForm.invalid) {
      return;
    }
    const createFishingLicenseBill: FishingLicenseCreateBill = this.createBillForm.value as FishingLicenseCreateBill;
    this._pdfService.sendFishingLicenseBill(createFishingLicenseBill).subscribe({
      next: ((response) => {
        if (response) {
          this.onClose(true);
          this._toastr.success('Emails mit PDF Rechnungen wurden erfolgreich versendet', 'Lizenz Rechnung');
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Emails mit Rechnungen konnten nicht versendet werden', 'Lizenz Rechnung');
      }
    });
  }

  onClose(reload: boolean) {
    const response: { reload: boolean } = {reload: reload}
    this._dialogRef.close(response);
  }
}
