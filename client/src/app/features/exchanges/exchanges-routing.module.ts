import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExchangesComponent } from './pages/exchanges/exchanges.component';
import { ExchangeComponent } from './pages/exchange/exchange.component';

const routes: Routes = [
  {
    path: '',
    component: ExchangesComponent,
    pathMatch: 'full',
  },
  {
    path: ':id',
    component: ExchangeComponent,
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ExchangesRoutingModule {}
