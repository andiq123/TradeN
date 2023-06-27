import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExchangesRoutingModule } from './exchanges-routing.module';
import { ExchangeComponent } from './pages/exchange/exchange.component';
import { ExchangesComponent } from './pages/exchanges/exchanges.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [ExchangeComponent, ExchangesComponent],
  imports: [CommonModule, ExchangesRoutingModule, SharedModule],
})
export class ExchangesModule {}
