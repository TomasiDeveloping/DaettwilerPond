import {Component, inject, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {PdfService} from "../../../../services/pdf.service";
import {MembersEmailModel} from "../../../../models/membersEmail.model";
import {ToastrService} from "ngx-toastr";

@Component({
    selector: 'app-admin-send-email',
    templateUrl: './admin-send-email.component.html',
    styleUrl: './admin-send-email.component.scss',
    standalone: false
})
export class AdminSendEmailComponent {

  files: any[] = [];
  memberEmails: { email: string, fullName: string }[] = [];
  emailSendForm: FormGroup;
  private readonly _dialogRef: MatDialogRef<AdminSendEmailComponent> = inject(MatDialogRef<AdminSendEmailComponent>);
  private readonly _pdfService: PdfService = inject(PdfService);
  private readonly _toastr: ToastrService = inject(ToastrService);

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

  get content() {
    return this.emailSendForm.get('content');
  }

  get subject() {
    return this.emailSendForm.get('subject');
  }

  onClose(): void {
    this._dialogRef.close();
  }

  onSubmit(): void {
    const memberEmail: MembersEmailModel = {
      subject: this.subject?.value,
      mailContent: this.content?.value,
      receiverAddresses: this.emails?.value,
      attachments: this.files,
    }
    this._pdfService.sendMemberEmails(memberEmail).subscribe({
      next: ((response: boolean): void => {
        if (response) {
          this._toastr.success('E-Mail(s) erforgreich versendet', 'Email Service');
          this.onClose();
        } else {
          this._toastr.error('E-Mail(s) konnten nicht versendet werden', 'Email Service');
        }
      }),
      error: (): void => {
        this._toastr.error('E-Mail(s) konnten nicht versendet werden', 'Email Service');
      }
    });
  }

  fileBrowseHandler(event: any): void {
    this.prepareFilesList(event.target.files);
  }

  prepareFilesList(files: Array<any>): void {
    for (const item of files) {
      item.progress = 0;
      this.files.push(item);
    }
  }

  formatBytes(bytes: any, decimals: number = 2): string {
    if (bytes === 0) {
      return "0 Bytes";
    }
    const k = 1024;
    const dm = decimals <= 0 ? 0 : decimals;
    const sizes = ["Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + " " + sizes[i];
  }

  deleteFile(index: number) {
    this.files.splice(index, 1);
  }
}
