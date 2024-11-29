import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  standalone: true,
  imports: [],
  templateUrl: './text-input-component.component.html',
  styleUrl: './text-input-component.component.css',
})
export class TextInputComponentComponent {
  @Input() label: string = 'Input';
  @Input() placeholder: string = 'Enter your values here...';
  @Input() inputId: string = 'input';
  @Input() type: string = 'text';
  @Input() warning: string = '';
  @Input() value: string = '';
  @Input() width: string = '100%';
  @Output() input = new EventEmitter<Event>();

  onInput(event: Event) {
    this.input.emit(event);
  }
}
