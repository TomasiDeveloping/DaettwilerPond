import {Component, inject, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-admin-send-email',
  templateUrl: './admin-send-email.component.html',
  styleUrl: './admin-send-email.component.scss'
})
export class AdminSendEmailComponent {

  memberEmails: {email: string, fullName: string}[] = [];
  emailSendForm: FormGroup;
  private readonly _dialogRef: MatDialogRef<AdminSendEmailComponent> = inject(MatDialogRef<AdminSendEmailComponent>);
constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
  this.memberEmails = data.memberEmails;
  this.emailSendForm = new FormGroup({
    emails: new FormControl<string[]>([], [Validators.required]),
    subject: new FormControl<string>('', [Validators.required]),
    content: new FormControl<string>('', [Validators.required])
  });
}

get emails() {
  return this.emailSendForm.get('emails');
}

  onClose() {
  this._dialogRef.close();
  }

  onSubmit() {

  }
}
