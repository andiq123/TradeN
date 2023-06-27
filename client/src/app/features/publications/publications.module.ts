import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PublicationComponent } from './pages/publication/publication.component';
import { PublicationsRoutingModule } from './publications-routing.module';
import { UserInfoComponent } from './components/user-info/user-info.component';
import { SharedModule } from '../../shared/shared.module';
import { AddOfferComponent } from './components/add-offer/add-offer.component';
import { OffersComponent } from './components/offers/offers.component';
import { CarouselComponent } from './components/carousel/carousel.component';

@NgModule({
  declarations: [
    PublicationComponent,
    UserInfoComponent,
    AddOfferComponent,
    OffersComponent,
    CarouselComponent,
  ],
  imports: [CommonModule, PublicationsRoutingModule, SharedModule],
})
export class PublicationsModule {}
