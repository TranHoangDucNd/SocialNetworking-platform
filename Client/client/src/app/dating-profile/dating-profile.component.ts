import {
  Component,
  ElementRef,
  OnInit,
  TemplateRef,
  ViewChild,
  inject,
} from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { DatingService } from '../_service/Dating.service';
import { EItem, UserInterest } from '../_models/DatingProfile';
import { MatChipInputEvent } from '@angular/material/chips';
import { LiveAnnouncer } from '@angular/cdk/a11y';
import { Observable, map, startWith } from 'rxjs';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { FloatLabelType } from '@angular/material/form-field';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dating-profile',
  templateUrl: './dating-profile.component.html',
  styleUrls: ['./dating-profile.component.css'],
})
export class DatingProfileComponent implements OnInit {
  @ViewChild('DatingProfileTemplate') DatingProfileTemplate!: TemplateRef<any>;
  @ViewChild('interestInput') interestInput!: ElementRef<any>;
  modalRef?: BsModalRef;

  UserInterests!: EItem[];
  ChooseUserInterests: EItem[] = [];
  ListUserInterests!: EItem[];
  announcer = inject(LiveAnnouncer);
  interestCtrl = new FormControl('');
  filteredInterest!: Observable<EItem[]>;
  separatorKeysCodes: number[] = [ENTER, COMMA];

  isFormValid = true;
  isAgeRangeValid = true;

  WhereToDates!: EItem[];
  Heights!: EItem[];
  DatingObjects!: EItem[];

  createdatingform = this.fb.group({
    datingAgeFrom: new FormControl(18, [Validators.min(18), Validators.max(99)]),
    datingAgeTo: new FormControl(99, [Validators.min(18), Validators.max(99)]),
    datingObject: [0, Validators.required],
    height: [0, Validators.required],
    whereToDate: [0, Validators.required],
    userInterests: [[], Validators.required],

  },{
    validators: [this.validateAgeRangeControlsValue('datingAgeFrom', 'datingAgeTo')
    ]
  });

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private modalService: BsModalService,
    private datingService: DatingService,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.GetDatingObject();
    this.GetHeight();
    this.GetWhereToDate();
    this.GetUserInterests();
    setTimeout(() => {
      this.filteredInterest = this.interestCtrl.valueChanges.pipe(
        startWith(),
        map((topic: string | null | EItem) =>
          typeof topic === 'string'
            ? this.ListUserInterests.filter((item) =>
              item.displayName.toLowerCase().includes(topic.toLowerCase())
            ).slice()
            : this._filterInterest(topic)
        )
      );

      this.createdatingform.valueChanges.subscribe((value) =>{
        this.isFormValid = this.createdatingform.valid;
        this.isAgeRangeValid = !this.createdatingform.hasError('Age range not valid');
      })

      this.openModal();
    }, 1000);
  }

  GetUserInterests() {
    this.datingService.GetUserInterests().subscribe(
      (data: any) => {
        this.UserInterests = data;
        this.ListUserInterests = this.UserInterests;
      },
      (error: any) => {
        console.log(error);
      }
    );
  }
  GetWhereToDate() {
    this.datingService.GetWhereToDate().subscribe(
      (data: any) => {
        this.WhereToDates = data;
      },
      (error: any) => {
        console.log(error);
      }
    );
  }
  GetHeight() {
    this.datingService.GetHeight().subscribe(
      (data: any) => {
        this.Heights = data;
      },
      (error: any) => {
        console.log(error);
      }
    );
  }

  GetDatingObject() {
    this.datingService.GetDatingObject().subscribe({
      next: (data: any[]) => {
        this.DatingObjects = data.filter(obj => obj.value >= 1 && obj.value <= 3);
      },
      error: (error: any) => {
        console.log(error);
      },
    });
  }

  openModal() {
    this.modalService.show(this.DatingProfileTemplate, {
      class: 'modal-lg',
      initialState: {
        modalIndex: 900,
      },
    });
  }

  selectedTopic(event: MatAutocompleteSelectedEvent): void {
    const value = event.option.value;
    if (!this.isDupplicationSelect(value)) {
      const selectedInterest = this.findInterestByName(value);
      if (selectedInterest != undefined) {
        this.ChooseUserInterests.push(selectedInterest);
        if (this.interestInput && this.interestInput.nativeElement) {
          this.interestInput.nativeElement.value = '';
        }
        this.interestCtrl.setValue(null);
      }
    }
  }

  addTopic(event: MatChipInputEvent): void {
    const value = event.value.trim();
    if (value) {
      const selectedInterest = this.findInterestByName(value);
      if (selectedInterest) {
        if (!this.isDupplication(selectedInterest)) {
          this.ChooseUserInterests.push(selectedInterest);
          console.log(this.ChooseUserInterests);
        }
      } else {
        console.warn(`Chủ đề "${value}" không tồn tại trong danh sách.`);
      }
    }
    event.chipInput!.clear();
    this.interestCtrl.setValue(null);
  }
  findInterestByName(name: string): EItem | undefined {
    return this.UserInterests.find((item) => item.displayName === name);
  }

  isDupplication(interest: EItem): boolean {
    return this.ChooseUserInterests.some(
      (item) => item.displayName === interest.displayName
    );
  }

  isDupplicationSelect(interest: string): boolean {
    return this.ChooseUserInterests.some(
      (item) => item.displayName === interest
    );
  }

  removeTopic(item: EItem): void {
    const index = this.ChooseUserInterests.indexOf(item);
    if (index >= 0) {
      this.ChooseUserInterests.splice(index, 1);
      this.announcer.announce(`Removed ${item.displayName}`);
    }
  }

  private _filterInterest(value: EItem | null): EItem[] {
    if (value == null) return [];
    const topicValue = value;
    return this.ListUserInterests.filter((interest) =>
      interest.displayName
        .toLowerCase()
        .includes(topicValue!.displayName.toLowerCase())
    );
  }

  updateDatingProfile() {
    const formData = new FormData();
    const formValue = this.createdatingform.getRawValue();

    formData.append('DatingObject', formValue.datingObject?.toString() || '');
    formData.append('Height', formValue.height?.toString() || '');
    formData.append('WhereToDate', formValue.whereToDate?.toString() || '');
    formData.append('DatingAgeFrom', formValue.datingAgeFrom?.toString() || '')
    formData.append('DatingAgeTo', formValue.datingAgeTo?.toString() || '')

    const userInterestsArray: UserInterest[] = [];
    for (let i = 0; i < this.ChooseUserInterests.length; i++) {
      const userInterest: UserInterest = {
        id: 0,
        datingProfileId: 0,
        interestName: this.ChooseUserInterests[i].value,
        interestNameCode: 0,
        interestType: 1
      };
      userInterestsArray.push(userInterest);
    }
    formData.append('UserInterests', JSON.stringify(userInterestsArray));

    this.datingService.updateDatingProfile(formData).subscribe(
      (data: any) => {
        this.modalService.hide();
        this.router.navigate(['/members']);
        this.toastr.success(
          'Congratulations! You have updated dating profile success'
        );
      },
      (error: any) => {
        console.log(error);
      }
    );
  }

  cancel() {
    this.modalService.hide();
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
