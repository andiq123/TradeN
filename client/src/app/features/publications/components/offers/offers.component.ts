import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { IOffer } from '../../interfaces/offer.interface';
import { Subscription } from 'rxjs';
import { OffersService } from '../../services/offers.service';
import { IExchange } from 'src/app/features/exchanges/interfaces/exchange.interface';
import { ExchangesService } from 'src/app/features/exchanges/services/exchanges.service';

@Component({
  selector: 'app-offers',
  templateUrl: './offers.component.html',
  styleUrls: ['./offers.component.scss'],
})
export class OffersComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];

  @Input() isLogged = false;
  @Input() isOwner = false;
  @Input() publicationId = '';
  offers: IOffer[] = [];

  @Output() offerAccepted = new EventEmitter<{
    offerId: string;
    offerUserId: string;
  }>();

  constructor(private offersService: OffersService) {}

  ngOnInit(): void {
    this.populateOffers();

    this.subscriptions.push(
      this.offersService.offersChanged$.subscribe(() => {
        this.populateOffers();
      })
    );
  }

  populateOffers() {
    this.subscriptions.push(
      this.getOffers(this.isOwner, this.publicationId).subscribe({
        next: (offers: IOffer[]) => {
          this.offers = offers;
        },
        error: (err: any) => {
          this.offers = [];
          console.log(err);
        },
      })
    );
  }

  getOffers(isOwner: boolean, publicationId: string) {
    return isOwner
      ? this.offersService.getOffersByPublicationId(publicationId)
      : this.offersService.getOffersByUserIdForPublicationId(publicationId);
  }

  removeOffer(offerId: string) {
    this.offersService.removeOffer(offerId).subscribe({
      next: () => {
        this.populateOffers();
      },
      error: (err: any) => {
        console.log(err);
      },
    });
  }

  acceptOffer(data: any) {
    this.offerAccepted.emit(data);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((s) => s.unsubscribe());
  }
}
