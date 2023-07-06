import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class AiService {
  baseUrl = environment.baseApiUrl + 'ai';
  constructor(private http: HttpClient) {}

  resumeContent(query: string) {
    return this.http.post<string>(this.baseUrl, { query }).pipe(
      tap((data) => {
        console.log(data);
      })
    );
  }

  reorderContent(publicationId: string) {
    return this.http.get(`${this.baseUrl}/${publicationId}`);
  }
}
