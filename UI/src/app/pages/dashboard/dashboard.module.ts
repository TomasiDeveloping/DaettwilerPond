import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from "@angular/router";
import {DashboardComponent} from "./dashboard.component";
import {ClosedSeasonComponent} from "./closed-season/closed-season.component";
import {FishingRegulationComponent} from "./fishing-regulation/fishing-regulation.component";
import {FishTypesComponent} from "./closed-season/fish-types/fish-types.component";
import { FishingLicenseComponent } from './fishing-license/fishing-license.component';
import { EFishingLicenseComponent } from './fishing-license/efishing-license/efishing-license.component';
import {NgxPrinterModule} from "ngx-printer";
import {ImageUrlPipe} from "../../pipes/image-url.pipe";
import {QRCodeComponent} from "angularx-qrcode";
import {MatDialogModule} from "@angular/material/dialog";


@NgModule({
  declarations: [
    DashboardComponent,
    ClosedSeasonComponent,
    FishingRegulationComponent,
    FishTypesComponent,
    FishingLicenseComponent,
    EFishingLicenseComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {path: '', component: DashboardComponent}
    ]),
    MatDialogModule,
    QRCodeComponent,
    NgxPrinterModule,
    ImageUrlPipe,
  ]
})
export class DashboardModule {
}
