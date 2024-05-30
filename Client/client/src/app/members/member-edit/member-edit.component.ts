import {
  Component,
  ElementRef,
  inject,
  Injectable,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormBuilder, FormControl, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { forkJoin, map, Observable, startWith, take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_service/account.service';
import { MembersService } from 'src/app/_service/members.service';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import {
  DatingProfile,
  EItem,
  EItem1,
  UserInterest,
  UserOccupation,
} from '../../_models/DatingProfile';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { LiveAnnouncer } from '@angular/cdk/a11y';
import { DatingService } from '../../_service/Dating.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css'],
})
export class MemberEditComponent implements OnInit {
  // @ViewChild('editForm') editForm: NgForm | undefined;
  @ViewChild('interestInput1') interestInput1!: ElementRef<any>;
  @ViewChild('interestInput2') interestInput2!: ElementRef<any>;
  @ViewChild('occupationInput1') occupationInput1!: ElementRef<any>;
  @ViewChild('occupationInput2') occupationInput2!: ElementRef<any>;

  announcer = inject(LiveAnnouncer);

  _formBuilder = inject(FormBuilder);
  //--------UserInterests
  UserInterests!: EItem[];
  ChooseUserInterests1: EItem[] = [];
  ChooseUserInterests2: EItem[] = [];
  ListUserInterests!: EItem[];

  interestCtrl1 = new FormControl('');
  filteredInterest1!: Observable<EItem[]>;
  interestCtrl2 = new FormControl('');
  filteredInterest2!: Observable<EItem[]>;

  //-------UserOccupations
  UserOccupations!: EItem[];
  ChooseUserOccupations1: EItem[] = [];
  ChooseUserOccupations2: EItem[] = [];
  ListUserOccupations!: EItem[];

  occupationCtrl1 = new FormControl('');
  filteredOccupation1!: Observable<EItem[]>;
  occupationCtrl2 = new FormControl('');
  filteredOccupation2!: Observable<EItem[]>;

  separatorKeysCodes: number[] = [ENTER, COMMA];

  WhereToDates!: EItem[];
  Heights!: EItem[];
  DatingObjects!: EItem[];
  genderList: any[] = [];
  member: Member | undefined;
  user: User | null = null;

  editProfileForm = this._formBuilder.group({
    introduction: [''],
    lookingFor: [''],
    interests1: [null],
    interests2: [null],
    occupations1: [null],
    occupations2: [null],
    city: [''],
    age: [0],
    datingObject: [0],
    whereToDate: [0],
    height: [0],
    weight: [0],
    datingAgeFrom: [0],
    datingAgeTo: [0],
    heightFrom: [0],
    heightTo: [0],
    weightFrom: [0],
    weightTo: [0],
  });

  constructor(
    private accountService: AccountService,
    private memberService: MembersService,
    private toastr: ToastrService,
    private datingService: DatingService
  ) {}

