import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {JwtModule} from "@auth0/angular-jwt";
import {ToastrModule} from "ngx-toastr";
import {HighchartsChartModule} from "highcharts-angular";
import {NgxScrollTopModule} from "ngx-scrolltop";
import {NgxSpinnerModule} from "ngx-spinner";


export function tokenGetter() {
  return localStorage.getItem('DaettwilerPondToken');
}
@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HighchartsChartModule,
    NgxScrollTopModule,
    NgxSpinnerModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter
      }
    }),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
  ],
  exports: [
    HighchartsChartModule,
    NgxScrollTopModule,
    NgxSpinnerModule,
    JwtModule,
    ToastrModule,
  ]
})
export class ThirdPartyModule { }
