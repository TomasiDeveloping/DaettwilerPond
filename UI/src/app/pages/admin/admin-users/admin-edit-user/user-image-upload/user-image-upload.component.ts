import {Component, Inject, inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {UserService} from "../../../../../services/user.service";
import {HttpEventType} from "@angular/common/http";
import {FormControl, FormGroup} from "@angular/forms";
import {ImageUrlPipe} from "../../../../../pipes/image-url.pipe";

@Component({
  selector: 'app-user-image-upload',
  templateUrl: './user-image-upload.component.html',
  styleUrl: './user-image-upload.component.scss'
})
export class UserImageUploadComponent {
  progress: number = 0;
  message: any;
  currentUserId: string;
  imageURL: string = '';
  uploadForm: FormGroup;
  uploadSuccess: boolean = false;
  isUpdate: boolean = false;

  private readonly _dialogRef: MatDialogRef<UserImageUploadComponent> = inject(MatDialogRef<UserImageUploadComponent>);
  private readonly _userService: UserService = inject(UserService);
  private readonly _imageUrlPipe: ImageUrlPipe = inject(ImageUrlPipe);

  constructor(@Inject(MAT_DIALOG_DATA) public data: {userId: string, imageUrl: string}) {
    this.currentUserId = data.userId;

    this.uploadForm = new FormGroup({
      userImage: new FormControl<File | null>(null)
    });

    if (data.imageUrl) {
      this.imageURL = this._imageUrlPipe.transform(data.imageUrl);
      this.isUpdate = true;
    }
  }

  showPreview(event: any) {
    if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];

      this.uploadForm.patchValue({
        userImage: file
      });

      const reader = new FileReader();
      reader.onload = e => this.imageURL = reader.result as string;

      reader.readAsDataURL(file);
    }
  }

  uploadFile() {
    const fileToUpload = this.uploadForm.get('userImage')?.value;
    if (!fileToUpload || fileToUpload.length === 0) {
      return;
    }

    const formData = new FormData();
    formData.append('File', fileToUpload);
    formData.append('UserId', this.currentUserId);

    this._userService.uploadUserProfile(formData).subscribe({
      next: (event) =>{
        if (event.type === HttpEventType.UploadProgress) {
          this.progress = Math.round(100 * event.loaded / event.total!);
        } else if (event.type === HttpEventType.Response) {
          this.message = 'Hochladen erfolgreich';
          this.uploadSuccess = true;
        }
      },
      error: (error) => {
        console.log(error);
      }
    });
  }

  onClose(reload: boolean) {
    this._dialogRef.close(reload);
  }


}
