import { Component, Input, OnInit } from '@angular/core';
import { OffersService } from 'src/app/features/publications/services/offers.service';
import { PublicationsService } from 'src/app/features/publications/services/publications.service';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss'],
})
export class ModalComponent implements OnInit {
  @Input({ required: true }) title: string = '';
  @Input() addButtonStyles = false;
  isOpened = false;

  constructor(
    private publicationService: PublicationsService,
    private offersService: OffersService
  ) {}

  ngOnInit(): void {
    this.publicationService.publicationsChanged$.subscribe({
      next: () => (this.isOpened = false),
    });

    this.offersService.offersChanged$.subscribe({
      next: () => (this.isOpened = false),
    });
  }

  open() {
    this.isOpened = true;
  }

  close() {
    this.isOpened = false;
  }
}
