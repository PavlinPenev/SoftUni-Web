import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserPageComponent } from './user-page/user-page.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { DashboardPageComponent } from './user-page/dashboard-page/dashboard-page.component';
import { RouterModule } from '@angular/router';
import { AddStoreModalComponent } from './user-page/dashboard-page/add-store-modal/add-store-modal.component';
import { StorePageComponent } from './store-page/store-page.component';

@NgModule({
  declarations: [
    UserPageComponent,
    DashboardPageComponent,
    AddStoreModalComponent,
    StorePageComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {
        path: '',
        children: [
          { path: '', component: DashboardPageComponent },
          { path: 'store/:storeId', component: StorePageComponent },
        ],
      },
    ]),
    SharedModule,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class UserModule {}
