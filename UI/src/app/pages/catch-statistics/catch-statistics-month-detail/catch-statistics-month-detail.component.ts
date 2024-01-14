import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {FishCatchService} from "../../../services/fish-catch.service";
import {FishCatchModel} from "../../../models/fishCatch.model";

@Component({
  selector: 'app-catch-statistics-month-detail',
  templateUrl: './catch-statistics-month-detail.component.html',
  styleUrl: './catch-statistics-month-detail.component.scss'
})
export class CatchStatisticsMonthDetailComponent implements OnInit{

  public currentMonth: number | null = null;
  public monthCatches: FishCatchModel[] = [];
  private readonly _activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  private readonly _fishCatchService: FishCatchService = inject(FishCatchService);

  ngOnInit() {
    const licenceId: string | null = this._activatedRoute.snapshot.paramMap.get('licenceId');
    const monthString: string | null = this._activatedRoute.snapshot.paramMap.get('month') ?? null;
    if (monthString) {
      this.currentMonth = +monthString;
    }

    if (!licenceId || this.currentMonth == null) {
      return;
    }
    this.getMonthCatches(licenceId, this.currentMonth);
  }

  getMonthCatches(licenceId: string, month: number) {
    this._fishCatchService.getCatchesForMonth(licenceId, month + 1).subscribe({
      next: ((response) => {
        if (response) {
          this.monthCatches = response;
        }
      })
    });
  }

}
