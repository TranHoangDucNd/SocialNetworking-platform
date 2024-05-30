import { User } from './user';

export class UserParams {
  currentUserName = '';
  gender= 0;
  minAge = 0;
  maxAge = 0;
  minHeight = 0;
  maxHeight = 0;
  province = 0;
  pageNumber = 1;
  minWeight = 0;
  maxWeight = 0;
  pageSize = 5;
  orderBy = 'lastActive';

  constructor(user?: User) {
    this.currentUserName = user?.userName || '';
  }
}