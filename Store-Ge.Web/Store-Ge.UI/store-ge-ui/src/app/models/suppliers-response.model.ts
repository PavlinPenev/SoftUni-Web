import { UserSupplier } from './user-supplier.model';

export interface SuppliersResponse {
  items: UserSupplier[];
  totalItemsCount: number;
}
