import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-floating-warning-box',
  imports: [],
  templateUrl: './floating-warning-box.component.html',
  styleUrl: './floating-warning-box.component.css',
})
export class FloatingWarningBoxComponent {
  @Input() warningTitle: string = '';
  @Input() warningMessage: string = '';
  @Input() toggleFloatingBox: Function = () => {};
  @Input() isFloatingVisible: boolean = false;
}
