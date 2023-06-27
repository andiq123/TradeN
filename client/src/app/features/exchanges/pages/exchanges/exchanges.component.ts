import { Component, OnInit } from '@angular/core';
import { ExchangesService } from '../../services/exchanges.service';
import { IExchange } from '../../interfaces/exchange.interface';

@Component({
  selector: 'app-exchanges',
  templateUrl: './exchanges.component.html',
  styleUrls: ['./exchanges.component.scss'],
})
export class ExchangesComponent implements OnInit {
  exchanges: IExchange[] = [];

  constructor(private exchangesService: ExchangesService) {}

  ngOnInit(): void {
    this.exchangesService.getExchanges().subscribe((exchanges) => {
      this.exchanges = exchanges;
    });
  }
}
