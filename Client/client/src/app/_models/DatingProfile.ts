export interface EItem {
  value: number;
  displayName: string;
  interestType: number;
}
export interface EItem1 {
  value: number;
  displayName: string;
  occupationType: number;
}

export interface UserInterest {
  id: number;
  datingProfileId: number;
  interestName: number | string;
  interestNameCode: number;
  interestType: number;
}

export interface UserOccupation {
  id: number;
  datingProfileId: number;
  occupationName: number | string;
  occupationNameCode: number;
  occupationType: number;
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
  datingAgeFrom: number;
  datingAgeTo: number;
  weightFrom: number;
  weightTo: number;
  heightTo: number;
  heightFrom: number;
  userInterests: UserInterest[];
  occupations: UserOccupation[];
}

export interface DatingResponse {
  id: number;
  senderId: number;
  senderName: string;
  crushId: number;
  crushName: string;
  startDate: string;
  datingTimeSeconds: number;
  senderAvatar?: string;
  crushAvatar?: string;
}
