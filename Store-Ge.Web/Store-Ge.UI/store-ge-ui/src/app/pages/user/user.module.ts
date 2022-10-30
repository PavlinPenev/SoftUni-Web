import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserPageComponent } from './user-page/user-page.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { DashboardPageComponent } from './user-page/dashboard-page/dashboard-page.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [UserPageComponent, DashboardPageComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {
        path: '',
        children: [{ path: '', component: DashboardPageComponent }],
      },
    ]),
    SharedModule,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class UserModule {}
