import { Component } from '@angular/core';

@Component({
  selector: 'app-header-template',
  standalone: true,
  imports: [],
  templateUrl: './header-template.component.html',
  styleUrl: './header-template.component.css',
})
export class HeaderTemplateComponent {
  title = 'EduConnect';
}
