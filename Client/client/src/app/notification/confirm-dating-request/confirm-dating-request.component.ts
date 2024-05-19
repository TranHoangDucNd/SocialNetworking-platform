import {Component, Inject, inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-confirm-dating-request',
  templateUrl: './confirm-dating-request.component.html',
  styleUrls: ['./confirm-dating-request.component.css']
})
export class ConfirmDatingRequestComponent {
  _dialogRef = inject(MatDialogRef<ConfirmDatingRequestComponent>);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { name: string, type: DatingRequestType }) {
  }

  handleResult(result: boolean) {
    this._dialogRef.close(result);
  }

  protected readonly DatingRequestType = DatingRequestType;
}

export enum DatingRequestType {
  Accept = 1,
  Cancel = 2
}
