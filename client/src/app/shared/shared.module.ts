import { NgModule } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { RatingComponent } from './components/rating/rating.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CustomInputComponent } from './components/custom-input/custom-input.component';
import { ModalComponent } from './components/modal/modal.component';
import { AddPublicationComponent } from './components/add-publication/add-publication.component';

@NgModule({
  declarations: [
    RatingComponent,
    CustomInputComponent,
    ModalComponent,
    AddPublicationComponent,
  ],
  imports: [CommonModule, ReactiveFormsModule, NgOptimizedImage],
  exports: [
    RatingComponent,
    ReactiveFormsModule,
    CustomInputComponent,
    ModalComponent,
    NgOptimizedImage,
    AddPublicationComponent,
  ],
})
export class SharedModule {}
