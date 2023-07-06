import { Component, OnDestroy, OnInit } from '@angular/core';
import { PublicationsService } from '../../services/publications.service';
import { IPublication } from '../../interfaces/publication.interface';
import { ActivatedRoute, Router } from '@angular/router';
import { ImageUtils } from 'src/app/shared/utils/image.utils';
import { OffersService } from '../../services/offers.service';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { IOffer } from '../../interfaces/offer.interface';
import { ICreateExchange } from 'src/app/features/exchanges/interfaces/create-exchange.interface';
import { ExchangesService } from 'src/app/features/exchanges/services/exchanges.service';

@Component({
  selector: 'app-publication',
  templateUrl: './publication.component.html',
  styleUrls: ['./publication.component.scss'],
})
export class PublicationComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];

  publication!: IPublication;
  publicationId!: string;
  isOwner: boolean = false;
  isLogged: boolean = false;

  constructor(
    private publicationsService: PublicationsService,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private exchangesService: ExchangesService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.subscriptions.push(
      this.activatedRoute.params.subscribe(({ id }: any) => {
        this.publicationId = id;
        this.populatePublication(this.publicationId);
        this.subscriptions.push(
          this.publicationsService.publicationsChanged$.subscribe(() => {
            this.populatePublication(this.publicationId);
          })
        );
      })
    );
  }

  acceptOffer({ offerId, offerUserId }: any) {
    const createExchange: ICreateExchange = {
      offerId,
      publicationId: this.publicationId,
      authorId: this.publication?.userId,
      offerUserId,
    };
    this.exchangesService
      .createExchange(createExchange)
      .subscribe((exchange) => {
        this.router.navigateByUrl('/exchanges/' + exchange.id);
      });
  }

  populatePublication(id: string) {
    this.publicationsService.getPublicationById(id).subscribe({
      next: (data: IPublication) => {
        this.publication = data;

        this.subscriptions.push(
          this.authService.userLoggedIn$.subscribe((data) => {
            this.isLogged = !!data;
            this.isOwner =
              this.isLogged && this.publication?.userId === data?.id;
          })
        );
      },
    });
  }

  loadingRemoveButton = false;
  removePublication() {
    this.loadingRemoveButton = true;
    this.publicationsService.removePublication(this.publicationId).subscribe({
      next: () => {
        this.loadingRemoveButton = false;
        this.router.navigateByUrl('/home');
      },
      error: (err: any) => {
        this.loadingRemoveButton = false;
        console.log(err);
      },
    });
  }

  replaceIfAbsent(photo?: string) {
    return ImageUtils.replaceIfAbsent(photo);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((sub) => sub.unsubscribe());
  }
}
