import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { IExchange } from '../interfaces/exchange.interface';
import { ICreateExchange } from '../interfaces/create-exchange.interface';

@Injectable({
  providedIn: 'root',
})
export class ExchangesService {
  apiUrl = environment.baseApiUrl + 'exchanges';
  constructor(private http: HttpClient) {}

  getExchanges() {
    return this.http.get<IExchange[]>(this.apiUrl + '/my-exchanges');
  }

  getExchange(id: string) {
    return this.http.get<IExchange>(this.apiUrl + '/' + id);
  }

  createExchange(exchange: ICreateExchange) {
    return this.http.post<IExchange>(this.apiUrl, exchange);
  }
}
