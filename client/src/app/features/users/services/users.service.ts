import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { IUser } from '../interfaces/user.interface';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  baseUrl = environment.baseApiUrl;
  constructor(private http: HttpClient) {}

  getUserById(id: string) {
    return this.http.get<IUser>(`${this.baseUrl}users/${id}`);
  }
}
