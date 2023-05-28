import {Component, inject, OnInit} from '@angular/core';
import {FishingRegulation} from "../../../models/fishingRegulation.model";
import {FishingRegulationService} from "../../../services/fishing-regulation.service";
import {MatDialog} from "@angular/material/dialog";
import {ToastrService} from "ngx-toastr";
import {
  AdminEditFishingRegulationComponent
} from "./admin-edit-fishing-regulation/admin-edit-fishing-regulation.component";
import Swal from "sweetalert2";

@Component({
  selector: 'app-admin-fishing-regulation',
  templateUrl: './admin-fishing-regulation.component.html',
  styleUrls: ['./admin-fishing-regulation.component.scss']
})
export class AdminFishingRegulationComponent implements OnInit{

  public fishingRegulations: FishingRegulation[] = [];

  private readonly _fishingRegulationService: FishingRegulationService = inject(FishingRegulationService);
  private readonly _dialog: MatDialog = inject(MatDialog);
  private readonly _toastr: ToastrService = inject(ToastrService);

  ngOnInit() {
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
        this._toastr.error(error.error ?? 'Fehler beim Abrufen der Vorschriften', 'Fehler Vorschriften');
      }
    });
  }

  onAddFishingRegulation() {
    const regulation: FishingRegulation = {
      id: '',
      regulation: ''
    };
    this.openEditFishingRegulationDialog(false, regulation);
  }
  onEdit(regulation: FishingRegulation) {
    this.openEditFishingRegulationDialog(true, regulation);
  }

  onDelete(regulation: FishingRegulation) {
    Swal.fire({
      title: 'Bist Du sicher?',
      text: `Vorschrift wirklisch löschen?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Ja, bitte löschen!',
      cancelButtonText: 'Abbrechen'
    }).then((result) => {
      if (result.isConfirmed) {
        this._fishingRegulationService.deleteFishingRegulation(regulation.id).subscribe({
          next: ((response) => {
            if (response) {
              this._toastr.success(`Vorschrift wurde erfolgreich gelöscht`, 'Vorschrift Löschen');
              this.getFishingRegulations();
            }
          }),
          error: error => {
            this._toastr.error(error.error ?? 'Vorschrift konnte nicht gelöscht werden', 'Vorschrift Löschen');
          }
        });
      }
    });
  }

  openEditFishingRegulationDialog(isUpdate: boolean, fishingRegulation: FishingRegulation) {
    const dialogRef = this._dialog.open(AdminEditFishingRegulationComponent, {
      width: '80%',
      height: 'auto',
      autoFocus: false,
      disableClose: true,
      data: {isUpdate: isUpdate, fishingRegulation: fishingRegulation}
    });
    dialogRef.afterClosed().subscribe((result: {reload: boolean}) => {
      if (result.reload) {
        this.getFishingRegulations();
      }
    })
  }

}
