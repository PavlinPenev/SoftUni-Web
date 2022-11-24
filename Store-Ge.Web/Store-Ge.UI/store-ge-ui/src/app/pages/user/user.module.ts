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
import { SuppliersPageComponent } from './suppliers-page/suppliers-page.component';
import { AddSupplierComponent } from './suppliers-page/add-supplier/add-supplier.component';
import { AllOrdersComponent } from './all-orders/all-orders.component';
import { AccountSettingsPageComponent } from './account-settings-page/account-settings-page.component';
import { SalesPageComponent } from './sales-page/sales-page.component';
import { AddCashierComponent } from './sales-page/add-cashier/add-cashier.component';

@NgModule({
  declarations: [
    UserPageComponent,
    DashboardPageComponent,
    AddStoreModalComponent,
    StorePageComponent,
    OrdersPageComponent,
    AddOrderComponent,
    SuppliersPageComponent,
    AddSupplierComponent,
    AllOrdersComponent,
    AccountSettingsPageComponent,
    SalesPageComponent,
    AddCashierComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {
        path: '',
        children: [
          { path: '', component: DashboardPageComponent },
          { path: 'account-settings', component: AccountSettingsPageComponent },
          { path: 'all-orders', component: AllOrdersComponent },
          { path: 'store/:storeId', component: StorePageComponent },
          { path: 'store/:storeId/orders', component: OrdersPageComponent },
          {
            path: 'store/:storeId/suppliers',
            component: SuppliersPageComponent,
          },
          { path: 'store/:storeId/sales', component: SalesPageComponent },
        ],
      },
    ]),
    SharedModule,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class UserModule {}
