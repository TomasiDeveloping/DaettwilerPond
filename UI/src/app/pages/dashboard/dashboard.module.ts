import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from "@angular/router";
import {DashboardComponent} from "./dashboard.component";
import {ClosedSeasonComponent} from "./closed-season/closed-season.component";
import {FishingRegulationComponent} from "./fishing-regulation/fishing-regulation.component";
import {FishTypesComponent} from "./closed-season/fish-types/fish-types.component";
import { FishingLicenseComponent } from './fishing-license/fishing-license.component';


@NgModule({
  declarations: [
    DashboardComponent,
    ClosedSeasonComponent,
    FishingRegulationComponent,
    FishTypesComponent,
    FishingLicenseComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {path: '', component: DashboardComponent}
    ])
  ]
})
export class DashboardModule {
}
