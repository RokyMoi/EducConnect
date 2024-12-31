import { Component, inject } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { MatIconModule } from '@angular/material/icon';
import {  RouterLink, RouterLinkActive,Router} from '@angular/router';
import { MatCardModule } from '@angular/material/card';


@Component({
  selector: 'app-landing-page-header',
  imports: [MatIconModule,RouterLink,RouterLinkActive,MatCardModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
  standalone: true,
})
export class HeaderComponent {
  AccountService = inject(AccountService);
  title = 'EduConnect';
}
