import { Product } from './product.model';

export interface ProductsResponse {
  items: Product[];
  totalItemsCount: number;
}
