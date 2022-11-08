import { MeasurementUnitEnum } from './measurement-unit.enum';

export interface Product {
  id: string;
  name: string;
  measurementUnit: MeasurementUnitEnum;
  quantity: number;
  price: number;
}
