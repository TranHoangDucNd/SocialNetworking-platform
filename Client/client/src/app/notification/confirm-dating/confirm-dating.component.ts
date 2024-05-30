import { Component, Inject, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-dating',
  templateUrl: './confirm-dating.component.html',
  styleUrls: ['./confirm-dating.component.css']
})
export class ConfirmDatingComponent {
  _dialogRef = inject(MatDialogRef<ConfirmDatingComponent>);

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
