import {
  computed,
  inject,
  Injectable,
  signal,
  WritableSignal,
} from '@angular/core';
import {
  TicketCreateDTO,
  TicketResponseDTO,
  TicketUpdateDTO,
} from 'src/app/core/models/ticket.model';
import { TicketHttpService } from './ticket-http.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, EMPTY, Subject, switchMap } from 'rxjs';
import {
  defaultTicketFilter,
  ITicketFilter,
} from 'src/app/core/models/ticket_filter.model';
import { emptyPage, Page } from 'src/app/core/models/rest.model';

interface State {
  loading: boolean;
  ticketsPage: Page<TicketResponseDTO>;
  ticketsFilter: ITicketFilter;
  error?: any;
}

@Injectable({
  providedIn: 'root',
})
export class TicketService {
  private ticketHttpService = inject(TicketHttpService);

  #state: WritableSignal<State> = signal<State>({
    loading: false,
    ticketsPage: emptyPage,
    ticketsFilter: defaultTicketFilter,
  });

  loading$ = computed(() => this.#state().loading);
  ticketsFilter$ = computed(() => this.#state().ticketsFilter);
  ticketsPage$ = computed(() => this.#state().ticketsPage);

  $fetch: Subject<ITicketFilter> = new Subject<ITicketFilter>();
  $remove: Subject<number> = new Subject<number>();
  $add: Subject<TicketCreateDTO> = new Subject<TicketCreateDTO>();
  $update: Subject<{ id: number; ticket: TicketUpdateDTO }> = new Subject<{
    id: number;
    ticket: TicketUpdateDTO;
  }>();
  constructor() {
    this.DispatchFetchingAllTickets();
    this.DispatchDeletingTicketById();
    this.DispatchAddingTicket();
    this.DispatchUpdatingTicket();
    this.$fetch.next(defaultTicketFilter);
  }

  private DispatchFetchingAllTickets(): void {
    this.$fetch
      .pipe(
        takeUntilDestroyed(),
        switchMap((filter) => {
          this.#state.update((state) => ({
            ...state,
            loading: true,
            ticketsFilter: filter,
          }));
          return this.ticketHttpService.getAllTickets(
            this.#state().ticketsFilter
          );
        }),
        catchError((error) => {
          this.#state.update((state) => ({
            ...state,
            error: error,
            isLoadedList: true,
          }));
          // this.displayStandardMessage(MessageType.Error);
          return EMPTY;
        })
      )
      .subscribe({
        next: (requests) => {
          return this.#state.update((state) => ({
            ...state,
            loading: false,
            ticketsPage: requests.data,
          }));
        },
      });
  }
  private DispatchDeletingTicketById(): void {
    this.$remove
      .pipe(
        takeUntilDestroyed(),
        switchMap((id: number) =>
          this.ticketHttpService.deleteTicketById(id).pipe(
            catchError((error) => {
              this.#state.update((state) => ({
                ...state,
                error: error,
              }));

              return EMPTY;
            })
          )
        )
      )
      .subscribe({
        next: (data) => {
          this.$fetch.next(this.#state().ticketsFilter);
        },
      });
  }
  private DispatchAddingTicket(): void {
    this.$add
      .pipe(
        takeUntilDestroyed(),
        switchMap((ticket: TicketCreateDTO) =>
          this.ticketHttpService.createTicket(ticket).pipe(
            catchError((error) => {
              this.#state.update((state) => ({
                ...state,
                error: error,
              }));

              return EMPTY;
            })
          )
        )
      )
      .subscribe({
        next: () => {
          this.$fetch.next(this.#state().ticketsFilter);
        },
      });
  }
  private DispatchUpdatingTicket(): void {
    this.$update
      .pipe(
        takeUntilDestroyed(),
        switchMap(({ id, ticket }) =>
          this.ticketHttpService.updateTicket(id, ticket).pipe(
            catchError((error) => {
              this.#state.update((state) => ({
                ...state,
                error: error,
              }));

              return EMPTY;
            })
          )
        )
      )
      .subscribe({
        next: () => {
          this.$fetch.next(this.#state().ticketsFilter);
        },
      });
  }
}
