import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  TicketResponseDTO,
  TicketUpdateDTO,
} from 'src/app/core/models/ticket.model';
import { statusValidator } from 'src/app/shared/validators/status_validator';

@Component({
  selector: 'app-edit-ticket',
  templateUrl: './edit-ticket.component.html',
  styleUrls: ['./edit-ticket.component.scss'],
})
export class EditTicketComponent implements OnChanges {
  @Output() formTicketSubmitter: EventEmitter<{
    id: number;
    ticket: TicketUpdateDTO;
  }> = new EventEmitter<{
    id: number;
    ticket: TicketUpdateDTO;
  }>();

  @Input() ticket!: TicketResponseDTO;
  statusOptions = ['Open', 'Closed'];

  formGroup: FormGroup = new FormGroup({
    description: new FormControl('', [
      Validators.required,
      Validators.maxLength(200),
    ]),
    status: new FormControl('', [
      Validators.required,
      statusValidator(this.statusOptions),
    ]),
  });

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['ticket']) {
      this.formGroup.patchValue({
        description: this.ticket.description,
        status: this.ticket.status,
      });
    }
  }

  get description() {
    return this.formGroup.get('description');
  }

  get status() {
    return this.formGroup.get('status');
  }

  formSubmit() {
    if (this.formGroup.valid) {
      this.formTicketSubmitter.emit({
        id: this.ticket.id,
        ticket: this.formGroup.value as TicketUpdateDTO,
      });
      this.formGroup.reset();
    }
  }
}
