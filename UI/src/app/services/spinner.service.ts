import {inject, Injectable} from '@angular/core';
import {NgxSpinnerService} from "ngx-spinner";

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {

  // Counter to keep track of active spinner requests
  private busyRequestCount: number = 0;

  // Injecting ngx-spinner service
  private readonly _ngxSpinnerService: NgxSpinnerService = inject(NgxSpinnerService);

  // Method to activate the spinner
  busy(): void {
    this.busyRequestCount++;
    this._ngxSpinnerService.show(undefined, {
      type: 'square-loader',
      bdColor: 'rgba(0, 0, 0, 0.8)',
      color: '#87aff1'
    }).then();
  }

  // Method to deactivate the spinner
  idle(): void {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this._ngxSpinnerService.hide().then();
    }
  }
}
