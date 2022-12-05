import { Order } from './order.model';

export interface OrdersResponse {
  items: Order[];
  totalItemsCount: number;
}
