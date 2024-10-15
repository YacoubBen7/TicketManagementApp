import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss'],
})
export class ModalComponent {
  @Input() visible: boolean = false;
  @Input() header: string = '';
  @Output() visibleChange: EventEmitter<boolean> = new EventEmitter<boolean>();

}
