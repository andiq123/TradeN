import { Injectable } from '@angular/core';
import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  private loadingSource = new Subject<boolean>();
  public loading$ = this.loadingSource.asObservable();
  constructor() {}

  public show(): void {
    this.loadingSource.next(true);
  }

  public hide(): void {
    this.loadingSource.next(false);
  }
}
