import {ChangeDetectionStrategy, Component, inject, OnInit} from '@angular/core';
import {MatDialogModule, MatDialogRef} from "@angular/material/dialog";
import {MatButtonModule} from "@angular/material/button";
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule, ValidationErrors,
  ValidatorFn,
  Validators
} from "@angular/forms";
import {MatInputModule} from "@angular/material/input";
import {MatSelectModule} from "@angular/material/select";
import {MatIconModule} from "@angular/material/icon";
import {NgForOf, NgIf} from "@angular/common";

const FILTER_LOCAL_STORAGE_KEY = 'memberFilter';

@Component({
  selector: 'app-member-filter',
  templateUrl: './member-filter.component.html',
  styleUrls: ['./member-filter.component.css'],
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    MatDialogModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    NgForOf,
    NgIf,
  ]
})
export class MemberFilterComponent implements OnInit {
  _dialogRef = inject(MatDialogRef<MemberFilterComponent>);
  _formBuilder = inject(FormBuilder);

  isFormValid = true;
  isAgeRangeValid = true;

  genderList = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' },
  ];

  cities = [
    { value: 'new york', display: 'New York' },
    { value: 'london', display: 'London' },
    { value: 'paris', display: 'Paris' },
    { value: 'berlin', display: 'Berlin' },
  ];

  countries = [
    { value: 'usa', display: 'USA' },
    { value: 'uk', display: 'UK' },
    { value: 'france', display: 'France' },
    { value: 'germany', display: 'Germany' },
  ];

  filterForm = this._formBuilder.group({
    minAge: new FormControl(18, [Validators.min(18), Validators.max(99)]),
    maxAge: new FormControl(99, [Validators.min(18), Validators.max(99)]),
    gender: new FormControl(["female"]),
    city: new FormControl([]),
    country: new FormControl([]),
  }, {
    validators: this.validateAgeRangeControlsValue('minAge', 'maxAge'),
  });

  ngOnInit(): void {
    this.filterForm?.valueChanges.subscribe((value) => {
      this.isFormValid = this.filterForm?.valid;
      this.isAgeRangeValid = !this.filterForm?.hasError('ageRangeNotValid');
    });

    const filter = localStorage.getItem(FILTER_LOCAL_STORAGE_KEY);
    if (filter) {
      this.filterForm?.patchValue(JSON.parse(filter));
    }
  }

  resetFilter() {
    this.filterForm?.reset();
    localStorage.removeItem(FILTER_LOCAL_STORAGE_KEY);
  }

  applyFilter() {
    if (this.filterForm?.valid) {
      localStorage.setItem(FILTER_LOCAL_STORAGE_KEY, JSON.stringify(this.filterForm?.value));
      this._dialogRef.close(this.filterForm?.value);
    }
  }

  validateAgeRangeControlsValue(
    firstControlName: string,
    secondControlName: string
  ): ValidatorFn {
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    // @ts-expect-error
    return (formGroup: FormGroup): ValidationErrors | null => {
      const { value: firstControlValue } = formGroup.get(
        firstControlName
      ) as AbstractControl;
      const { value: secondControlValue } = formGroup.get(
        secondControlName
      ) as AbstractControl;
      return firstControlValue <= secondControlValue
        ? null
        : {
          ageRangeNotValid: {
            firstControlValue,
            secondControlValue,
          },
        };
    };
  }
}
