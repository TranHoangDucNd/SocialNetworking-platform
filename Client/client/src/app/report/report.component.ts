import { Component, OnInit } from '@angular/core';
import { PostReportDto, UserShortDto } from '../_models/PostModels';
import { EItem } from '../_models/DatingProfile';
import { PostService } from '../_service/post.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {
  userShort!: UserShortDto;
  report: EItem[] = [];
  postReport: PostReportDto = {
    id: 0,
    checked: false,
    description: '',
    postId: 0,
    userId: 0,
    reportDate: new Date(),
    report: -1,
  };

  postId!: number;

  constructor(
    private service: PostService,
    private modalService: BsModalService,
    private router: Router,
    private toastr: ToastrService
  ){

  }
  ngOnInit(): void {
    this.getUserShort();
    this.getContentReport();
  }

  getUserShort(){
    this.service.getUserShort().subscribe(
      (data: any) => {
        console.log(data);
        this.userShort = data.resultObj;
      }
    )
  }

  getContentReport(){
    this.service.GetContentReport().subscribe(
      (data: any) =>{
        console.log(data)
        this.report = data;
      }
    )
  }

  Report(){
    this.postReport.userId = this.userShort.id;
    this.postReport.postId = this.postId;
    if(this.postReport.report == -1){
      this.toastr.info('Please choose report content!');
      return;
    }

    this.service.Report(this.postReport).subscribe(
      (data: any) =>{
        if(data == true){
          this.modalService.hide();
          this.toastr.success('Report success!');
        }
      },
      (error) =>{
        console.log(error)
      }
    )
  }
  
}
