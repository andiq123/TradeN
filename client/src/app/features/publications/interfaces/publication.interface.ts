import { Base } from '../../../core/interfaces/base.interface';
import { IUser } from '../../../../../users/interfaces/user.interface';
import { IOffer } from './offer.interface';
import { IPhoto } from './photo.interface';

export interface IPublication extends Base {
  title: string;
  content: string;
  publishDate: Date;
  userId: string;
  user: IUser;
  photos?: IPhoto[];
  isAvailable: boolean;
}
