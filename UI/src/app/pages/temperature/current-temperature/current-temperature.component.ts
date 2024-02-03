import {Component, inject, OnInit} from '@angular/core';
import {TemperatureService} from "../../../services/temperature.service";
import {TemperatureMeasurement} from "../../../models/temperatureMeasurement.model";

@Component({
  selector: 'app-current-temperature',
  templateUrl: './current-temperature.component.html',
  styleUrls: ['./current-temperature.component.scss']
})
export class CurrentTemperatureComponent implements OnInit {

  // Property to store the current temperature
  public currentTemperature!: TemperatureMeasurement;

  // Injecting the TemperatureService using the inject function
  private readonly _temperatureService: TemperatureService = inject(TemperatureService);

  ngOnInit(): void {
    // Fetch the current temperature from the TemperatureService
    this._temperatureService.getCurrentTemperature().subscribe({
      next: ((response: TemperatureMeasurement): void => {
        // Check if the response is truthy
        if (response) {
          // Assign the response to the currentTemperature property
          this.currentTemperature = response;
        }
      })
    });
  }
}
