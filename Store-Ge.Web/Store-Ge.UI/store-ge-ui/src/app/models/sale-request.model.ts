import { AddProduct } from './add-product.model';

export interface SaleRequest {
  storeId: string;
  products: AddProduct[];
}
