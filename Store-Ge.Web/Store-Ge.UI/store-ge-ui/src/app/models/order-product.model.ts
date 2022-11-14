import { MeasurementUnitEnum } from './measurement-unit.enum';

export interface OrderProduct {
  name: string;
  measurementUnit: MeasurementUnitEnum;
  quantity: number;
  price: number;
}
