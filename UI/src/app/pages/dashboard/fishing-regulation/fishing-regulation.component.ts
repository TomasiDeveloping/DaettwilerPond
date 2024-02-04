import {Component, inject, OnInit} from '@angular/core';
import {FishingRegulation} from "../../../models/fishingRegulation.model";
import {FishingRegulationService} from "../../../services/fishing-regulation.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-fishing-regulation',
  templateUrl: './fishing-regulation.component.html'
})
export class FishingRegulationComponent implements OnInit {

  // Public property to store an array of fishing regulations
  public fishingRegulations: FishingRegulation[] = [];

  // Private properties for FishingRegulationService and ToastrService using Angular DI
  private readonly _fishingRegulationService: FishingRegulationService = inject(FishingRegulationService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  ngOnInit(): void {
    this.getFishingRegulations();
  }

  // Method to fetch fishing regulations from the FishingRegulationService
  getFishingRegulations(): void {
    this._fishingRegulationService.getFishingRegulations().subscribe({
      next: ((response: FishingRegulation[]): void => {
        if (response) {
          // Assigning fetched fishing regulations to the public property
          this.fishingRegulations = response;
        }
      }),
      // Handling errors and displaying toastr messages
      error: error => {
        this._toastr.error(error.error ?? 'Fehler beim abrufen der Fischereivorschriften', 'Fischereivorschriften');
      }
    });
  }
}
