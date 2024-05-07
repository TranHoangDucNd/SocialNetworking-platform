import { User } from './user';

export class UserParams {
  currentUserName = '';
  gender= 2;
  minAge = 18;
  maxAge = 99;
  minHeight = 120;
  maxHeight = 200;
  province = 0;
  pageNumber = 1;
  pageSize = 5;
  orderBy = 'lastActive';

  constructor(user?: User) {
    this.currentUserName = user?.userName || '';
    this.gender = user?.gender === 'female' ? 1 : 2;
  }

}
