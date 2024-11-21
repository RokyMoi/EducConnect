import { Component, Input } from '@angular/core';

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
}
