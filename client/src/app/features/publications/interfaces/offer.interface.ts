import { Base } from 'src/app/core/interfaces/base.interface';
import { IUser } from '../../users/interfaces/user.interface';
import { IPhoto } from './photo.interface';

export interface IOffer extends Base {
  title: string;
  content: string;
  offerDate: string;
  userId: string;
  user: IUser;
  photos?: IPhoto[];
  publicationId: string;
  rank: number;
}
