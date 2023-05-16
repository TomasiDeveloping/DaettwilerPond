import {Component, inject, OnInit} from '@angular/core';
import {TemperatureService} from "../../services/temperature.service";
import {TemperatureMeasurement} from "../../models/temperatureMeasurement.model";

@Component({
  selector: 'app-current-temperature',
  templateUrl: './current-temperature.component.html',
  styleUrls: ['./current-temperature.component.scss']
})
export class CurrentTemperatureComponent implements OnInit{

  public currentTemperature!: TemperatureMeasurement;

  private readonly _temperatureService: TemperatureService = inject(TemperatureService);

  ngOnInit(): void {
    this._temperatureService.getCurrentTemperature().subscribe({
      next: ((response) => {
        if (response) {
          this.currentTemperature = response;
        }
      })
    });
  }

}
