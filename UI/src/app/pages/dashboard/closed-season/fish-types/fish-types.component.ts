import {Component, Input} from '@angular/core';
import {FishType} from "../../../../models/fishType.model";

@Component({
  selector: 'app-fish-types',
  templateUrl: './fish-types.component.html'
})
export class FishTypesComponent {

  // Declaring an input property to receive an array of FishType objects
  @Input() fishTypes!: FishType[];
}
