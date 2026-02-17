import {Component, Inject, inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {UserService} from "../../../../../services/user.service";
import { HttpEvent, HttpEventType } from "@angular/common/http";
import {FormControl, FormGroup} from "@angular/forms";
import {ImageUrlPipe} from "../../../../../pipes/image-url.pipe";
import {ToastrService} from "ngx-toastr";

@Component({
    selector: 'app-user-image-upload',
    templateUrl: './user-image-upload.component.html',
    styleUrl: './user-image-upload.component.scss',
    standalone: false
})
export class UserImageUploadComponent {

  // Public properties
  public progress: number = 0;
  public message: any;
  public currentUserId: string;
  public imageURL: string = '';
  public uploadForm: FormGroup;
  public uploadSuccess: boolean = false;
  public isUpdate: boolean = false;

  // Dependency injection
  private readonly _dialogRef: MatDialogRef<UserImageUploadComponent> = inject(MatDialogRef<UserImageUploadComponent>);
  private readonly _userService: UserService = inject(UserService);
  private readonly _imageUrlPipe: ImageUrlPipe = inject(ImageUrlPipe);
  private readonly _toastr: ToastrService = inject(ToastrService);

  constructor(@Inject(MAT_DIALOG_DATA) public data: {userId: string, imageUrl: string}) {
    // Initialize public properties
    this.currentUserId = data.userId;

    // Initialize upload form
    this.uploadForm = new FormGroup({
      userImage: new FormControl<File | null>(null)
    });

    // Check if there is an image URL provided
    if (data.imageUrl) {
      // Transform image URL using the image URL pipe
      this.imageURL = this._imageUrlPipe.transform(data.imageUrl);
      this.isUpdate = true;
    }
  }

  // Method to show image preview
  showPreview(event: any): void {
    if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];

      // Patch the value of the form control with the selected file
      this.uploadForm.patchValue({
        userImage: file
      });

      // Read and set image URL for preview
      const reader: FileReader = new FileReader();
      reader.onload = () => this.imageURL = reader.result as string;
      reader.readAsDataURL(file);
    }
  }

  // Method to upload file
  uploadFile(): void {
    const fileToUpload = this.uploadForm.get('userImage')?.value;
    if (!fileToUpload || fileToUpload.length === 0) {
      return;
    }

    // Create FormData object to send file and user ID
    const formData: FormData = new FormData();
    formData.append('File', fileToUpload);
    formData.append('UserId', this.currentUserId);

    // Subscribe to the uploadUserProfile method from the userService
    this._userService.uploadUserProfile(formData).subscribe({
      // Handle different types of events during file upload
      next: (event: HttpEvent<Object>): void =>{
        if (event.type === HttpEventType.UploadProgress) {
          this.progress = Math.round(100 * event.loaded / event.total!);
        } else if (event.type === HttpEventType.Response) {
          this.message = 'Hochladen erfolgreich';
          this.uploadSuccess = true;
        }
      },
      // Handle error during file upload
      error: (): void => {
        this._toastr.error('Bild konnte nicht hochgeladen werden', 'Bild hochladen');
      }
    });
  }

  // Method to close the dialog
  onClose(reload: boolean): void {
    this._dialogRef.close(reload);
  }


}
