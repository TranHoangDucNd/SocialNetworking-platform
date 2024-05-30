export interface Message {
  id: number;
  senderId: number;
  senderUsername: string;
  senderPhotoUrl: string;
  recipientId: number;
  recipientUsername: string;
  recipientPhotoUrl: string;
  content: string;
  url?: string;
  dateRead?: Date;
  messageSent: string;
}


export interface CreateMessageDto {
  recipientUsername: string;
  content: string;
  url?: any;
  publicId?: any;
}

export interface UploadImageMess {
  path: string
  publicId: string
}

export interface MessagesForUser {
  username: string
  lastMessageContent: string
  url?: string
  lastMessageSent: string,
  knownAs: string,
  dateRead: boolean
}