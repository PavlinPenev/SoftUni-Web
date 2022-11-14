import { AddProduct } from './add-product.model';

export interface AddOrderRequest {
  storeId: string;
  orderNumber: number;
  products: AddProduct[];
  supplierId: string;
}
