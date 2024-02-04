import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ToastrModule} from "ngx-toastr";
import {HighchartsChartModule} from "highcharts-angular";
import {NgxScrollTopModule} from "ngx-scrolltop";
import {NgxSpinnerModule} from "ngx-spinner";
import {NgxMaskDirective, NgxMaskPipe, provideEnvironmentNgxMask} from "ngx-mask";


@NgModule({
  declarations: [],
  imports: [
    // CommonModule is imported to ensure common directives like *ngFor, *ngIf are available
    CommonModule,
    // Importing third-party UI components and services
    HighchartsChartModule,
    NgxScrollTopModule,
    NgxSpinnerModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    // Importing and configuring ngx-mask for input masking
    NgxMaskDirective,
    NgxMaskPipe
  ],
  // Exporting both Angular and third-party modules for use in other modules
  exports: [
    HighchartsChartModule,
    NgxScrollTopModule,
    NgxSpinnerModule,
    ToastrModule,
    NgxMaskDirective,
    NgxMaskPipe
  ],
  // Providing configuration for ngx-mask
  providers: [
    provideEnvironmentNgxMask()
  ]
})
export class ThirdPartyModule {
}
