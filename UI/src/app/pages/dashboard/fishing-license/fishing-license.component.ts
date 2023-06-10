import {Component, Input, OnChanges, SimpleChanges} from '@angular/core';
import {FishingLicense} from "../../../models/fishingLicense.model";
import * as moment from "moment";

@Component({
  selector: 'app-fishing-license',
  templateUrl: './fishing-license.component.html',
  styleUrls: ['./fishing-license.component.scss']
})
export class FishingLicenseComponent implements OnChanges {

  @Input() public fishingLicence: FishingLicense | undefined;
  public expiresInDays: number | undefined;

  ngOnChanges(changes: SimpleChanges) {
    if (this.fishingLicence) {
      if (new Date(this.fishingLicence.expiresOn) > new Date()) {
        this.expiresInDays = moment(this.fishingLicence?.expiresOn).diff(Date.now(), 'days');
      }
    }
  }
}
