import { TicketResponseDTO } from './ticket.model';

export interface Link {
  href: string;
  rel: string;
  type: string;
}

export interface Rest<T> {
  links: Link[];
  data: T;
}
export interface Page<T> {
  pageSize: number;
  pageIndex: number;
  recordCount: number;
  totalCount: number;
  data: T[];
}
export const emptyPage: Page<TicketResponseDTO> = {
  pageSize:  0,
  pageIndex: 0,
  recordCount: 0,
  totalCount: 0,
  data: [],
};
