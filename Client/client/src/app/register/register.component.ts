import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_service/account.service';
import {
  AbstractControl,
  FormBuilder,
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

  cities: string[] = [
    'Hà Nội',
    'Hồ Chí Minh',
    'Đà Nẵng',
    'Hải Phòng',
    'Cần Thơ',
    'Nghệ An',
    'Thanh Hóa',
    'Bắc Ninh',
    'Bảo Lộc',
    'Biên Hòa',
    'Bến Tre',
    'Buôn Ma Thuột',
    'Cà Mau',
    'Cẩm Phả',
    'Cao Lãnh',
    'Đà Lạt',
    'Điện Biên Phủ',
    'Đông Hà',
    'Đồng Hới',
    'Hà Tĩnh',
    'Hạ Long',
    'Hải Dương',
    'Hòa Bình',
    'Hội An',
    'Huế',
    'Hưng Yên',
    'Kon Tum',
    'Lạng Sơn',
    'Lào Cai',
    'Long Xuyên',
    'Móng Cái',
    'Mỹ Tho',
    'Nam Định',
    'Ninh Bình',
    'Nha Trang',
    'Phan Rang-Tháp Chàm',
    'Phan Thiết',
    'Phủ Lý',
    'Pleiku',
    'Quảng Ngãi',
    'Quy Nhơn',
    'Rạch Giá',
    'Sóc Trăng',
    'Sơn La',
    'Tam Kỳ',
    'Tân An',
    'Thái Bình',
    'Thái Nguyên',
    'Trà Vinh',
    'Tuy Hòa',
    'Tuyên Quang',
    'Uông Bí',
    'Việt Trì',
    'Vĩnh Yên',
    'Vĩnh Long',
    'Vũng Tàu',
    'Yên Bái'
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
