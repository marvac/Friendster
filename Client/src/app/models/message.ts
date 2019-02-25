export interface Message {
  id: number;
  senderId: number;
  senderKnownAs: string;
  senderPhotoUrl: string;
  recipientId: number;
  recipientKnownAs: string;
  content: string;
  markedAsRead: boolean;
  readDate: Date;
  messageSent: Date;
}
