import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from "@angular/router";
import {AdminComponent} from "./admin.component";
import {SyncfusionModule} from "../../sharedModules/syncfusion.module";
import {AdminFishTypesComponent} from "./admin-fish-types/admin-fish-types.component";
import {AdminUsersComponent} from "./admin-users/admin-users.component";
import {AdminFishingRegulationComponent} from "./admin-fishing-regulation/admin-fishing-regulation.component";
import {AdminFishingLicenseComponent} from "./admin-fishing-license/admin-fishing-license.component";
import {AdminEditUserComponent} from "./admin-users/admin-edit-user/admin-edit-user.component";
import {AdminEditFishTypeComponent} from "./admin-fish-types/admin-edit-fish-type/admin-edit-fish-type.component";
import {
  AdminEditFishingRegulationComponent
} from "./admin-fishing-regulation/admin-edit-fishing-regulation/admin-edit-fishing-regulation.component";
import {
  AdminEditFishingLicenseComponent
} from "./admin-fishing-license/admin-edit-fishing-license/admin-edit-fishing-license.component";
import {ReactiveFormsModule} from "@angular/forms";
import {MaterialModule} from "../../sharedModules/material.module";


@NgModule({
  declarations: [
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
    CommonModule,
    SyncfusionModule,
    MaterialModule,
    ReactiveFormsModule,
    RouterModule.forChild([
      {path: '', component: AdminComponent}
    ]),
  ]
})
export class AdminModule {
}
