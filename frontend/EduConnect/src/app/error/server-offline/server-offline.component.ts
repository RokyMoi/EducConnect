import { Component } from '@angular/core';
import { HeaderTemplateComponent } from '../../common/header/header-template/header-template.component';

@Component({
  selector: 'app-server-offline',
  imports: [HeaderTemplateComponent],
  templateUrl: './server-offline.component.html',
  styleUrl: './server-offline.component.css',
  standalone: true,
})
export class ServerOfflineComponent {}
