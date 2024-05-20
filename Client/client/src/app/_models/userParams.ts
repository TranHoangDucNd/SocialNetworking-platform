import { User } from './user';

export class UserParams {
  currentUserName = '';
  gender= 0;
  minAge = 0;
  maxAge = 0;
  minHeight = 120;
  maxHeight = 200;
  province = 0;
  pageNumber = 1;
  pageSize = 5;
  orderBy = 'lastActive';

  constructor(user?: User) {
    this.currentUserName = user?.userName || '';
  }

}
