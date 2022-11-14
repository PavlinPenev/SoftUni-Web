import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserPageComponent } from './user-page/user-page.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { DashboardPageComponent } from './user-page/dashboard-page/dashboard-page.component';
import { RouterModule } from '@angular/router';
import { AddStoreModalComponent } from './user-page/dashboard-page/add-store-modal/add-store-modal.component';
import { StorePageComponent } from './store-page/store-page.component';
import { OrdersPageComponent } from './orders-page/orders-page.component';
import { AddOrderComponent } from './orders-page/add-order/add-order.component';

@NgModule({
  declarations: [
    UserPageComponent,
    DashboardPageComponent,
    AddStoreModalComponent,
    StorePageComponent,
    OrdersPageComponent,
    AddOrderComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {
        path: '',
        children: [
          { path: '', component: DashboardPageComponent },
          { path: 'store/:storeId', component: StorePageComponent },
          { path: 'store/:storeId/orders', component: OrdersPageComponent },
        ],
      },
    ]),
    SharedModule,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class UserModule {}
