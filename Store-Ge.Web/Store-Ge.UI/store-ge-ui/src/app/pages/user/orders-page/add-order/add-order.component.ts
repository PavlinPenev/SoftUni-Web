import { Component, Inject, OnInit } from '@angular/core';
import {
  UntypedFormControl,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import {
  MatBottomSheetRef,
  MAT_BOTTOM_SHEET_DATA,
} from '@angular/material/bottom-sheet';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { debounceTime, filter, first } from 'rxjs';
import { AddOrderRequest } from 'src/app/models/add-order-request.model';
import { AddProduct } from 'src/app/models/add-product.model';
import { MeasurementUnitEnum } from 'src/app/models/measurement-unit.enum';
import { ProductMeasurementUnitConfiguration } from 'src/app/models/product-measurement-configuration.model';
import { Supplier } from 'src/app/models/supplier.model';
import { OrdersService } from 'src/app/services/orders.service';
import { ProductsService } from 'src/app/services/products.service';
import { SuppliersService } from 'src/app/services/suppliers.service';
import * as constants from 'src/assets/text.constants';

@Component({
  selector: 'app-add-order',
  templateUrl: './add-order.component.html',
  styleUrls: ['./add-order.component.scss'],
})
export class AddOrderComponent implements OnInit {
  constants = constants;

  orderProducts: AddProduct[] = [];
  products: AddProduct[] = [];
  suppliers: Supplier[] = [];

  productMeasurementUnitConfiguration: ProductMeasurementUnitConfiguration[] = [
    { name: 'SingularPiece', value: 0 },
    { name: 'Kilogram', value: 1 },
    { name: 'Litre', value: 2 },
    { name: 'Metre', value: 3 },
  ];

  supplierForm = new UntypedFormControl('', Validators.required);
  orderForm = new UntypedFormGroup({
    orderNumber: new UntypedFormControl('', Validators.required),
    existingProduct: new UntypedFormControl(''),
    addQuantity: new UntypedFormControl(''),
    newProductName: new UntypedFormControl('', [
      Validators.maxLength(constants.PRODUCT_NAME_MAX_LENGTH),
      Validators.required,
    ]),
    newProductPrice: new UntypedFormControl('', Validators.required),
    newProductQuantity: new UntypedFormControl('', Validators.required),
    newProductMeasurementUnit: new UntypedFormControl(
      this.productMeasurementUnitConfiguration[0],
      Validators.required
    ),
  });

  constructor(
    private router: Router,
    private ordersService: OrdersService,
    private suppliersService: SuppliersService,
    private productsService: ProductsService,
    public bottomSheetRef: MatBottomSheetRef<AddOrderComponent>,
    @Inject(MAT_BOTTOM_SHEET_DATA)
    public data: { userId: string; storeId: string }
  ) {}

  ngOnInit(): void {
    this.suppliersService
      .getUserSuppliers(this.data.userId)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => (this.suppliers = response));

    this.productsService
      .getStoreAddProducts(this.data.storeId)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => (this.products = response));
  }

  addProductToList(): void {
    const product: AddProduct =
      this.orderForm.controls['existingProduct'].value;
    const plusQuantity = this.orderForm.controls['addQuantity'].value;

    const plusQuantityNumber = Number(plusQuantity);
    product.plusQuantity = plusQuantityNumber;

    this.orderProducts.push(product);

    this.orderForm.controls['existingProduct'].setValue('');
  }

  addNewProductToList(): void {
    const newProductToAdd: AddProduct = {
      id: null,
      name: this.orderForm.controls['newProductName'].value,
      quantity: this.orderForm.controls['newProductQuantity'].value,
      price: this.orderForm.controls['newProductPrice'].value,
      measurementUnit:
        this.orderForm.controls['newProductMeasurementUnit'].value.value,
      plusQuantity: null,
    };

    this.orderProducts.push(newProductToAdd);
  }

  isNewProductFormValid(): boolean {
    return (
      this.orderForm.controls['newProductName'].valid &&
      this.orderForm.controls['newProductQuantity'].valid &&
      this.orderForm.controls['newProductPrice'].valid &&
      this.orderForm.controls['newProductMeasurementUnit'].valid
    );
  }

  addOrder(event: MouseEvent): void {
    const request: AddOrderRequest = {
      orderNumber: this.orderForm.controls['orderNumber'].value,
      storeId: this.data.storeId,
      products: this.orderProducts,
      supplierId: this.supplierForm.value.id,
    };

    this.bottomSheetRef.dismiss();
    event.preventDefault();

    this.ordersService
      .addOrder(request)
      .pipe(
        debounceTime(500),
        filter((x) => !!x),
        first()
      )
      .subscribe();

    let currentUrl = this.router.url;

    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl]);
    });
  }

  getUnitTypeString(measurementUnit: MeasurementUnitEnum): string {
    let enumStringLiteral: string = '';

    switch (MeasurementUnitEnum[measurementUnit]) {
      case 'SingularPiece':
        enumStringLiteral = 'pieces';
        break;
      case 'Kilogram':
        enumStringLiteral = 'kg';
        break;
      case 'Litre':
        enumStringLiteral = 'l';
        break;
      case 'Metre':
        enumStringLiteral = 'm';
        break;
    }

    return enumStringLiteral;
  }

  navigateToStoreSuppliers(): void {
    this.bottomSheetRef.dismiss();

    this.router.navigate([
      '/user',
      this.data.userId,
      'store',
      this.data.storeId,
      'suppliers',
    ]);
  }
}
