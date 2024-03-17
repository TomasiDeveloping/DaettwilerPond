import {AfterViewInit, Component, inject, ViewChild} from '@angular/core';
import {NgxScannerQrcodeComponent, ScannerQRCodeResult} from "ngx-scanner-qrcode";
import {MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-overseer-validate-license',
  templateUrl: './overseer-validate-license.component.html',
  styleUrl: './overseer-validate-license.component.scss'
})
export class OverseerValidateLicenseComponent implements AfterViewInit{

  // Flag to indicate if there's a scan error
  public hasScanError: boolean = false;

  // Reference to the dialog
  private readonly _dialogRef: MatDialogRef<OverseerValidateLicenseComponent> = inject(MatDialogRef<OverseerValidateLicenseComponent>);

  // Reference to the QR code scanner component
  @ViewChild('action') action!: NgxScannerQrcodeComponent;

  ngAfterViewInit(): void {
    // Subscribe to the readiness event of the scanner component
    this.action.isReady.subscribe((): void => {
      // Call the handle function when the scanner is ready
      this.handle(this.action, 'start');
    });
  }

  // Function to handle QR code scanner events
  onEvent(event: ScannerQRCodeResult[], action: NgxScannerQrcodeComponent): void {
    // Get the scanned value from the event
    const scannedValue: string = event[0].value;

    // Stop the scanner
    action.stop();

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

  // Function to handle scanner initialization
  private handle(action: NgxScannerQrcodeComponent, _: string): void {
    const playDeviceFacingBack = (devices: any[]): void => {
      // front camera or back camera check here!
      const device = devices.find(f => (/back|rear|environment/gi.test(f.label)));
      action.playDevice(device ? device.deviceId : devices[0].deviceId);
    };
    // Start the scanner
    action.start();
  }

  // Function to close the dialog
  onClose(licenseId: string | null): void {
    this._dialogRef.close(licenseId);
  }

  // Function to handle cancellation
  onCancel(): void {
    // Stop the scanner and close the dialog with null
    this.action.stop();
    this.onClose(null);
  }
}
