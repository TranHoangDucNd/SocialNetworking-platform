import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_service/account.service';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit{

  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup = new FormGroup({});
  maxDate: Date = new Date();
  validationErrors:string[] | undefined;

  constructor(
    private accountService: AccountService,
    private fb: FormBuilder,
    private router: Router
  ){}

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm(){
    this.registerForm = this.fb.group({
      gender: ['male'],
      userName: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(15),
        createPasswordStrengthValidator()
      ]],
      confirmPassword: ['', [
        Validators.required,
        this.matchValues('password')
      ]]
    })

    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }

  matchValues(matchTo: string): ValidatorFn{
    return (control: AbstractControl) =>{
      return control.value === control.parent?.get(matchTo)?.value
        ? null
        : {notMatching: true}
    };
  }

  register(){
    const dob = this.getDateOnly(this.getDateOnly(this.registerForm.controls['dateOfBirth'].value))
    const values = {...this.registerForm.value, dateOfBirth: dob}
    this.accountService.register(values).subscribe({
      next: () =>{
        this.router.navigateByUrl('/members')
      },
      error: (error)=>{
        this.validationErrors = error
      },
    })
  }

  cancel(){
    this.cancelRegister.emit(false);
  }

  private getDateOnly(dob: string | undefined){
    if(!dob) return;

    let theDob = new Date(dob);

    return new Date(theDob.setMinutes(theDob.getMinutes() - theDob.getTimezoneOffset())).toISOString().slice(0,10);
  }

}

export function createPasswordStrengthValidator(): ValidatorFn {
  return (control:AbstractControl) : ValidationErrors | null => {

      const value = control.value;

      if (!value) {
          return null;
      }

      const hasUpperCase = /[A-Z]+/.test(value);

      const hasLowerCase = /[a-z]+/.test(value);

      const hasNumeric = /[0-9]+/.test(value);

      const passwordValid = hasUpperCase && hasLowerCase && hasNumeric;

      return !passwordValid ? {passwordStrength:true}: null;
  }
}
