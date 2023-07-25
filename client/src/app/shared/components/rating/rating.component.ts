import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-rating',
  templateUrl: './rating.component.html',
  styleUrls: ['./rating.component.scss'],
})
export class RatingComponent {
  @Input() rating: number = 0.5;
  @Input() name: string = '';
  @Output() rateChange: EventEmitter<number> = new EventEmitter();

  onRate(rating: number) {
    this.rating = rating;
    this.rateChange.emit(rating);
  }
}
