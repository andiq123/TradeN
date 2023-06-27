import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject, tap } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { IOffer } from '../interfaces/offer.interface';
import { IPhoto } from '../interfaces/photo.interface';

@Injectable({
  providedIn: 'root',
})
export class OffersService {
  baseUrl = environment.baseApiUrl + 'offers';
  private offersChangedSource = new Subject<void>();
  offersChanged$ = this.offersChangedSource.asObservable();

  constructor(private http: HttpClient) {}

  public createOffer(
    publicationId: string,
    title: string,
    content: string,
    photos: IPhoto[]
  ) {
    return this.http
      .post(`${this.baseUrl}`, {
        title,
        content,
        publicationId,
        photos,
      })
      .pipe(tap(() => this.offersChangedSource.next()));
  }

  public removeOffer(id: string) {
    return this.http
      .delete(`${this.baseUrl}/${id}`)
      .pipe(tap(() => this.offersChangedSource.next()));
  }

  public getOffersByPublicationId(publicationId: string) {
    return this.http.get<IOffer[]>(`${this.baseUrl}/${publicationId}/all`);
  }

  public getOffersByUserIdForPublicationId(publicationId: string) {
    return this.http.get<IOffer[]>(`${this.baseUrl}/${publicationId}/my`);
  }
}
