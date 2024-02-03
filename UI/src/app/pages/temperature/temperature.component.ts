import {Component, inject, OnInit} from '@angular/core';
import * as Highcharts from 'highcharts';
import {TemperatureService} from "../../services/temperature.service";
import {DatePipe} from "@angular/common";
import {TemperatureMeasurement} from "../../models/temperatureMeasurement.model";
import {TemperatureHistoryModel} from "../../models/temperatureHistory.model";

@Component({
  selector: 'app-temperature',
  templateUrl: './temperature.component.html',
  styleUrls: ['./temperature.component.scss']
})
export class TemperatureComponent implements OnInit {

  // Properties to store current and last temperature measurements, Highcharts instance, and other data
  public currentMeasurement!: TemperatureMeasurement;
  public lastMeasurement!: TemperatureMeasurement;
  public Highcharts: typeof Highcharts = Highcharts;
  public loadData: boolean = false;
  public historyData: TemperatureHistoryModel | undefined;
  public currentYear: number = new Date().getFullYear();
  public currentMonth: number = new Date().getMonth();

  // Injected services for temperature data and date formatting
  private readonly _temperatureService: TemperatureService = inject(TemperatureService);
  private readonly _datePipe: DatePipe = inject(DatePipe);

  // Highcharts chart options for displaying temperature data
  chartOptions: Highcharts.Options = {
    global: {
      months: ['Januar', 'Februar', 'März', 'April', 'Mai', 'Juni', 'Juli', 'August', 'September', 'Oktober', 'November', 'Dezember'],
    },
    lang: {
      loading: 'Daten werden geladen...',
      months: ['Januar', 'Februar', 'März', 'April', 'Mai', 'Juni', 'Juli', 'August', 'September', 'Oktober', 'November', 'Dezember'],
      weekdays: ['Sonntag', 'Montag', 'Dienstag', 'Mittwoch', 'Donnerstag', 'Freitag', 'Samstag'],
      shortMonths: ['Jan', 'Feb', 'Mär', 'Apr', 'Mai', 'Jun', 'Jul', 'Aug', 'Sep', 'Okt', 'Nov', 'Dez'],
    },
    time: {
      timezoneOffset: new Date().getTimezoneOffset()
    },
    series: [{
      name: 'Wassertemperatur',
      data: [],
      type: 'line'
    }],
    title: {
      // @ts-ignore
      text: this._datePipe.transform(Date.now(), 'dd.MM.yyyy').toString()
    },
    xAxis: [{
      title: {
        text: 'Zeit'
      },
      type: 'datetime',
      labels: {
        format: '{value:%H:%M}'
      }
    }],
    yAxis: [{
      labels: {
        format: '{value}°C',
        style: {
          color: 'blue'
        }
      },
      title: {
        text: 'Temperatur',
        style: {
          color: 'black'
        }
      }
    }]
  };

  ngOnInit(): void {
    // Fetch current day's temperature measurements and populate chart data
    this._temperatureService.getTemperatureMeasurementByDay().subscribe({
      next: ((response: TemperatureMeasurement[]): void => {
        if (response) {
          response.forEach((measurement: TemperatureMeasurement): void => {
            // @ts-ignore
            this.chartOptions.series[0].data.push([new Date(measurement.receivedAt).getTime(), measurement.temperature]);
          });
          this.currentMeasurement = response[response.length - 1];
          this.lastMeasurement = response[response.length - 2];
          this.loadData = true;
        }
      })
    });

    // Fetch historical temperature data
    this._temperatureService.getHistoryData().subscribe({
      next: ((response: TemperatureHistoryModel): void => {
        if (response) {
          this.historyData = response;
        }
      })
    });
  }

  // Determine temperature icon based on current and last measurements
  getIcon(): string {
    if (this.currentMeasurement.temperature === this.lastMeasurement.temperature) {
      return 'bi bi-arrow-right';
    }
    if (this.currentMeasurement.temperature < this.lastMeasurement.temperature) {
      return 'bi bi-arrow-down-right down-icon';
    }
    return 'bi bi-arrow-up-right up-icon';
  }
}
