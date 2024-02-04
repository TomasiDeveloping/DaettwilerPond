import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {TemperatureMeasurement} from "../models/temperatureMeasurement.model";
import {TemperatureHistoryModel} from "../models/temperatureHistory.model";

@Injectable({
  providedIn: 'root'
})
export class TemperatureService {

  // API base URL obtained from environment
  private readonly _serviceUrl: string = environment.apiBaseUrl + '/Lsn50V2Measurements';

  // Injecting necessary services
  private readonly _httpClient: HttpClient = inject(HttpClient);


  // Method to get the latest temperature measurement
  getCurrentTemperature(): Observable<TemperatureMeasurement> {
    return this._httpClient.get<TemperatureMeasurement>(this._serviceUrl + '/GetLatestTemperatureMeasurement');
  }

  // Method to get temperature measurements for a specific number of days
  getTemperatureMeasurementByDay(days: number = 0): Observable<TemperatureMeasurement[]> {
    return this._httpClient.get<TemperatureMeasurement[]>(this._serviceUrl + '/GetTemperatureMeasurementByDay/' + days);
  }

  // Method to get historical temperature data
  getHistoryData(): Observable<TemperatureHistoryModel> {
    return this._httpClient.get<TemperatureHistoryModel>(this._serviceUrl + '/GetHistoryData');
  }
}
