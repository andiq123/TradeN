import { Component, OnInit } from '@angular/core';
import { UsersService } from '../../services/users.service';
import { ActivatedRoute } from '@angular/router';
import { IUser } from '../../interfaces/user.interface';
import { ImageUtils } from 'src/app/shared/utils/image.utils';

@Component({
  selector: 'app-guest',
  templateUrl: './guest.component.html',
  styleUrls: ['./guest.component.scss'],
})
export class GuestComponent implements OnInit {
  user!: IUser;
  constructor(
    private usersService: UsersService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(({ id }: any) => {
      this.usersService.getUserById(id).subscribe((user) => {
        this.user = user;
        console.log(this.user);
      });
    });
  }

  replaceIfAbsent(photoUrl: string) {
    return ImageUtils.replaceIfAbsent(photoUrl);
  }
}
