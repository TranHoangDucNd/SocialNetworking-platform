export interface Reports {
  id: number;
  userId: number;
  postId: number;
  description: string;
  reportDate: string;
  checked: boolean;
  report: string;
}

export interface PostDetailReportResponseDto {
  id: number;
  content: string;
  images: string[];
  userShort: UserShortDto;
}

export interface UserShortDto {
  id: number;
  fullName: string;
  image: string;
  knownAs: string;
}

export interface MembersLock {
  id: number
  userName: string
  photoUrl: string
  knownAs: string;
  lock: boolean
}
export interface LockUser {
  userName: string
  check: boolean
}
