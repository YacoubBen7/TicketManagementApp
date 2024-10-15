import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PaginatorState } from 'primeng/paginator';
import { TicketResponseDTO } from 'src/app/core/models/ticket.model';
import { defaultTicketFilter } from 'src/app/core/models/ticket_filter.model';
import { SortEvent } from 'primeng/api';
import { FormControl } from '@angular/forms';
import { debounceTime } from 'rxjs';

@Component({
  selector: 'app-ticket-table',
  templateUrl: './ticket-table.component.html',
  styleUrls: ['./ticket-table.component.scss'],
})
export class TicketTableComponent implements OnInit {
  @Input() tickets: TicketResponseDTO[] = [];
  @Input() loading: boolean = false;
  @Input() totalRecords: number = 0;
  @Output() onPageChange = new EventEmitter<{
    first: number;
    rows: number;
  }>();
  @Output() onSortChange = new EventEmitter<{
    sortColumn: string;
    sortOrder: number;
  }>();

  @Output() onFilterChange = new EventEmitter<string>();

  @Output() onDelete = new EventEmitter<number>();
  @Output() onEdit = new EventEmitter<number>();
  @Output() onAdd = new EventEmitter<void>();
  @Input() first: number = 0;

  rowsPerPageOptions: number[] = [5, 10, 20, 30, 50, 100];
  searchControl: FormControl = new FormControl();

  rows: number = defaultTicketFilter.pageSize;

  onPaginatorChange(event: PaginatorState) {
    this.first = event.first ?? 0;
    this.rows = event.rows ?? 10;
    this.onPageChange.emit({ first: this.first, rows: this.rows });
  }

  ngOnInit(): void {
    this.searchControl.valueChanges
      .pipe(debounceTime(500)) // Delay detection by 500 milliseconds
      .subscribe((value) => {
        this.onFilterChange.emit(value);
      });
  }
  constructor() {}

  customSort(event: SortEvent) {
    console.log(event);
    this.onSortChange.emit({
      sortColumn: event.field as string,
      sortOrder: event.order === 1 ? 1 : -1,
    });
  }
}
