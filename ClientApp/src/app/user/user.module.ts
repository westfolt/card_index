import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { MatMenuModule } from '@angular/material/menu';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatSelectModule} from '@angular/material/select';

import { UserRoutingModule } from './user-routing.module';
import { UserListComponent } from './user-list/user-list.component';
import { UserDeleteComponent } from './user-delete/user-delete.component';
import { UserUpdateComponent } from './user-update/user-update.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { UserCabinetComponent } from './user-cabinet/user-cabinet.component';


@NgModule({
  declarations: [
    UserListComponent,
    UserDeleteComponent,
    UserUpdateComponent,
    UserDetailComponent,
    UserCabinetComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    UserRoutingModule,
    BsDatepickerModule.forRoot(),
    MatMenuModule,
    MatFormFieldModule,
    MatSelectModule
  ]
})
export class UserModule { }
