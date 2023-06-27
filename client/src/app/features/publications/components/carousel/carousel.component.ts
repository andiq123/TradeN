import {
  AfterViewInit,
  Component,
  ElementRef,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { IPhoto } from '../../interfaces/photo.interface';
import Swiper, { Navigation, Scrollbar } from 'swiper';

@Component({
  selector: 'app-carousel',
  templateUrl: './carousel.component.html',
  styleUrls: ['./carousel.component.scss'],
})
export class CarouselComponent implements OnInit, AfterViewInit {
  @ViewChild('swiperContainer') swiperContainer!: ElementRef;
  @Input({ required: true }) photos: IPhoto[] = [];

  ngAfterViewInit() {
    Swiper.use([Navigation, Scrollbar]);

    const swiper = new Swiper(this.swiperContainer.nativeElement, {
      slidesPerView: 'auto',
      scrollbar: {
        el: '.swiper-scrollbar',
        hide: true,
      },
      navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
      },
    });
  }

  ngOnInit(): void {}
}
