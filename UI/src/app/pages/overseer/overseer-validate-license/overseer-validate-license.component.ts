import {AfterViewInit, Component, inject, ViewChild} from '@angular/core';
import {NgxScannerQrcodeComponent, ScannerQRCodeResult} from "ngx-scanner-qrcode";
import {MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-overseer-validate-license',
  templateUrl: './overseer-validate-license.component.html',
  styleUrl: './overseer-validate-license.component.scss'
})
export class OverseerValidateLicenseComponent implements AfterViewInit{

  public hasScanError: boolean = false;
  private readonly _dialogRef: MatDialogRef<OverseerValidateLicenseComponent> = inject(MatDialogRef<OverseerValidateLicenseComponent>);

  @ViewChild('action') action!: NgxScannerQrcodeComponent;

  ngAfterViewInit(): void {
    this.action.isReady.subscribe((res: any) => {
      this.handle(this.action, 'start');
    });
  }
  onEvent(event: ScannerQRCodeResult[], action: NgxScannerQrcodeComponent) {
    const scannedValue: string = event[0].value;
    action.stop();
    if (!scannedValue) {
      this.hasScanError = true;
      return;
    }
    const splitUrl: string[] = scannedValue.split('/');
    const licenseId: string = splitUrl[splitUrl.length -1];

    if (!licenseId) {
      this.hasScanError = true;
      return;
    }

    this.onClose(licenseId);
  }

  private handle(action: NgxScannerQrcodeComponent, fn: string) {
    const playDeviceFacingBack = (devices: any[]) => {
      // front camera or back camera check here!
      const device = devices.find(f => (/back|rear|environment/gi.test(f.label))); // Default Back Facing Camera
      action.playDevice(device ? device.deviceId : devices[0].deviceId);
    };
    action.start();
  }

  onClose(licenseId: string | null) {
    this._dialogRef.close(licenseId);
  }

  onCancel() {
    this.action.stop();
    this.onClose(null);
  }
}
