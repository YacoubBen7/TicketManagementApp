export type TicketKeys = 'Id' | 'Title' | 'Description' | 'Status' | 'CreatedAt' | 'UpdatedAt';

export interface TicketResponseDTO {
  id: number;
  title: string;
  description: string;
  status: string;
  createdAt: string;
  updatedAt: string;
}
export interface TicketCreateDTO {
  Description: string;
}
export interface TicketUpdateDTO {
  Description: string;
  Status: string;
}
