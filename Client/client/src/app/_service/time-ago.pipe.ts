import { Pipe, PipeTransform } from '@angular/core';
import { formatDistanceToNow } from 'date-fns';
import { vi } from 'date-fns/locale';

@Pipe({
  name: 'timeAgo'
})
export class TimeAgoPipe implements PipeTransform {

  transform(value: Date | string): string {
    let date = typeof value === 'string' ? new Date(value) : value;
    // Add 7 hours to the date
    date = new Date(date.getTime() + 7 * 60 * 60 * 1000);
    return formatDistanceToNow(date, { addSuffix: true });
  }

}
