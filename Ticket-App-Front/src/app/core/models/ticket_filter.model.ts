import { TicketKeys } from "./ticket.model";

export interface ITicketFilter {
  pageIndex: number;
  pageSize: number;
  sortColumn: TicketKeys;
  sortOrder: 'ASC' | 'DESC';
  filterQuery: string;
}

export const defaultTicketFilter: ITicketFilter = {
  pageIndex: 0,
  pageSize: 10,
  sortColumn: 'Id',
  sortOrder: 'ASC',
  filterQuery: '',
};
