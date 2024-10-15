import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TicketCreateDTO } from 'src/app/core/models/ticket.model';

@Component({
  selector: 'app-create-ticket',
  templateUrl: './create-ticket.component.html',
  styleUrls: ['./create-ticket.component.scss'],
})
export class CreateTicketComponent {
  @Output() formTicketSubmitter: EventEmitter<TicketCreateDTO> =
    new EventEmitter<TicketCreateDTO>();
  formGroup: FormGroup;
  constructor() {
    this.formGroup = new FormGroup({
      description: new FormControl('', [
        Validators.required,
        Validators.maxLength(200),
      ]),
    });
  }
  get description() {
    return this.formGroup.get('description');
  }
  formSubmit() {
    if (this.formGroup.valid){
      this.formTicketSubmitter.emit(this.formGroup.value as TicketCreateDTO);
      this.formGroup.reset();
    }
  }
}
