import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Result } from 'src/app/_models/PostModels';
@Component({
  selector: 'app-show-list-reaction',
  templateUrl: './show-list-reaction.component.html',
  styleUrls: ['./show-list-reaction.component.css']
})
export class ShowListReactionComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: { resultObj: Result[] }) { }
}
