import {Component, ElementRef, inject, Injectable, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormControl, NgForm} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import {forkJoin, map, Observable, startWith, take} from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_service/account.service';
import { MembersService } from 'src/app/_service/members.service';
import {COMMA, ENTER} from "@angular/cdk/keycodes";
import {DatingProfile, EItem, UserInterest} from "../../_models/DatingProfile";
import {MatAutocompleteSelectedEvent} from "@angular/material/autocomplete";
import {MatChipInputEvent} from "@angular/material/chips";
import {LiveAnnouncer} from "@angular/cdk/a11y";
import {DatingService} from "../../_service/Dating.service";

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css'],
})
export class MemberEditComponent implements OnInit{
  // @ViewChild('editForm') editForm: NgForm | undefined;
  @ViewChild('interestInput') interestInput!: ElementRef<any>;
  announcer = inject(LiveAnnouncer);

  _formBuilder = inject(FormBuilder);

  UserInterests!: EItem[];
  ChooseUserInterests: EItem[] = [];
  ListUserInterests!: EItem[];
  interestCtrl = new FormControl('');
  filteredInterest!: Observable<EItem[]>;
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
    interests: [null],
    city: [''],
    country: [''],
    datingObject: [0],
    whereToDate: [0],
    height: [0],
  })

  constructor(
    private accountService: AccountService,
    private memberService: MembersService,
    private toastr: ToastrService,
    private datingService: DatingService,
  ) {
  }

  ngOnInit(): void {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => (this.user = user),
    });
    this.getOptionsValue();
    this.loadMember();
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
    }, 1000);
  }

  getOptionsValue(){
    forkJoin([
      this.datingService.GetUserInterests(),
      this.datingService.GetWhereToDate(),
      this.datingService.GetHeight(),
      this.datingService.GetDatingObject(),
      this.memberService.getGenders()
    ]).subscribe({
      next: ([userInterests, whereToDate, heights, datingObjects, genders]) => {
        this.UserInterests = userInterests;
        this.ListUserInterests = this.UserInterests;
        this.WhereToDates = whereToDate;
        this.Heights = heights;
        this.DatingObjects = datingObjects;
        this.genderList = genders;
      },
      error: (error: any) => {
        console.log(error);
      }
    })
  }

  loadMember(){
    if(!this.user) return;
    this.memberService.getMember(this.user.userName).subscribe({
      next: member => {
        this.member = member;
        this.editProfileForm.patchValue({
          introduction: member.introduction,
          lookingFor: member.lookingFor,
          interests: member?.datingProfile.userInterests.map((i: { interestNameCode: any; }) => i.interestNameCode),
          city: member.city,
          country: member.country,
          datingObject: member?.datingProfile.datingObjectCode,
          whereToDate: member?.datingProfile.whereToDateCode,
          height: member?.datingProfile.heightCode,
        });
      }
    })
  }

  updateMember(){
    const formValue = this.editProfileForm.value;
    console.log(formValue);
    this.memberService.upDateMember({
      ...this.member,
      ...formValue,
      interests: "",
      datingProfile: {
        ...this.member?.datingProfile,
        userInterests: this.ChooseUserInterests.map((i) => {
          return {interestName: i.displayName, interestNameCode: i.value}
        }) as UserInterest[],
        datingObjectCode: formValue.datingObject || 0,
        heightCode: formValue.height || 0,
        whereToDateCode: formValue.whereToDate || 0
      } as DatingProfile
    } as Member).subscribe({
      next: _ =>{
        this.toastr.success('Profile uploaded successfully');
        this.loadMember()
      }
    })
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

}
