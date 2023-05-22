import {Component, Input} from '@angular/core';
import {FishType} from "../../../../models/fishType.model";

@Component({
  selector: 'app-fish-types',
  templateUrl: './fish-types.component.html',
  styleUrls: ['./fish-types.component.scss']
})
export class FishTypesComponent {
  @Input() fishTypes!: FishType[];
}
