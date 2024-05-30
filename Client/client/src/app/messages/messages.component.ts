import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { MessageService } from '../_service/message.service';
import { ToastrService } from 'ngx-toastr';
import {MatDialog, MatDialogRef} from '@angular/material/dialog';
@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages?: Message[];
  pagination?: Pagination;
  container = 'Unread';
  pageNumber = 1;
  pageSize = 5;
  loading = false;

  constructor(private messageService: MessageService, private toastr: ToastrService, public dialog: MatDialog){

  }
  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(){
    this.loading = true;
    this.messageService.getMessage(this.pageNumber, this.pageSize, this.container).subscribe({
      next: response =>{
        this.messages = response.result;
        this.pagination = response.pagination;
        this.loading = false;
      }
    })
  }

  // deleteAllMessages(userId: number){
  //   this.messageService.deleteAllMessage(userId).subscribe({
  //     next: _ => {
  //       this.toastr.success("Deleted successfully");
  //       this.loadMessages();
  //     },
  //     error: err =>{
  //       this.toastr.error("Delete failed");
  //     }
  //   })
  // }

  // openconfirmDialog(userId: number, event: Event){
  //   event.stopPropagation();
  //   const dialogRef = this.dialog.open(ConfirmDialogComponent);

  //   dialogRef.afterClosed().subscribe(result =>{
  //     if(result){
  //       this.deleteAllMessages(userId);
  //     }
  //   })
  // }

  // deleteMessage(id: number){
  //   this.messageService.deleteMessage(id,).subscribe({
  //     next: () =>{
  //       this.messages?.splice(this.messages.findIndex(m => m.id === id), 1);
  //     }
  //   })
  // }

  pageChanged(event: any){
    if(this.pageNumber !== event.page){
      this.pageNumber = event.page;
      this.loadMessages();

    }
  }

}

