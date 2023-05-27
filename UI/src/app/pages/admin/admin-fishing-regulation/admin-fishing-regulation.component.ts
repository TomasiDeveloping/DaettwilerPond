import {Component, inject, OnInit} from '@angular/core';
import {FishingRegulation} from "../../../models/fishingRegulation.model";
import {FishingRegulationService} from "../../../services/fishing-regulation.service";

@Component({
  selector: 'app-admin-fishing-regulation',
  templateUrl: './admin-fishing-regulation.component.html',
  styleUrls: ['./admin-fishing-regulation.component.scss']
})
export class AdminFishingRegulationComponent implements OnInit{

  public fishingRegulations: FishingRegulation[] = [];

  private readonly _fishingRegulationService: FishingRegulationService = inject(FishingRegulationService);

  ngOnInit() {
    this._fishingRegulationService.getFishingRegulations().subscribe({
      next: ((response) => {
        if (response) {
          this.fishingRegulations = response;
        }
      })
    });
  }

  onEdit(regulation: FishingRegulation) {

  }

  onDelete(regulation: FishingRegulation) {

  }
}
