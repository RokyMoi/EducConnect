import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-custom-header-ng-content-dialog-box',
  standalone: true,
  imports: [],
  templateUrl: './custom-header-ng-content-dialog-box.component.html',
  styleUrl: './custom-header-ng-content-dialog-box.component.css',
})
export class CustomHeaderNgContentDialogBoxComponent {
  @Input() title: string = 'Dialog Title';
}
