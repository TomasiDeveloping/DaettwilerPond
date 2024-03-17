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
import {AdminClubInformationComponent} from './admin-club-information/admin-club-information.component';
import {CreateLicenseBillComponent} from './admin-fishing-license/create-license-bill/create-license-bill.component';
import {ThirdPartyModule} from "../../sharedModules/third-party.module";
import { AdminSendEmailComponent } from './admin-users/admin-send-email/admin-send-email.component';
import { UserImageUploadComponent } from './admin-users/admin-edit-user/user-image-upload/user-image-upload.component';
import {ImageUrlPipe} from "../../pipes/image-url.pipe";


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
    AdminClubInformationComponent,
    CreateLicenseBillComponent,
    AdminSendEmailComponent,
    UserImageUploadComponent,
  ],
  imports: [
    CommonModule,
    SyncfusionModule,
    MaterialModule,
    ThirdPartyModule,
    ReactiveFormsModule,
    RouterModule.forChild([
      {path: '', component: AdminComponent}
    ]),
  ],
  providers: [ImageUrlPipe]
})
export class AdminModule {
}
