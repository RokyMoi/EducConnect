import { Component } from '@angular/core';

import { TabsModule } from 'ngx-bootstrap/tabs';
import { ForYouPageComponent } from "../for-you-page/for-you-page.component";



@Component({
  selector: 'app-student-dashboard',
  standalone: true,
  imports: [TabsModule, ForYouPageComponent],
  templateUrl: './student-dashboard.component.html',
  styleUrl: './student-dashboard.component.css'
})
export class StudentDashboardComponent {

}
