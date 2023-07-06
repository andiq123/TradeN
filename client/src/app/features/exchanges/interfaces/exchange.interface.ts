import { IUser } from 'src/app/features/users/interfaces/user.interface';
import { IPublication } from '../../publications/interfaces/publication.interface';

export interface IExchange {
  id: string;
  publicationId: string;
  publication: IPublication;
  authorId: string;
  author: IUser;
  offerUserId: string;
  offerUser: IUser;
  exchangeDate: Date;
}
