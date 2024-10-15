import { Component, Signal } from '@angular/core';
import { TicketService } from '../../services/ticket.service';
import {
  TicketCreateDTO,
  TicketKeys,
  TicketResponseDTO,
  TicketUpdateDTO,
} from 'src/app/core/models/ticket.model';
import { Page } from 'src/app/core/models/rest.model';
import {
  defaultTicketFilter,
  ITicketFilter,
} from 'src/app/core/models/ticket_filter.model';
import { capitalizeFirstLetter } from 'src/app/shared/Utilities/string_extenstion';

@Component({
  selector: 'app-home-index',
  templateUrl: './home-index.component.html',
  styleUrls: ['./home-index.component.scss'],
})
export class HomeIndexComponent {
  loading: Signal<boolean>;
  ticketsPage: Signal<Page<TicketResponseDTO>>;
  tableFilter: ITicketFilter = { ...defaultTicketFilter };
  visible: boolean = false;
  modalTitle: string = 'Create Ticket';
  selectedTicket: TicketResponseDTO | null = null;

  constructor(private ticketService: TicketService) {
    this.loading = ticketService.loading$;
    this.ticketsPage = ticketService.ticketsPage$;
  }
  OnPageChange(event: { first: number; rows: number }) {
    this.tableFilter = {
      ...this.tableFilter,
      pageSize: event.rows,
      pageIndex: event.first / event.rows,
    };
    this.ticketService.$fetch.next(this.tableFilter);
  }
  OnSortChange(event: { sortColumn: string; sortOrder: number }) {
    const sortOrder = event.sortOrder === 1 ? 'ASC' : 'DESC';
    if (
      this.tableFilter.sortColumn !==
        (capitalizeFirstLetter(event.sortColumn) as TicketKeys) ||
      this.tableFilter.sortOrder !== sortOrder
    ) {
      this.tableFilter = {
        ...this.tableFilter,
        sortColumn: capitalizeFirstLetter(event.sortColumn) as TicketKeys,
        sortOrder: sortOrder,
        pageIndex: 0,
      };

      this.ticketService.$fetch.next(this.tableFilter);
    }
  }
  OnFilterChange($event: string) {
    if (this.tableFilter.filterQuery !== $event) {
      this.tableFilter = {
        ...this.tableFilter,
        filterQuery: $event,
        pageIndex: 0,
      };
      this.ticketService.$fetch.next(this.tableFilter);
    }
  }
  OnDelete($event: number) {
    this.ticketService.$remove.next($event);
  }
  OnEdit($event: number) {
    this.modalTitle = 'Edit Ticket';
    this.visible = true;
    this.selectedTicket = this.ticketsPage().data.find((x) => x.id === $event)!;
  }
  OnAdd() {
    this.visible = true;
    this.modalTitle = 'Create Ticket';
  }

  onTicketAdd($event: TicketCreateDTO) {
    this.ticketService.$add.next($event);
    this.visible = false;
  }
  onTicketEdit($event: { id: number; ticket: TicketUpdateDTO }) {
    this.ticketService.$update.next($event);
    this.visible = false;
  }
}
