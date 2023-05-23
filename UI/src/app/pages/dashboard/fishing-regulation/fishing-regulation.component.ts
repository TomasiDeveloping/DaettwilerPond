import {Component, inject, OnInit} from '@angular/core';
import {FishingRegulation} from "../../../models/fishingRegulation.model";
import {FishingRegulationService} from "../../../services/fishing-regulation.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-fishing-regulation',
  templateUrl: './fishing-regulation.component.html',
  styleUrls: ['./fishing-regulation.component.scss']
})
export class FishingRegulationComponent implements OnInit {

  public fishingRegulations: FishingRegulation[] = [];

  private readonly _fishingRegulationService: FishingRegulationService = inject(FishingRegulationService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  ngOnInit(): void {
    this.getFishingRegulations();
  }

  getFishingRegulations() {
    this._fishingRegulationService.getFishingRegulations().subscribe({
      next: ((response) => {
        if (response) {
          this.fishingRegulations = response;
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Fehler beim abrufen der Fischereivorschriften', 'Fischereivorschriften');
      }
    });
  }
}
