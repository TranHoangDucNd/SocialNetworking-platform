import { DatingProfile } from './DatingProfile';
import { photo } from './photo';

export interface Member {
  id: number;
  userName: string;
  photoUrl: string;
  age: number;
  knownAs: string;
  created: Date;
  lastActive: Date;
  gender: string;
  introduction: string;
  city: string;
  height: number;
  weight: number;
  photos: photo[];
  datingProfile: DatingProfile;
}
