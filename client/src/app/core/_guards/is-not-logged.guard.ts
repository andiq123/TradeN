import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs';
import { AuthService } from 'src/app/features/auth/services/auth.service';

export const isNotLoggedGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  return inject(AuthService).userLoggedIn$.pipe(
    map((user) => {
      if (user) {
        router.navigateByUrl('/home');
        return false;
      }
      return true;
    })
  );
};
