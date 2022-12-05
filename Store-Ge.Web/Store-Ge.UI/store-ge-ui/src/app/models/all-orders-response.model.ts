import { AllOrder } from './all-order.model';

export interface AllOrdersResponse {
  items: AllOrder[];
  totalItemsCount: number;
}
