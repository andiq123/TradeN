import { Component, OnDestroy, OnInit } from '@angular/core';
import { IPublication } from '../../../features/publications/interfaces/publication.interface';
import { PublicationsService } from '../../../features/publications/services/publications.service';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { ImageUtils } from 'src/app/shared/utils/image.utils';
import { OffersService } from 'src/app/features/publications/services/offers.service';
import { IPhoto } from 'src/app/features/publications/interfaces/photo.interface';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit, OnDestroy {
  subscriptions: Subscription[] = [];
  queryParams = {
    title: '',
    orderBy: 'date',
    descending: true,
  };

  publications: IPublication[] = [];
  isLoggedIn = false;

  constructor(
    private publicationService: PublicationsService,
    private authService: AuthService,
    private actvatedRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.subscriptions.push(
      this.actvatedRoute.queryParams.subscribe((params: any) => {
        this.queryParams.title = params.title ?? this.queryParams.title;
        this.queryParams.orderBy = params.orderBy ?? this.queryParams.orderBy;
        this.queryParams.descending =
          params.descending ?? this.queryParams.descending;

        this.populatePublications();
      })
    );

    this.subscriptions.push(
      this.publicationService.publicationsChanged$.subscribe({
        next: () => {
          this.populatePublications();
        },
      })
    );

    this.subscriptions.push(
      this.authService.userLoggedIn$.subscribe((data) => {
        this.isLoggedIn = !!data;
      })
    );
  }

  timeOut: any;

  queryChanged() {
    clearTimeout(this.timeOut);

    this.timeOut = setTimeout(() => {
      this.router.navigate([], {
        relativeTo: this.actvatedRoute,
        queryParams: this.queryParams,
      });
    }, 500);
  }

  findMainPhoto(photos: IPhoto[] | undefined): string {
    return photos?.find((p) => p.isMain)?.url ?? '';
  }

  populatePublications() {
    this.publicationService.getPublications(this.queryParams).subscribe({
      next: (data) => {
        this.publications = data;
      },
      error: (err) => {
        this.publications = [];
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
