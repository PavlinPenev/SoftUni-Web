export interface OrdersRequest {
  storeId: string;
  searchTerm: string;
  orderBy: string;
  isDescending: boolean;
  dateAddedFrom: string | null;
  dateAddedTo: string | null;
  skip: number;
  take: number;
}
