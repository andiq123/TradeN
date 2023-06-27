import { Component, OnDestroy, OnInit } from '@angular/core';
import { ExchangesService } from '../../services/exchanges.service';
import { ActivatedRoute } from '@angular/router';
import { IExchange } from '../../interfaces/exchange.interface';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/features/auth/services/auth.service';

@Component({
  selector: 'app-exchange',
  templateUrl: './exchange.component.html',
  styleUrls: ['./exchange.component.scss'],
})
export class ExchangeComponent implements OnInit, OnDestroy {
  private subsciptions: Subscription[] = [];
  authorIsWatching = false;
  exchange!: IExchange;

  constructor(
    private exchangesService: ExchangesService,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.subsciptions.push(
      this.activatedRoute.params.subscribe(({ id }: any) => {
        this.exchangesService.getExchange(id).subscribe((exchange) => {
          this.exchange = exchange;
          this.subsciptions.push(
            this.authService.userLoggedIn$.subscribe((user) => {
              this.authorIsWatching = user?.id === this.exchange.authorId;
            })
          );
        });
      })
    );
  }

  ngOnDestroy(): void {
    this.subsciptions.forEach((sub) => sub.unsubscribe());
  }
}
