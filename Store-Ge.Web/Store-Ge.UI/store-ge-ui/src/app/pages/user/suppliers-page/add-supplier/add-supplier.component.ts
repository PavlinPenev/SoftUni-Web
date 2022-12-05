import { Component, Inject, OnInit } from '@angular/core';
import { UntypedFormControl, Validators } from '@angular/forms';
import {
  MatBottomSheetRef,
  MAT_BOTTOM_SHEET_DATA,
} from '@angular/material/bottom-sheet';
import { Router } from '@angular/router';
import { debounceTime, filter, first } from 'rxjs';
import { AddSupplierRequest } from 'src/app/models/add-supplier-request.model';
import { SuppliersService } from 'src/app/services/suppliers.service';
import * as constants from 'src/assets/text.constants';

@Component({
  selector: 'app-add-supplier',
  templateUrl: './add-supplier.component.html',
  styleUrls: ['./add-supplier.component.scss'],
})
export class AddSupplierComponent implements OnInit {
  constants = constants;

  form = new UntypedFormControl('', [
    Validators.required,
    Validators.maxLength(constants.SUPPLIER_NAME_MAX_LENGTH),
  ]);

  constructor(
    private router: Router,
    private bottomSheetRef: MatBottomSheetRef<AddSupplierComponent>,
    private suppliersService: SuppliersService,
    @Inject(MAT_BOTTOM_SHEET_DATA)
    public data: { userId: string }
  ) {}

  ngOnInit(): void {}

  onAddSupplier(event: MouseEvent): void {
    const request: AddSupplierRequest = {
      name: this.form.value,
      userId: this.data.userId,
    };

    this.bottomSheetRef.dismiss();
    event.preventDefault();

    this.suppliersService
      .addSupplier(request)
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
}
