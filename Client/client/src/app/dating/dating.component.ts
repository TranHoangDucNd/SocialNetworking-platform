import { Component, OnDestroy, OnInit } from '@angular/core';
import { DatingResponse } from '../_models/DatingProfile';
import { MembersService } from '../_service/members.service';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { differenceInDays, format } from 'date-fns';
import { ConfirmDatingComponent, DatingRequestType } from '../notification/confirm-dating/confirm-dating.component';

@Component({
  selector: 'app-dating',
  templateUrl: './dating.component.html',
  styleUrls: ['./dating.component.css'],
})
export class DatingComponent implements OnInit, OnDestroy {
  daysSinceDate: number = 0;
  currentTime: string = '';
  intervalId: any;
  datingResponse: DatingResponse | undefined;
  noDatingMessage = 'Bạn đang không có mối quan hệ nào cả';

  private targetDateString = '';
  targetDate: Date = new Date();

  constructor(private memberService: MembersService, private dialog: MatDialog, private toastr: ToastrService){

  }

  ngOnInit(): void {
    this.loadDating();
  }

  ngOnDestroy(): void {
    if(this.intervalId){
      clearInterval(this.intervalId);
    }
  }

  private calculateDaysSinceDate(): void{
    const now = new Date();
    this.daysSinceDate = differenceInDays(now, this.targetDate) + 1;
  }

  private startClock(): void{
    this.intervalId = setInterval(() => {
      this.currentTime = format(new Date(), 'HH:mm:ss');
    }, 1000);
  }

  endDating(){
    const ref = this.dialog.open(ConfirmDatingComponent, {
      data: {name: this.datingResponse?.crushName, type: DatingRequestType.Cancel}
    });

    ref.afterClosed().subscribe(result =>{
      if(result){
        this.memberService.endDating().subscribe({
          next: (res) => {
            console.log(res.message);
            this.toastr.success(res.message);
            this.loadDating();
          }
        })
      }
    })
  }

  private loadDating(){
    this.memberService.getDating().subscribe({
      next: (response) => {
        if(response.resultObj === null){
          this.datingResponse = undefined;
          this.noDatingMessage = response.message;
        }

        this.targetDateString = response.resultObj?.startDate;
        this.datingResponse = response.resultObj;
        this.targetDate = new Date(this.targetDateString);
        this.calculateDaysSinceDate();
        this.startClock();
      }
    })
  }
}
