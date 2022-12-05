import { StoreTypeEnum } from './store-type.enum';

export interface Store {
  id: string;
  name: string;
  type: StoreTypeEnum;
}
