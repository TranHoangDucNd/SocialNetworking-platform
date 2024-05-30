import { Component, DestroyRef, EventEmitter, inject, OnInit, Output } from '@angular/core';
import { AccountService } from '../_service/account.service';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap/modal';
import { DatingProfileComponent } from '../dating-profile/dating-profile.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup = new FormGroup({});
  maxDate: Date = new Date();
  validationErrors: string[] | undefined;
  _destroyed = inject(DestroyRef);

  isFormValid = true;
  isAgeRangeValid = true;
  constructor(
    private accountService: AccountService,
    private fb: FormBuilder,
    private router: Router,
    private modalService: BsModalService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      userName: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      height: new FormControl(140, [Validators.min(140), Validators.max(190)]),
      weight: new FormControl(40, [Validators.min(40), Validators.max(100)]),
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(15),
          createPasswordStrengthValidator(),
        ],
      ],
      confirmPassword: [
        '',
        [Validators.required, this.matchValues('password')],
      ],

      isUpdatedDatingProfile: [false, Validators.required],
    }, {
      validators: [this.validateHeightWeightRange('height', 'weight')
        
      ]
    });

    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () =>
        this.registerForm.controls['confirmPassword'].updateValueAndValidity(),
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value
        ? null
        : { notMatching: true };
    };
  }

  register() {
    const dob = this.getDateOnly(
      this.getDateOnly(this.registerForm.controls['dateOfBirth'].value)
    );
    const values = { ...this.registerForm.value, dateOfBirth: dob };
    this.accountService.register(values).subscribe({
      next: () => {
        this.router.navigateByUrl('/members');
      },
      error: (error) => {
        this.validationErrors = error;
      },
    });
  }

  validateHeightWeightRange(heightControlName: string, weightControlName: string) {
    return (formGroup: FormGroup) => {
      const heightControl = formGroup.controls[heightControlName];
      const weightControl = formGroup.controls[weightControlName];

      if (!heightControl || !weightControl) {
        return null;
      }

      if (heightControl.value > 190 || heightControl.value < 140) {
        heightControl.setErrors({ notValid: true });
      } else {
        heightControl.setErrors(null);
      }

      if (weightControl.value > 100 || weightControl.value < 40) {
        weightControl.setErrors({ notValid: true });
      } else {
        weightControl.setErrors(null);
      }

      return null;
    };
  }



  cancel() {
    this.cancelRegister.emit(false);
  }

  private getDateOnly(dob: string | undefined) {
    if (!dob) return;

    let theDob = new Date(dob);

    return new Date(
      theDob.setMinutes(theDob.getMinutes() - theDob.getTimezoneOffset())
    )
      .toISOString()
      .slice(0, 10);
  }

  citiesInVietnam: string[] = [
    "TP. Hà Nội",
    "TP. Hồ Chí Minh",
    "TP. Đà Nẵng",
    "TP. Hải Phòng",
    "TP. Cần Thơ",
    "TP. Nha Trang",
    "TP. Huế",
    "TP. Vũng Tàu",
    "TP. Quy Nhơn",
    "TP. Đà Lạt",
    "TP. Buôn Ma Thuột",
    "TP. Thanh Hóa",
    "TP. Vinh",
    "TP. Nam Định",
    "TP. Thái Nguyên",
    "TP. Phan Thiết",
    "TP. Rạch Giá",
    "TP. Long Xuyên",
    "TP. Thủ Dầu Một",
    "TP. Bạc Liêu",
    "TP. Trà Vinh",
    "TP. Cao Lãnh",
    "TP. Bắc Ninh",
    "TP. Tuy Hòa",
    "TP. Bến Tre",
    "TP. Tân An",
    "TP. Hà Tĩnh",
    "TP. Việt Trì",
    "TP. Lào Cai",
    "TP. Yên Bái",
    "TP. Điện Biên Phủ",
    "TP. Sơn La",
    "TP. Lạng Sơn",
    "TP. Hạ Long",
    "TP. Móng Cái",
    "TP. Bắc Giang",
    "TP. Phủ Lý",
    "TP. Hưng Yên",
    "TP. Thái Bình",
    "TP. Ninh Bình",
    "TP. Tam Kỳ",
    "TP. Quảng Ngãi",
    "TP. Pleiku",
    "TP. Kon Tum",
    "TP. Sóc Trăng",
    "TP. Vị Thanh",
    "TP. Châu Đốc",
    "TP. Sa Đéc",
    "TP. Hồng Ngự"
];
}

export function createPasswordStrengthValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    if (!value) {
      return null;
    }

    const hasUpperCase = /[A-Z]+/.test(value);

    const hasLowerCase = /[a-z]+/.test(value);

    const hasNumeric = /[0-9]+/.test(value);

    const passwordValid = hasUpperCase && hasLowerCase && hasNumeric;

    return !passwordValid ? { passwordStrength: true } : null;
  };

  
}
