import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { delay, delayWhen, Observable, of, timer } from 'rxjs';
import { Page, Rest } from 'src/app/core/models/rest.model';
import {
  TicketCreateDTO,
  TicketResponseDTO,
  TicketUpdateDTO,
} from 'src/app/core/models/ticket.model';
import { ITicketFilter } from 'src/app/core/models/ticket_filter.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TicketHttpService {
  private http = inject(HttpClient);

  public getAllTickets(
    filter: ITicketFilter
  ): Observable<Rest<Page<TicketResponseDTO>>> {
    let params = new HttpParams()
      .set('pageIndex', filter.pageIndex.toString())
      .set('pageSize', filter.pageSize.toString())
      .set('sortColumn', filter.sortColumn)
      .set('sortOrder', filter.sortOrder)
      .set('filterQuery', filter.filterQuery);
    return this.http
      .get<Rest<Page<TicketResponseDTO>>>(
        `${environment.apiBaseUrl}/api/Ticket`,
        {
          params,
        }
      )
      .pipe(delayWhen(() => (environment.production ? timer(0) : timer(1000))));
  }

  public deleteTicketById(id: number): Observable<Rest<number>> {
    return this.http
      .delete<Rest<number>>(`${environment.apiBaseUrl}/api/Ticket/${id}`)
      .pipe(delayWhen(() => (environment.production ? timer(0) : timer(1000))));
  }
  public createTicket(ticket: TicketCreateDTO): Observable<Rest<number>> {
    return this.http
      .post<Rest<number>>(`${environment.apiBaseUrl}/api/Ticket`, ticket)
      .pipe(delayWhen(() => (environment.production ? timer(0) : timer(1000))));
  }
  public updateTicket(
    id: number,
    ticket: TicketUpdateDTO
  ): Observable<Rest<number>> {
    return this.http
      .patch<Rest<number>>(`${environment.apiBaseUrl}/api/Ticket/${id}`, ticket)
      .pipe(delayWhen(() => (environment.production ? timer(0) : timer(1000))));
  }
}
