import { Component, inject } from '@angular/core';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-landing-page-header',
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
  standalone: true,
})
export class HeaderComponent {
  AccountService = inject(AccountService);
  title = 'EduConnect';
}
