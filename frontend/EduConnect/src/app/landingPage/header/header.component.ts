import { Component } from '@angular/core';
import { HeaderTemplateComponent } from '../../common/header/header-template/header-template.component';

@Component({
  selector: 'app-landing-page-header',
  standalone: true,
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements HeaderTemplateComponent {
  title = 'EduConnect';
}
