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

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    HomeComponent,
    CurrentTemperatureComponent,
    TemperatureComponent,
    HistoryComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    HighchartsChartModule,
    NgxScrollTopModule,
    NgxSpinnerModule
  ],
  providers: [
    DatePipe,
    {provide: HTTP_INTERCEPTORS, useClass: SpinnerInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
