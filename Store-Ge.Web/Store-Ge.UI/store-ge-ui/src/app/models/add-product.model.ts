import { MeasurementUnitEnum } from './measurement-unit.enum';

export interface AddProduct {
  id: string | null;
  name: string;
  measurementUnit: MeasurementUnitEnum;
  quantity: number;
  plusQuantity: number | null;
  price: number;
}
