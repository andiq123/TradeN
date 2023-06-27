import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  Observable,
  ReplaySubject,
  catchError,
  of,
  switchMap,
  tap,
} from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { IAuthResponse } from '../interfaces/iauth-response.interface';
import { IRegisterRequest } from '../interfaces/register-request.interface';
import { ILoginRequest } from '../interfaces/login-request.interface';
import { IUser } from '../../../../../users/interfaces/user.interface';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  url = environment.baseApiUrl + 'auth/';
  private userLoggedInSource = new ReplaySubject<IUser | null>(1);
  userLoggedIn$ = this.userLoggedInSource.asObservable();

  constructor(private http: HttpClient) {
    this.setLoggedUser().subscribe();
  }

  setLoggedUser(): Observable<IUser | null> {
    const token = localStorage.getItem('token');

    if (!token) {
      this.logOut();
      return of(null);
    }

    return this.http.get<IUser>(this.url).pipe(
      catchError((err) => {
        this.logOut();
        throw err;
      }),
      tap((user: IUser) => {
        this.userLoggedInSource.next(user);
      })
    );
  }

  login(request: ILoginRequest) {
    return this.http.post<IAuthResponse>(this.url + 'login', request).pipe(
      tap((res: IAuthResponse) => {
        localStorage.setItem('token', res.token);
      }),
      switchMap(() => this.setLoggedUser())
    );
  }

  register(request: IRegisterRequest) {
    return this.http.post<IAuthResponse>(this.url + 'register', request).pipe(
      tap((res: IAuthResponse) => {
        localStorage.setItem('token', res.token);
      }),
      switchMap(() => this.setLoggedUser())
    );
  }

  logOut() {
    localStorage.removeItem('token');
    this.userLoggedInSource.next(null);
  }
}
