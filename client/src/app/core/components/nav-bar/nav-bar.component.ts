import { Component } from '@angular/core';
import { Observable, of } from 'rxjs';

import { AuthService } from 'src/app/features/auth/services/auth.service';
import { IUser } from 'src/app/features/users/interfaces/user.interface';
import { ImageUtils } from 'src/app/shared/utils/image.utils';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
})
export class NavBarComponent {
  userLoggedIn$: Observable<IUser | null>;

  constructor(private authService: AuthService) {
    this.userLoggedIn$ = this.authService.userLoggedIn$;
  }

  logout() {
    this.authService.logOut();
  }

  replaceIfAbsent(photoUrl?: string) {
    return ImageUtils.replaceIfAbsent(photoUrl);
  }
}
