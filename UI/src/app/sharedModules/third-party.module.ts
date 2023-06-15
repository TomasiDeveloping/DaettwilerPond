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
    CommonModule,
    HighchartsChartModule,
    NgxScrollTopModule,
    NgxSpinnerModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    NgxMaskDirective,
    NgxMaskPipe
  ],
  exports: [
    HighchartsChartModule,
    NgxScrollTopModule,
    NgxSpinnerModule,
    ToastrModule,
    NgxMaskDirective,
    NgxMaskPipe
  ],
  providers: [
    provideEnvironmentNgxMask()
  ]
})
export class ThirdPartyModule {
}
