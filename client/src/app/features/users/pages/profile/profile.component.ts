import { Component, OnInit } from '@angular/core';
import { IUser } from '../../interfaces/user.interface';
import { Observable, of } from 'rxjs';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { ImageUtils } from 'src/app/shared/utils/image.utils';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  user$: Observable<IUser | null> = of(null);

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.user$ = this.authService.userLoggedIn$;
  }

  replaceIfAbsent(photo?: string) {
    return ImageUtils.replaceIfAbsent(photo);
  }
}
