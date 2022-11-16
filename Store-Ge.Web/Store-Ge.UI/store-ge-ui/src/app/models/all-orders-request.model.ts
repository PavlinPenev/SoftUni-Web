export interface AllOrdersRequest {
  userId: string;
  searchTerm: string;
  orderBy: string;
  isDescending: boolean;
  dateAddedFrom: string | null;
  dateAddedTo: string | null;
  skip: number;
  take: number;
}
