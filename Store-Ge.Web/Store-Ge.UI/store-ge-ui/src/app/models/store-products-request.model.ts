export interface StoreProductsRequest {
  storeId: string;
  searchTerm: string;
  isDescending: boolean;
  skip: number;
  take: number;
}
