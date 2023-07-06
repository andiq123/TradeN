import { EventEmitter, Injectable } from '@angular/core';
import { IPublication } from '../interfaces/publication.interface';
import { Observable, Subject, switchMap, tap } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { IPhoto } from '../interfaces/photo.interface';
import { AiService } from '../../open-ai-api/services/ai.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class PublicationsService {
  baseUrl = environment.baseApiUrl + 'publications';
  private publicationsChangedSource = new Subject<void>();
  publicationsChanged$ = this.publicationsChangedSource.asObservable();

  constructor(
    private http: HttpClient,
    private aiService: AiService,
    private toastr: ToastrService
  ) {}

  public getPublications(params: any): Observable<IPublication[]> {
    return this.http.get<IPublication[]>(`${this.baseUrl}`, { params });
  }

  public getPublicationById(id: string): Observable<IPublication> {
    return this.http.get<IPublication>(`${this.baseUrl}/${id}`);
  }

  public createPublication(
    title: string,
    content: string,
    desiredItem: string,
    photos: IPhoto[]
  ): Observable<IPublication> {
    this.toastr.info('Rezumam contentul folosind AI...');
    return this.aiService.resumeContent(content).pipe(
      switchMap((data: any) => {
        return this.http
          .post<IPublication>(`${this.baseUrl}`, {
            title,
            content,
            desiredItem,
            contentResumed: data.result,
            photos,
          })
          .pipe(
            tap(() => {
              this.toastr.success('Publicat cu succes!');
              this.publicationsChangedSource.next();
            })
          );
      })
    );
  }

  public updatePublication(
    id: string | undefined,
    title: string,
    content: string,
    photos: IPhoto[]
  ): Observable<IPublication> {
    return this.http
      .put<IPublication>(`${this.baseUrl}/${id}`, {
        title,
        content,
        photos,
      })
      .pipe(tap(() => this.publicationsChangedSource.next()));
  }

  public removePublication(id: string): Observable<void> {
    return this.http
      .delete<void>(`${this.baseUrl}/${id}`)
      .pipe(tap(() => this.publicationsChangedSource.next()));
  }
}
