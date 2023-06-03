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
    CommonModule,
    PagerModule,
    GridModule,
  ],
  exports: [
    CommonModule,
    PagerModule,
    GridModule
  ],
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
