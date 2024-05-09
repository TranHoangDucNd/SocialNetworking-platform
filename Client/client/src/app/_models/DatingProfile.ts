export interface EItem {
  value: number;
  displayName: string;
}

export interface UserInterest {
  id: number;
  datingProfileId: number;
  interestName: number | string;
  interestNameCode: number;
}

export interface DatingProfile {
  id: number;
  userId: number;
  datingObject: string;
  datingObjectCode: number;
  height: string;
  heightCode: number;
  whereToDate: string;
  whereToDateCode: number;
  userInterests: UserInterest[];
}

