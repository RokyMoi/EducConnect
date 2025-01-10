import { Component, Input } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-course-main-materials',
  imports: [],
  templateUrl: './course-main-materials.component.html',
  styleUrl: './course-main-materials.component.css',
})
export class CourseMainMaterialsComponent {
  @Input() componentTitle: string =
    'Select and upload files, your students can use as a general reference while doing the course';
  
}
