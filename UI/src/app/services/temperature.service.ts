import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {TemperatureMeasurement} from "../models/temperatureMeasurement.model";

@Injectable({
  providedIn: 'root'
})
export class TemperatureService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + '/Lsn50V2Measurements';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getCurrentTemperature(): Observable<TemperatureMeasurement> {
    return this._httpClient.get<TemperatureMeasurement>(this._serviceUrl + '/GetLatestTemperatureMeasurement');
  }
}
