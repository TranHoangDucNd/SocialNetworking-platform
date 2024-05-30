import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { BsModalService } from 'ngx-bootstrap/modal';
import { Overlay, ToastrService } from 'ngx-toastr';
import {MembersLock, Reports} from 'src/app/_models/admin';
import { PostReportDto, UserShortDto } from 'src/app/_models/PostModels';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_service/admin.service';
import { PostreportdetailComponent } from '../postreportdetail/postreportdetail.component';
import {MatTableDataSource} from "@angular/material/table";

@Component({
  selector: 'app-managepostreport',
  templateUrl: './managepostreport.component.html',
  styleUrls: ['./managepostreport.component.css']
})
export class ManagepostreportComponent implements OnInit{

  user!: User;
  userShort!: UserShortDto;
  postReports: Reports[] = [];
  dataSource: MatTableDataSource<Reports> = new MatTableDataSource<Reports>(this.postReports);
  displayedColumns: string[] = ['id','report', 'description', 'action'];

  constructor(private adminService: AdminService,
    private toastr: ToastrService,
    private modalService: BsModalService,
    private dialog: MatDialog,
    private overlay: Overlay
  ){}
  ngOnInit(): void {
    this.getPostReports();
  }

  getPostReports(){
    return this.adminService.getPostReports().subscribe(
      (data: any) =>{
        if(data.isSuccessed === true){
          this.postReports = data.resultObj as Reports[];
          this.dataSource = new MatTableDataSource(this.postReports);
        }
      }
    )
  }

  openReport(postId: number){
    this.openDialogReportDetail('10ms', '10ms', postId);
  }

  openDialogReportDetail(enteranimation: any, exitanimation: any, postId: number){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.hasBackdrop = false;
    const popup = this.dialog.open(PostreportdetailComponent,{
      enterAnimationDuration: enteranimation,
      exitAnimationDuration: exitanimation,
      width: '600px',
      height: '700px',
      data: {
        SubId: postId,
      },
      backdropClass: 'custom-backdrop',
    })
  }

  deletePost(postId: number){
    this.adminService.deletePost(postId).subscribe(
      (data: any) =>{
        if(data.isSuccessed === true){
          this.getPostReports();
        }
      }
    )
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

}
