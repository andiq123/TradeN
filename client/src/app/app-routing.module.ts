import { Inject, NgModule, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Route, RouterModule } from '@angular/router';
import { HomeComponent } from './core/pages/home/home.component';
import { AuthService } from './features/auth/services/auth.service';
import { map, switchMap, take } from 'rxjs';
import { isNotLoggedGuard } from './core/_guards/is-not-logged.guard';
import { isLoggedGuard } from './core/_guards/is-logged.guard';

const routes: Route[] = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  {
    path: 'auth',
    loadChildren: () =>
      import('./features/auth/auth.module').then((m) => m.AuthModule),
    canActivate: [isNotLoggedGuard],
  },
  {
    path: 'publications',
    loadChildren: () =>
      import('./features/publications/publications.module').then(
        (m) => m.PublicationsModule
      ),
  },
  {
    path: 'users',
    loadChildren: () =>
      import('../../users/users.module').then((m) => m.UsersModule),
    canActivate: [isLoggedGuard],
  },
  {
    path: 'exchanges',
    loadChildren: () =>
      import('./features/exchanges/exchanges.module').then(
        (m) => m.ExchangesModule
      ),
    canActivate: [isLoggedGuard],
  },
];

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
