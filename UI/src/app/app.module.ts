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
import { FishingRegulationComponent } from './pages/dashboard/fishing-regulation/fishing-regulation.component';
import { FishTypesComponent } from './pages/dashboard/closed-season/fish-types/fish-types.component';
import { AccountComponent } from './pages/user/account/account.component';
import { ChangePasswordComponent } from './pages/user/change-password/change-password.component';
import {DetailRowService, GridModule, GroupService, PagerModule} from '@syncfusion/ej2-angular-grids';
import { AdminComponent } from './pages/admin/admin.component';
import {MatTabsModule} from "@angular/material/tabs";
import { AdminFishTypesComponent } from './pages/admin/admin-fish-types/admin-fish-types.component';
import { AdminUsersComponent } from './pages/admin/admin-users/admin-users.component';
import { AdminFishingRegulationComponent } from './pages/admin/admin-fishing-regulation/admin-fishing-regulation.component';
import { AdminFishingLicenseComponent } from './pages/admin/admin-fishing-license/admin-fishing-license.component';
import {MatCardModule} from "@angular/material/card";
import { AdminEditUserComponent } from './pages/admin/admin-users/admin-edit-user/admin-edit-user.component';
import { AdminEditFishTypeComponent } from './pages/admin/admin-fish-types/admin-edit-fish-type/admin-edit-fish-type.component';
import { AdminEditFishingRegulationComponent } from './pages/admin/admin-fishing-regulation/admin-edit-fishing-regulation/admin-edit-fishing-regulation.component';
import { AdminEditFishingLicenseComponent } from './pages/admin/admin-fishing-license/admin-edit-fishing-license/admin-edit-fishing-license.component';


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
    FishingRegulationComponent,
    FishTypesComponent,
    AccountComponent,
    ChangePasswordComponent,
    AdminComponent,
    AdminFishTypesComponent,
    AdminUsersComponent,
    AdminFishingRegulationComponent,
    AdminFishingLicenseComponent,
    AdminEditUserComponent,
    AdminEditFishTypeComponent,
    AdminEditFishingRegulationComponent,
    AdminEditFishingLicenseComponent,
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
    GridModule, PagerModule, MatTabsModule, MatCardModule,
  ],
  providers: [
    DatePipe,
    GroupService,
    DetailRowService,
    {provide: HTTP_INTERCEPTORS, useClass: SpinnerInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
