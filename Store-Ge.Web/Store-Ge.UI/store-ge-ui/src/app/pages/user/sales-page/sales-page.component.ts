import { Component, OnInit } from '@angular/core';
import * as constants from 'src/assets/text.constants';
import { Location } from '@angular/common';
import { AddProduct } from 'src/app/models/add-product.model';
import { UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import { MeasurementUnitEnum } from 'src/app/models/measurement-unit.enum';
import { ProductsService } from 'src/app/services/products.service';
import { filter, first } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { SaleRequest } from 'src/app/models/sale-request.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { AddCashierComponent } from './add-cashier/add-cashier.component';

@Component({
  selector: 'app-sales-page',
  templateUrl: './sales-page.component.html',
  styleUrls: ['./sales-page.component.scss'],
})
export class SalesPageComponent implements OnInit {
  constants = constants;
  saleProducts: AddProduct[] = [];
  products: AddProduct[] = [];
  storeId: string = '';

  saleForm: UntypedFormGroup = new UntypedFormGroup({
    existingProduct: new UntypedFormControl(''),
    sellQuantity: new UntypedFormControl(''),
  });

  constructor(
    private location: Location,
    private productsService: ProductsService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private bottomSheet: MatBottomSheet
  ) {}

  ngOnInit(): void {
    this.storeId = this.route.snapshot.params['storeId'];

    this.productsService
      .getStoreAddProducts(this.storeId)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => (this.products = response));
  }

  addProductToList(): void {
    const product: AddProduct = this.saleForm.controls['existingProduct'].value;
    const plusQuantity = this.saleForm.controls['sellQuantity'].value;

    const plusQuantityNumber = Number(plusQuantity);
    product.plusQuantity = plusQuantityNumber;

    this.saleProducts.push(product);

    this.saleForm.controls['existingProduct'].setValue('');
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

  getBillValue(): number {
    let sum = 0;

    for (const product of this.saleProducts) {
      sum += product.price * product.plusQuantity!;
    }

    return sum;
  }

  makeASale(): void {
    const request: SaleRequest = {
      storeId: this.storeId,
      products: this.saleProducts,
    };

    this.productsService
      .sellProducts(request)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => {
        if (response) {
          this.saleProducts = [];
        } else {
          this.snackBar.open(constants.SOMETHING_WENT_WRONG, constants.CLOSE, {
            horizontalPosition: 'center',
            verticalPosition: 'bottom',
          });
        }
      });
  }

  addCashier(): void {
    this.bottomSheet.open(AddCashierComponent, {
      data: { storeId: this.storeId },
    });
  }

  goBack(): void {
    this.location.back();
  }
}
