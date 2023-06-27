import { Base } from '../../src/app/core/interfaces/base.interface';

export interface IUser extends Base {
  username: string;
  email: string;
  fullName: string;
  location: string;
  phoneNumber: string;
  photoUrl?: string;
  rating?: number;
}
