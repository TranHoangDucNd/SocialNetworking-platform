import {ChangeDetectionStrategy, ChangeDetectorRef, Component, DestroyRef, inject, OnInit} from '@angular/core';
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
import {MembersService} from "../../_service/members.service";
import {takeUntilDestroyed} from "@angular/core/rxjs-interop";
import {UserParams} from "../../_models/userParams";
import {forkJoin, map} from "rxjs";

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
  _memberService = inject(MembersService);
  _destroyed = inject(DestroyRef);
  _cdr = inject(ChangeDetectorRef);

  isFormValid = true;
  isAgeRangeValid = true;
  userParams = this._memberService.getUserParams();

  genderList: any[] = [];

  provinces: any[] = [];

  filterForm = this._formBuilder.group({
    minAge: new FormControl(18, [Validators.min(18), Validators.max(99)]),
    maxAge: new FormControl(99, [Validators.min(18), Validators.max(99)]),
    minHeight: new FormControl(120, [Validators.min(120), Validators.max(200)]),
    maxHeight: new FormControl(200, [Validators.min(120), Validators.max(200)]),
    gender: new FormControl(2),
    province: new FormControl(0),
  }, {
    validators: [this.validateAgeRangeControlsValue('minAge', 'maxAge'),
      this.validateAgeRangeControlsValue('minHeight', 'maxHeight')
    ]
  });

  ngOnInit(): void {
    this.filterForm?.valueChanges
      .pipe(takeUntilDestroyed(this._destroyed))
      .subscribe((value) => {
      this.isFormValid = this.filterForm?.valid;
      this.isAgeRangeValid = !this.filterForm?.hasError('ageRangeNotValid');
    });

    this.setOptions()

    if (this.userParams) {
      this.filterForm?.patchValue(this.userParams);
    }
  }

  setOptions() {
    forkJoin([
      this._memberService.getGenders(),
      this._memberService.getProvinces()
    ]).subscribe({
      next: (response) => {
        if (response) {
          this.genderList = response[0];
          this.provinces = response[1];
          this._cdr.markForCheck();
        }
      }
    })
  }

  resetFilter() {
    this.filterForm?.reset();
    this._memberService.resetUserParams();
    const userParams = this._memberService.getUserParams();
    this.filterForm?.patchValue(this.userParams);
  }

  applyFilter() {
    if (this.filterForm.valid) {
      const userParams = this.filterForm?.value as UserParams;

      this._memberService.setUserParams(userParams);
      this._dialogRef.close(userParams);
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
