import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { IPhoto } from '../interfaces/photo.interface';

@Injectable({
  providedIn: 'root',
})
export class PhotosService {
  baseUrl = environment.baseApiUrl + 'photos';
  constructor(private httpClient: HttpClient) {}

  uploadPhoto(file: File) {
    const formData = new FormData();
    formData.append('file', file, file.name);
    return this.httpClient.post<IPhoto>(this.baseUrl, formData);
  }

  removePhoto(id: string, publicationId: string) {
    return this.httpClient.delete(`${this.baseUrl}/${id}/${publicationId}`);
  }
}
