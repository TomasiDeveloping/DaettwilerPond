import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {MatTabsModule} from "@angular/material/tabs";
import {MatCardModule} from "@angular/material/card";
import {MatDialogModule} from "@angular/material/dialog";
import {MatInputModule} from "@angular/material/input";
import {MatSelectModule} from "@angular/material/select";
import {MatCheckboxModule} from "@angular/material/checkbox";


@NgModule({
  declarations: [],
  imports: [
    // CommonModule is imported to ensure common directives like *ngFor, *ngIf are available
    CommonModule,
    // Importing Angular Material modules for UI components
    MatTabsModule,
    MatCardModule,
    MatDialogModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
  ],
  // Exporting Angular Material modules for use in other modules
  exports: [
    MatTabsModule,
    MatCardModule,
    MatDialogModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
  ]
})
export class MaterialModule {
}
