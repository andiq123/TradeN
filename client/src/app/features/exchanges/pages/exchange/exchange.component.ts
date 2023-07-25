import { Component, OnDestroy, OnInit } from '@angular/core';
import { ExchangesService } from '../../services/exchanges.service';
import { ActivatedRoute } from '@angular/router';
import { IExchange } from '../../interfaces/exchange.interface';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { RatingsService } from 'src/app/features/publications/services/ratings.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-exchange',
  templateUrl: './exchange.component.html',
  styleUrls: ['./exchange.component.scss'],
})
export class ExchangeComponent implements OnInit, OnDestroy {
  private subsciptions: Subscription[] = [];
  authorIsWatching = false;
  exchange!: IExchange;
  alreadyRated = false;

  constructor(
    private exchangesService: ExchangesService,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private ratingService: RatingsService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.subsciptions.push(
      this.activatedRoute.params.subscribe(({ id }: any) => {
        this.exchangesService.getExchange(id).subscribe((exchange) => {
          this.exchange = exchange;
          this.subsciptions.push(
            this.authService.userLoggedIn$.subscribe((user) => {
              this.authorIsWatching = user?.id === this.exchange.authorId;
              this.ratingService
                .checkIfAvailable(this.exchange.publicationId)
                .subscribe((result) => {
                  this.alreadyRated = result.alreadyRated;
                });
            })
          );
        });
      })
    );
  }

  onRate(rate: number) {
    this.ratingService
      .setRating({
        forPublicationId: this.exchange.publicationId,
        forUserId: this.authorIsWatching
          ? this.exchange.offerUserId
          : this.exchange.authorId,
        rate,
      })
      .subscribe(() => {
        this.toastr.success('Recenzie adaugata cu success!');
        this.alreadyRated = true;
      });
  }

  ngOnDestroy(): void {
    this.subsciptions.forEach((sub) => sub.unsubscribe());
  }
}
