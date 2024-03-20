import {AfterViewInit, Component, inject, OnDestroy, ViewChild} from '@angular/core';
import {MatDialogRef} from "@angular/material/dialog";
import {ZXingScannerComponent} from "@zxing/ngx-scanner";
import {Subscription} from "rxjs";
import { BarcodeFormat } from '@zxing/library';

@Component({
  selector: 'app-overseer-validate-license',
  templateUrl: './overseer-validate-license.component.html',
  styleUrl: './overseer-validate-license.component.scss'
})
export class OverseerValidateLicenseComponent implements AfterViewInit, OnDestroy{

  // Flags and variables initialization
  public hasScanError: boolean = false;
  public isLoading: boolean = true;
  public cameras: MediaDeviceInfo[] = [];
  public errorMessage: string = '';
  public selectedCamera: string = '';
  public allowedFormats = [BarcodeFormat.QR_CODE]

  // Subscriptions for managing scanner events
  private permissionResponseSubscription: Subscription | undefined;
  private cameraFoundSubscription: Subscription | undefined;
  private hasDeviceSubscription: Subscription | undefined;

  // Injecting MatDialogRef for dialog control
  private readonly _dialogRef: MatDialogRef<OverseerValidateLicenseComponent> = inject(MatDialogRef<OverseerValidateLicenseComponent>);

  // Accessing ZXingScannerComponent view child
  @ViewChild('scanner', {static: false}) scanner!: ZXingScannerComponent;

  ngAfterViewInit(): void {
    // Initiating event handlers
    this.onPermissionResponse();
    this.onCamerasFound();
    this.onHasDevices();
  }

  // Close dialog and pass licenseId if available
  onClose(licenseId: string | null): void {
    this._dialogRef.close(licenseId);
  }

  // Cancel scanning process
  onCancel(): void {
    this.scanner.enable = false;
    this.onClose(null);
  }

  // Handle successful scan
  onScanSuccess(event: string) {
    const scannedValue: string = event;

    // Disable scanner after successful scan
    this.scanner.enable = false;

    if (!scannedValue) {
      // Handle empty scan
      this.hasScanError = true;
      this.errorMessage = 'Keine Daten im QR Code gefunden';
      return;
    }

    // Extracting licenseId from scanned value
    const splitUrl: string[] = scannedValue.split('/');
    const licenseId: string = splitUrl[splitUrl.length -1];

    if (!licenseId) {
      // Handle invalid scanned data
      this.hasScanError = true;
      this.errorMessage = 'Keine Lizenznummer gefunden';
      return;
    }

    // Closing dialog and passing licenseId
    this.onClose(licenseId);
  }

  // Handle camera change event
  onCameraChange(event: any): void {
    const deviceId = event.target.value;
    const device: MediaDeviceInfo | undefined = this.cameras.find((deviceInfo: MediaDeviceInfo): boolean => deviceInfo.deviceId === deviceId);
    if (device === undefined || !device) {
      return;
    }
    // Set scanner device to the selected camera
    this.scanner.device = device;
    this.selectedCamera = device.deviceId;
  }

  // Subscription to handle permission response
  private onPermissionResponse(): void {
    this.permissionResponseSubscription = this.scanner.permissionResponse.subscribe({
      next: ((permission: boolean): void => {
        if (permission) {
          // Handle permission granted
          this.isLoading = false;
        } else {
          // Handle permission denied
          this.isLoading = false;
          this.hasScanError = true;
          this.errorMessage = 'Keine Kamera berechtigung';
        }
      }),
      // Handle error in permission response
      error: (_: any): void=> {
        this.isLoading = false;
        this.hasScanError = true;
        this.errorMessage = 'Fehler bei Kamera berechtigung';
      }
    })
  }

  // Subscription to handle camera detection
  private onCamerasFound(): void {
    this.cameraFoundSubscription = this.scanner.camerasFound.subscribe({
      next: ((cameras: MediaDeviceInfo[]): void => {
        if (cameras.length === 1) {
          // If only one camera found, set it as default
          this.scanner.device = cameras[0];
        } else {
          // If multiple cameras found, populate cameras list and set first as default
          this.cameras = cameras;
          this.scanner.device = cameras[0];
          this.selectedCamera = cameras[0].deviceId;
        }
      }),
      error: (_: any): void => {
        // Handle error in camera detection
        this.hasScanError = true;
        this.errorMessage = 'Fehler beim Kamera suchen';
      }
    })
  }

  // Subscription to handle device availability
  private onHasDevices(): void {
    this.hasDeviceSubscription = this.scanner.hasDevices.subscribe({
      next: ((device: boolean): void => {
        if (!device) {
          // Handle no camera available
          this.hasScanError = true;
          this.errorMessage = 'Keine Kamera gefunden';
        }
      }),
      error: (_: any): void => {
        // Handle error in device availability check
        this.hasScanError = true;
        this.errorMessage = 'Keine Kamera gefunden';
      }
    });
  }

  // Lifecycle hook - OnDestroy
  ngOnDestroy(): void {
    // Unsubscribe from subscriptions to prevent memory leaks
    if (this.permissionResponseSubscription) {
      this.permissionResponseSubscription.unsubscribe();
    }

    if (this.cameraFoundSubscription) {
      this.cameraFoundSubscription.unsubscribe();
    }

    if (this.hasDeviceSubscription) {
      this.hasDeviceSubscription = undefined;
    }
  }
}
