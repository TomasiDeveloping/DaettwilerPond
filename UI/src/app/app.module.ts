import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';
import { HomeComponent } from './pages/home/home.component';
import { CurrentTemperatureComponent } from './pages/temperature/current-temperature/current-temperature.component';
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import { TemperatureComponent } from './pages/temperature/temperature.component';
import { HistoryComponent } from './pages/history/history.component';
import {HighchartsChartModule} from "highcharts-angular";
import {DatePipe} from "@angular/common";
import { NgxScrollTopModule } from 'ngx-scrolltop';
import {NgxSpinnerModule} from "ngx-spinner";
import {SpinnerInterceptor} from "./interceptors/spinner.interceptor";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import { LoginComponent } from './authentication/login/login.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ToastrModule} from "ngx-toastr";
import {JwtModule} from "@auth0/angular-jwt";
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import {JwtInterceptor} from "./interceptors/jwt.interceptor";
import { ForgotPasswordDialogComponent } from './authentication/forgot-password-dialog/forgot-password-dialog.component';
import {MatDialogModule} from "@angular/material/dialog";
import { PasswordResetComponent } from './authentication/password-reset/password-reset.component';
import { ClosedSeasonComponent } from './pages/dashboard/closed-season/closed-season.component';


export function tokenGetter() {
  return localStorage.getItem('DaettwilerPondToken');
}
@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    HomeComponent,
    CurrentTemperatureComponent,
    TemperatureComponent,
    HistoryComponent,
    LoginComponent,
    DashboardComponent,
    ForgotPasswordDialogComponent,
    PasswordResetComponent,
    ClosedSeasonComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    HighchartsChartModule,
    NgxScrollTopModule,
    NgxSpinnerModule,
    ReactiveFormsModule,
    MatDialogModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter
      }
    }),
    FormsModule,
  ],
  providers: [
    DatePipe,
    {provide: HTTP_INTERCEPTORS, useClass: SpinnerInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
