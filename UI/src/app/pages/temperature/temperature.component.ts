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

  public currentMeasurement!: TemperatureMeasurement;
  public lastMeasurement!: TemperatureMeasurement;
  public Highcharts: typeof Highcharts = Highcharts;
  public loadData: boolean = false;
  public historyData: TemperatureHistoryModel | undefined;
  public currentYear: number = new Date().getFullYear();
  public currentMonth: number = new Date().getMonth();

  private readonly _temperatureService: TemperatureService = inject(TemperatureService);
  private readonly _datePipe: DatePipe = inject(DatePipe);
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

  ngOnInit() {
    this._temperatureService.getTemperatureMeasurementByDay().subscribe({
      next: ((response) => {
        if (response) {
          response.forEach((measurement) => {
            // @ts-ignore
            this.chartOptions.series[0].data.push([new Date(measurement.receivedAt).getTime(), measurement.temperature]);
          });
          this.currentMeasurement = response[response.length - 1];
          this.lastMeasurement = response[response.length - 2];
          this.loadData = true;
        }
      })
    });
    this._temperatureService.getHistoryData().subscribe({
      next: ((response) => {
        if (response) {
          this.historyData = response;
        }
      })
    });
  }

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
