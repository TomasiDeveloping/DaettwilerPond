import {Component, inject, OnInit} from '@angular/core';
import {FishingClubService} from "../../../services/fishing-club.service";
import {FishingClub} from "../../../models/fishingClub.model";
import {ToastrService} from "ngx-toastr";
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-admin-club-information',
  templateUrl: './admin-club-information.component.html',
  styleUrls: ['./admin-club-information.component.scss']
})
export class AdminClubInformationComponent implements OnInit {
  public fishingClub: FishingClub | undefined;
  public fishingClubForm!: FormGroup;
  public isEdit: boolean = false;

  private isNewClub: boolean = false;

  private readonly _fishingClubService: FishingClubService = inject(FishingClubService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  get name() {
    return this.fishingClubForm.get('name');
  }

  get billAddressName() {
    return this.fishingClubForm.get('billAddressName');
  }

  get billAddress() {
    return this.fishingClubForm.get('billAddress');
  }

  get billPostalCode() {
    return this.fishingClubForm.get('billPostalCode');
  }

  get billCity() {
    return this.fishingClubForm.get('billCity');
  }

  get ibanNumber() {
    return this.fishingClubForm.get('ibanNumber');
  }

  get licensePrice() {
    return this.fishingClubForm.get('licensePrice');
  }

  ngOnInit() {
    this.getFishingClub();
  }

  getFishingClub() {
    this._fishingClubService.getFishingClubs().subscribe({
      next: ((response) => {
        if (response) {
          this.fishingClub = response[0];
          this.createFishingClubForm(this.fishingClub);
        } else {
          this.isNewClub = true;
          this.createFishingClubForm(this.fishingClub);
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Etwa ist schief gelaufen', 'Fischerclub Daten');
      }
    });
  }

  createFishingClubForm(fishingClub: FishingClub | undefined) {
    this.fishingClubForm = new FormGroup({
      id: new FormControl<string | null>(fishingClub ? fishingClub.id : null),
      name: new FormControl<string | null>(fishingClub ? fishingClub.name : null, [Validators.required]),
      billAddressName: new FormControl<string | null>(fishingClub ? fishingClub.billAddressName : null, [Validators.required]),
      billAddress: new FormControl<string | null>(fishingClub ? fishingClub.billAddress : null, [Validators.required]),
      billPostalCode: new FormControl<string | null>(fishingClub ? fishingClub.billPostalCode : null, [Validators.required]),
      billCity: new FormControl<string | null>(fishingClub ? fishingClub.billCity : null, [Validators.required]),
      ibanNumber: new FormControl<string | null>(fishingClub ? fishingClub.ibanNumber : null, [Validators.required]),
      licensePrice: new FormControl<number | null>(fishingClub ? fishingClub.licensePrice : null, [Validators.required])
    });
    this.fishingClubForm.disable();
  }

  onSubmit() {
    if (!this.fishingClubForm || this.fishingClubForm.invalid) {
      return;
    }
    this.isEdit = false;
    const fishingClub: FishingClub = this.fishingClubForm.value as FishingClub;
    this, this.isNewClub ? this.createFishingClub(fishingClub) : this.updateFishingClub(fishingClub.id, fishingClub);
  }

  onEdit() {
    this.isEdit = true;
    this.fishingClubForm?.enable();
  }

  onCancel() {
    this.isEdit = false;
    this.createFishingClubForm(this.fishingClub);
    this.fishingClubForm?.disable();
  }

  private createFishingClub(fishingClub: FishingClub) {
    this._fishingClubService.createFishingClub(fishingClub).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Fischerclubdaten erfolgreich hinzugefügt', 'Fischerclubdaten');
          this.createFishingClubForm(response);
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Fischerclubdaten konnten nicht hinzugefügt werden', 'Fischerclubdaten');
      }
    });
  }

  private updateFishingClub(fishingClubId: string, fishingClub: FishingClub) {
    this._fishingClubService.updateFishingClub(fishingClubId, fishingClub).subscribe({
      next: ((response) => {
        if (response) {
          this.createFishingClubForm(response);
          this._toastr.success('Daten wurden erfolgreich bearbeitet', 'Daten bearbeiten');
        }
      }),
      error: error => {
        this._toastr.error(error.error ?? 'Daten konnten nicht bearbeitet werden', 'Daten bearbeiten');
      }
    });
  }
}
