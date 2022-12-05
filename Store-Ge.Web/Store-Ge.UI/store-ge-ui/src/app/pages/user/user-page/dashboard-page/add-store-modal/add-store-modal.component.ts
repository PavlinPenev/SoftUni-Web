import { Component, EventEmitter, Output } from '@angular/core';
import {
  UntypedFormControl,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { MatBottomSheetRef } from '@angular/material/bottom-sheet';
import { Router } from '@angular/router';
import { debounceTime, filter, first } from 'rxjs';
import { AddStoreRequest } from 'src/app/models/add-store-request.model';
import { StoreType } from 'src/app/models/store-type.model';
import { StoresService } from 'src/app/services/stores.service';
import * as constants from 'src/assets/text.constants';

@Component({
  selector: 'app-add-store-modal',
  templateUrl: './add-store-modal.component.html',
  styleUrls: ['./add-store-modal.component.scss'],
})
export class AddStoreModalComponent {
  @Output() addStore: EventEmitter<AddStoreRequest> =
    new EventEmitter<AddStoreRequest>();

  constants = constants;

  form = new UntypedFormGroup({
    userId: new UntypedFormControl(''),
    name: new UntypedFormControl('', [
      Validators.required,
      Validators.maxLength(constants.STORE_NAME_MAX_LENGTH),
    ]),
    type: new UntypedFormControl('', [Validators.required]),
  });

  storeTypeConfiguration: StoreType[] = [
    { name: 'Supermarket', value: 0 },
    { name: 'ClothesShop', value: 1 },
    { name: 'Kitchenware', value: 2 },
    { name: 'Pharmacy', value: 3 },
    { name: 'ConstructionMaterials', value: 4 },
    { name: 'Toys', value: 5 },
    { name: 'Other', value: 6 },
  ];

  constructor(
    private router: Router,
    private storesService: StoresService,
    private bottomSheetRef: MatBottomSheetRef<AddStoreModalComponent>
  ) {
    this.form.get('userId')?.setValue(this.router.url.replace('/user/', ''));
  }

  onAddStore(event: MouseEvent): void {
    const request: AddStoreRequest = {
      userId: this.form.get('userId')!.value,
      name: this.form.get('name')!.value,
      type: this.form.get('type')!.value,
    };

    this.bottomSheetRef.dismiss();
    event.preventDefault();

    this.storesService
      .addStore(request)
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
