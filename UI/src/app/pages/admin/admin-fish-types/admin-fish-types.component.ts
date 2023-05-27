import {Component, inject, OnInit} from '@angular/core';
import {FishTypeService} from "../../../services/fish-type.service";
import {FishType} from "../../../models/fishType.model";

@Component({
  selector: 'app-admin-fish-types',
  templateUrl: './admin-fish-types.component.html',
  styleUrls: ['./admin-fish-types.component.scss']
})
export class AdminFishTypesComponent implements OnInit{

  private readonly _fishTypeService: FishTypeService = inject(FishTypeService);
  fishTypes: FishType[] = [];
  ngOnInit() {
    this.getFishTypes();
  }

  getFishTypes() {
    this._fishTypeService.getFishTypes().subscribe({
      next: ((response) => {
        if (response) {
          this.fishTypes = response;
        }
      })
    });
  }

  onEdit(event: any) {
    const fishType: FishType = event.rowData as FishType;
    console.log(fishType);
  }
}
