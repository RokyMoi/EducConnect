export interface Message {
    id: number
    senderId: string
    senderEmail: string
    senderPhotoUrl: string
    recipientId: string
    recipientPhotoUrl: string
    recipientEmail: string
    content: string
    dateRead?: Date
    messageSent: Date
  }