export interface EItem {
  value: number;
  displayName: string;
}

export interface UserInterest {
  id: number;
  datingProfileId: number;
  interestName: number | string;
}

export interface DatingProfile {
  id: number;
  userId: number;
  datingObject: string;
  height: string;
  whereToDate: string;
  userInterests: UserInterest[];
}