  ngOnInit(): void {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => (this.user = user),
    });
    this.getOptionsValue();
    this.loadMember();

    setTimeout(() => {
      this.filteredInterest1 = this.interestCtrl1.valueChanges.pipe(
        startWith(''),
        map((topic: string | null) =>
          topic ? this._filterInterest(topic, 1) : this.ListUserInterests.slice()
        )
      );
      this.filteredInterest2 = this.interestCtrl2.valueChanges.pipe(
        startWith(''),
        map((topic: string | null) =>
          topic ? this._filterInterest(topic, 2) : this.ListUserInterests.slice()
        )
      );
      this.filteredOccupation1 = this.occupationCtrl1.valueChanges.pipe(
        startWith(''),
        map((topic: string | null) =>
          topic ? this._filterOccupation(topic, 1) : this.ListUserOccupations.slice()
        )
      );
      this.filteredOccupation2 = this.occupationCtrl2.valueChanges.pipe(
        startWith(''),
        map((topic: string | null) =>
          topic ? this._filterOccupation(topic, 2) : this.ListUserOccupations.slice()
        )
      );
    }, 1000);
  }

  getOptionsValue() {
    forkJoin([
      this.datingService.GetUserInterests(),
      this.datingService.GetUserOccupations(),
      this.datingService.GetWhereToDate(),
      this.datingService.GetHeight(),
      this.datingService.GetDatingObject(),
      this.memberService.getGenders(),
    ]).subscribe({
      next: ([userInterests,userOccupations, whereToDate, heights, datingObjects, genders]) => {
        this.UserInterests = userInterests;
        this.UserOccupations = userOccupations;
        this.ListUserInterests = this.UserInterests;
        this.ListUserOccupations = this.UserOccupations;
        this.WhereToDates = whereToDate;
        this.Heights = heights;
        this.DatingObjects = datingObjects;
        this.genderList = genders;
      },
      error: (error: any) => {
        console.log(error);
      },
    });
  }

  loadMember() {
    if (!this.user) return;
    this.memberService.getMember(this.user.userName).subscribe({
      next: (member) => {
        this.member = member;
        this.editProfileForm.patchValue({
          introduction: member.introduction,
          lookingFor: member.lookingFor,
          interests1: member?.datingProfile.userInterests
            .filter((i: any) => i.interestType === 1)
            .map((i: { interestNameCode: any; }) => i.interestNameCode),
          interests2: member?.datingProfile.userInterests
            .filter((i: any)  => i.interestType === 2)
            .map((i: { interestNameCode: any; })  => i.interestNameCode),

          occupations1: member?.datingProfile.occupations
            .filter((i: any) => i.occupationType === 1)
            .map((i: any) => i.occupationNameCode),
          occupations2: member?.datingProfile.occupations
            .filter((i: any)  => i.occupationNameCode === 2)
            .map((i: any)  => i.occupationNameCode),

          city: member.city,
          datingObject: member?.datingProfile.datingObjectCode,
          whereToDate: member?.datingProfile.whereToDateCode,
          height: member.height,
          weight: member.weight,
          age: member.age,
          datingAgeFrom: member?.datingProfile.datingAgeFrom,
          datingAgeTo: member?.datingProfile.datingAgeTo,
          weightFrom: member?.datingProfile.weightFrom,
          weightTo: member?.datingProfile.weightTo,
          heightFrom: member?.datingProfile.heightFrom,
          heightTo: member?.datingProfile.heightTo

        });

        this.ChooseUserInterests1 = member?.datingProfile.userInterests.filter((i: any)  => i.interestType === 1);
        this.ChooseUserInterests2 = member?.datingProfile.userInterests.filter((i: any)  => i.interestType === 2);

        this.ChooseUserOccupations1 = member?.datingProfile.occupations.filter((i: any)  => i.occupationType === 1);
        this.ChooseUserOccupations2 = member?.datingProfile.occupations.filter((i: any)  => i.occupationType === 2);
      },
    });
  }

  updateMember() {
    const formValue = this.editProfileForm.value;
    this.memberService.upDateMember({
      ...this.member,
      ...formValue,
      interests: '',
      datingProfile: {
        ...this.member?.datingProfile,
        userInterests: [
          ...this.ChooseUserInterests1.map((i) => ({
            interestName: i.displayName,
            interestNameCode: i.value,
            interestType: 1,
          })),
          ...this.ChooseUserInterests2.map((i) => ({
            interestName: i.displayName,
            interestNameCode: i.value,
            interestType: 2,
          })),
        ] as UserInterest[],
        occupations: [
          ...this.ChooseUserOccupations1.map((i) => ({
            occupationName: i.displayName,
            occupationNameCode: i.value,
            occupationType: 1,
          })),
          ...this.ChooseUserOccupations2.map((i) => ({
            occupationName: i.displayName,
            occupationNameCode: i.value,
            occupationType: 2,
          })),
        ] as UserOccupation[],
        datingObjectCode: formValue.datingObject || 0,
        whereToDateCode: formValue.whereToDate || 0,
        height: formValue.height?.toString() || 0,
        heightFrom: formValue.heightFrom?.toString() || 0,
        heightTo: formValue.heightTo?.toString() || 0,
        age: formValue.age,
        datingAgeFrom: formValue.datingAgeFrom,
        datingAgeTo: formValue.datingAgeTo,

        // weight: [0],
        // datingAgeFrom: [0],
        // datingAgeTo: [0],
        // heightFrom: [0],
        // heightTo: [0],
        // weightFrom: [0],
        // weightTo: [0],


      } as DatingProfile,
    } as Member).subscribe({
      next: () => {
        this.toastr.success('Profile uploaded successfully');
        this.loadMember();
      },
    });
  }

  selectedTopic1(event: MatAutocompleteSelectedEvent): void {
    const value = event.option.value;
    if (!this.isDupplicationSelect1(value)) {
      const selectedInterest = this.findInterestByName(value);
      if (selectedInterest) {
        this.ChooseUserInterests1.push({ ...selectedInterest, interestType: 1 });
        if (this.interestInput1 && this.interestInput1.nativeElement) {
          this.interestInput1.nativeElement.value = '';
        }
        this.interestCtrl1.setValue(null);
      }
    }
  }

  selectedTopic2(event: MatAutocompleteSelectedEvent): void {
    const value = event.option.value;
    if (!this.isDupplicationSelect2(value)) {
      const selectedInterest = this.findInterestByName(value);
      if (selectedInterest) {
        this.ChooseUserInterests2.push({ ...selectedInterest, interestType: 2 });
        if (this.interestInput2 && this.interestInput2.nativeElement) {
          this.interestInput2.nativeElement.value = '';
        }
        this.interestCtrl2.setValue(null);
      }
    }
  }

  selectedTopicOccupation1(event: MatAutocompleteSelectedEvent): void {
    const value = event.option.value;
    if (!this.isDupplicationSelect1(value)) {
      const selectedOccupation = this.findOccupationByName(value);
      if (selectedOccupation) {
        this.ChooseUserOccupations1.push({ ...selectedOccupation, interestType: 1 });
        if (this.occupationInput1 && this.occupationInput1.nativeElement) {
          this.occupationInput1.nativeElement.value = '';
        }
        this.occupationCtrl1.setValue(null);
      }
    }
  }

  selectedTopicOccupation2(event: MatAutocompleteSelectedEvent): void {
    const value = event.option.value;
    if (!this.isDupplicationSelect2(value)) {
      const selectedOccupation = this.findOccupationByName(value);
      if (selectedOccupation) {
        this.ChooseUserOccupations2.push({ ...selectedOccupation, interestType: 2 });
        if (this.occupationInput2 && this.occupationInput2.nativeElement) {
          this.occupationInput2.nativeElement.value = '';
        }
        this.occupationCtrl2.setValue(null);
      }
    }
  }

  findInterestByName(name: string): EItem | undefined {
    return this.UserInterests.find((item) => item.displayName === name);
  }

  findOccupationByName(name: string): EItem | undefined {
    return this.UserOccupations.find((item) => item.displayName === name);
  }

  isDupplicationSelect1(interest: string): boolean {
    return this.ChooseUserInterests1.some(
      (item) => item.displayName === interest
    );
  }

  isDupplicationSelect2(interest: string): boolean {
    return this.ChooseUserInterests2.some(
      (item) => item.displayName === interest
    );
  }
  isDupplicationSelectOccupation1(occupation: string): boolean {
    return this.ChooseUserOccupations1.some(
      (item) => item.displayName === occupation
    );
  }

  isDupplicationSelectOccupation2(occupation: string): boolean {
    return this.ChooseUserOccupations2.some(
      (item) => item.displayName === occupation
    );
  }

  addTopic1(event: MatChipInputEvent): void {
    const input = event.input;
    const value = (event.value || '').trim();

    if (value) {
      const selectedInterest = this.findInterestByName(value);
      if (selectedInterest) {
        this.ChooseUserInterests1.push({ ...selectedInterest, interestType: 1 });
      }
    }

    if (input) {
      input.value = '';
    }

    this.interestCtrl1.setValue(null);
  }

  addTopic2(event: MatChipInputEvent): void {
    const input = event.input;
    const value = (event.value || '').trim();

    if (value) {
      const selectedInterest = this.findInterestByName(value);
      if (selectedInterest) {
        this.ChooseUserInterests2.push({ ...selectedInterest, interestType: 2 });
      }
    }

    if (input) {
      input.value = '';
    }

    this.interestCtrl2.setValue(null);
  }

  addOccupationTopic1(event: MatChipInputEvent): void {
    const input = event.input;
    const value = (event.value || '').trim();

    if (value) {
      const selectedOccupation = this.findOccupationByName(value);
      if (selectedOccupation) {
        this.ChooseUserOccupations1.push({ ...selectedOccupation, interestType: 1 });
      }
    }

    if (input) {
      input.value = '';
    }

    this.occupationCtrl1.setValue(null);
  }

  addOccupationTopic2(event: MatChipInputEvent): void {
    const input = event.input;
    const value = (event.value || '').trim();

    if (value) {
      const selectedOccupation = this.findOccupationByName(value);
      if (selectedOccupation) {
        this.ChooseUserOccupations2.push({ ...selectedOccupation, interestType: 2 });
      }
    }

    if (input) {
      input.value = '';
    }

    this.occupationCtrl2.setValue(null);
  }

  removeTopic1(interest: EItem): void {
    const index = this.ChooseUserInterests1.indexOf(interest);
    if (index >= 0) {
      this.ChooseUserInterests1.splice(index, 1);
      this.announcer.announce(`Removed ${interest.displayName}`);
    }
  }

  removeTopic2(interest: EItem): void {
    const index = this.ChooseUserInterests2.indexOf(interest);
    if (index >= 0) {
      this.ChooseUserInterests2.splice(index, 1);
      this.announcer.announce(`Removed ${interest.displayName}`);
    }
  }

  removeOccupationTopic1(occupation: EItem): void {
    const index = this.ChooseUserOccupations1.indexOf(occupation);
    if (index >= 0) {
      this.ChooseUserOccupations1.splice(index, 1);
      this.announcer.announce(`Removed ${occupation.displayName}`);
    }
  }

  removeOccupationTopic2(occupation: EItem): void {
    const index = this.ChooseUserOccupations2.indexOf(occupation);
    if (index >= 0) {
      this.ChooseUserOccupations2.splice(index, 1);
      this.announcer.announce(`Removed ${occupation.displayName}`);
    }
  }

  private _filterInterest(value: string, type: number): EItem[] {
    const filterValue = value.toLowerCase();
    return this.ListUserInterests.filter((interest) =>
      interest.displayName.toLowerCase().includes(filterValue)
    );
  }
  private _filterOccupation(value: string, type: number): EItem[] {
    const filterValue = value.toLowerCase();
    return this.ListUserOccupations.filter((occupation) =>
      occupation.displayName.toLowerCase().includes(filterValue)
    );
  }
}
