import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {NavigationComponent} from './navigation/navigation.component';
import {HomeComponent} from './pages/home/home.component';
import {CurrentTemperatureComponent} from './pages/temperature/current-temperature/current-temperature.component';
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {TemperatureComponent} from './pages/temperature/temperature.component';
import {HistoryComponent} from './pages/history/history.component';
import {DatePipe} from "@angular/common";
import {SpinnerInterceptor} from "./interceptors/spinner.interceptor";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {LoginComponent} from './authentication/login/login.component';
import {ReactiveFormsModule} from "@angular/forms";
import {JwtInterceptor} from "./interceptors/jwt.interceptor";
import {ForgotPasswordDialogComponent} from './authentication/forgot-password-dialog/forgot-password-dialog.component';
import {PasswordResetComponent} from './authentication/password-reset/password-reset.component';
import {AccountComponent} from './pages/user/account/account.component';
import {ChangePasswordComponent} from './pages/user/change-password/change-password.component';
import {MaterialModule} from "./sharedModules/material.module";
import {ThirdPartyModule} from "./sharedModules/third-party.module";
import {SyncfusionModule} from "./sharedModules/syncfusion.module";
import {DownloadComponent} from './pages/download/download.component';
import {JwtModule} from "@auth0/angular-jwt";


@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    HomeComponent,
    CurrentTemperatureComponent,
    TemperatureComponent,
    HistoryComponent,
    LoginComponent,
    ForgotPasswordDialogComponent,
    PasswordResetComponent,
    AccountComponent,
    ChangePasswordComponent,
    DownloadComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    ThirdPartyModule,
    MaterialModule,
    SyncfusionModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          return localStorage.getItem('DaettwilerPondToken')
        }
      }
    }),
  ],
  providers: [
    DatePipe,
    {provide: HTTP_INTERCEPTORS, useClass: SpinnerInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
