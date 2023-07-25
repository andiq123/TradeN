import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { IRating } from '../interfaces/rating.interface';

@Injectable({
  providedIn: 'root',
})
export class RatingsService {
  baseUrl = environment.baseApiUrl + 'ratings';
  constructor(private http: HttpClient) {}

  checkIfAvailable(publicationId: string) {
    return this.http.get<{ alreadyRated: boolean }>(
      `${this.baseUrl}/${publicationId}`
    );
  }

  setRating(request: IRating) {
    return this.http.post(`${this.baseUrl}`, request);
  }
}
