import { Component, Input } from '@angular/core';
import { IUser } from 'src/app/features/users/interfaces/user.interface';
import { ImageUtils } from 'src/app/shared/utils/image.utils';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.scss'],
})
export class UserInfoComponent {
  @Input() user: IUser | null = null;
  @Input() fullVersion = true;

  replaceIfAbsent(photo?: string) {
    return ImageUtils.replaceIfAbsent(photo);
  }
}
