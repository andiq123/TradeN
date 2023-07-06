import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsersRoutingModule } from './users-routing.module';
import { ProfileComponent } from './pages/profile/profile.component';
import { GuestComponent } from './pages/guest/guest.component';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [ProfileComponent, GuestComponent],
  imports: [CommonModule, UsersRoutingModule, SharedModule],
})
export class UsersModule {}
