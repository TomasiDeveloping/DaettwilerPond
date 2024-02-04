import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {
  DetailRowService,
  GridModule,
  GroupService,
  PagerModule,
  PageService,
  ResizeService,
  SearchService,
  SortService,
  ToolbarService
} from "@syncfusion/ej2-angular-grids";


@NgModule({
  declarations: [],
  imports: [
    // CommonModule is imported to ensure common directives like *ngFor, *ngIf are available
    CommonModule,
    // Importing Syncfusion Grid modules for UI components
    PagerModule,
    GridModule,
  ],
  // Exporting both Angular and Syncfusion Grid modules for use in other modules
  exports: [
    CommonModule,
    PagerModule,
    GridModule
  ],
  // Providing Syncfusion Grid services to be used as dependencies
  providers: [
    GroupService,
    DetailRowService,
    PageService,
    SearchService,
    ToolbarService,
    SortService,
    ResizeService,]
})
export class SyncfusionModule {
}
