import {AfterViewInit, Component, inject, ViewChild} from '@angular/core';
import {MatDialogRef} from "@angular/material/dialog";
import {ZXingScannerComponent} from "@zxing/ngx-scanner";

@Component({
  selector: 'app-overseer-validate-license',
  templateUrl: './overseer-validate-license.component.html',
  styleUrl: './overseer-validate-license.component.scss'
})
export class OverseerValidateLicenseComponent implements AfterViewInit{

  // Flag to indicate if there's a scan error
  public hasScanError: boolean = false;
  public isLoading: boolean = true;
  public cameras: MediaDeviceInfo[] = [];

  // Reference to the dialog
  private readonly _dialogRef: MatDialogRef<OverseerValidateLicenseComponent> = inject(MatDialogRef<OverseerValidateLicenseComponent>);

  // Reference to the QR code scanner component
  @ViewChild('scanner', {static: false}) scanner!: ZXingScannerComponent;

  ngAfterViewInit(): void {
    this.scanner.camerasFound.subscribe({
      next: ((cameras: MediaDeviceInfo[]) => {
        if (cameras.length === 1) {
          this.cameras = cameras;
          this.scanner.device = cameras[0];
        } else {
          this.cameras = cameras;
          this.scanner.device = cameras[0];
        }
      }),
      error: (error: any) => {
        console.log(error)
      }
    })

    this.scanner.hasDevices.subscribe({
      next: ((device: boolean) => {
        console.log('HasDevice', device)
      }),
      error: (error: any) => {
        console.log(error);
      }
    });

    this.scanner.permissionResponse.subscribe({
      next: ((permission: boolean) => {
        if (permission) {
          this.isLoading = false;
          console.log('Has Permission')
        } else {
          console.log('No Permission')
        }
      }),
      error: (error: any)=> {
        console.log(error);
      }
    })
  }

  // Function to close the dialog
  onClose(licenseId: string | null): void {
    this._dialogRef.close(licenseId);
  }

  // Function to handle cancellation
  onCancel(): void {
    // Stop the scanner and close the dialog with null
    this.scanner.enable = false;
    this.onClose(null);
  }

  onScanSuccess(event: string) {
    // Get the scanned value from the event
    const scannedValue: string = event;

    // Stop the scanner
    this.scanner.enable = false;

    // Check if there's a scanned value
    if (!scannedValue) {
      // Set the scan error flag and return
      this.hasScanError = true;
      return;
    }

    // Split the scanned URL to extract the license ID
    const splitUrl: string[] = scannedValue.split('/');
    const licenseId: string = splitUrl[splitUrl.length -1];
    // Check if a valid license ID is obtained
    if (!licenseId) {
      // Set the scan error flag and return
      this.hasScanError = true;
      return;
    }

    // Close the dialog and pass the license ID
    this.onClose(licenseId);
  }

  onScanFailure(event: any) {

  }

  onCameraChange(event: any) {
    const deviceId = event.target.value;
    const device: MediaDeviceInfo | undefined = this.cameras.find(d => d.deviceId === deviceId);
    if (device === undefined) {
      return;
    }
    this.scanner.device = device;
  }
}
