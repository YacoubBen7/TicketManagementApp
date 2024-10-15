import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TicketRoutingModule } from './ticket-routing.module';
import { HomeIndexComponent } from './pages/home-index/home-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { TicketTableComponent } from './components/ticket-table/ticket-table.component';
import { CreateTicketComponent } from './components/create-ticket/create-ticket.component';
import { ReactiveFormsModule } from '@angular/forms';
import { EditTicketComponent } from './components/edit-ticket/edit-ticket.component';


@NgModule({
  declarations: [
    HomeIndexComponent,
    TicketTableComponent,
    CreateTicketComponent,
    EditTicketComponent
  ],
  imports: [
    CommonModule,
    TicketRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ]
})
export class TicketModule { }
