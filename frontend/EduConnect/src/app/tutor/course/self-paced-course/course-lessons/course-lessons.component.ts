import { Component } from '@angular/core';
import { SubmitButtonComponent } from '../../../../common/button/submit-button/submit-button.component';
import { CourseLessonModalComponent } from '../course-lesson-modal/course-lesson-modal.component';
import { NgIf } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-course-lessons',
  imports: [SubmitButtonComponent, CourseLessonModalComponent, NgIf],
  templateUrl: './course-lessons.component.html',
  styleUrl: './course-lessons.component.css',
})
export class CourseLessonsComponent {
  addLessonButtonText: string = 'Add lesson';
  addLessonButtonColor: string = 'blue';
  addLessonButtonMargin: string = '10% 0';
  addLessonButtonWidth: string = '100%';

  showCourseLessonModal: boolean = false;

  isCreateOrEditMode: boolean = true;

  modalTitle: string = 'Add Lesson';
  toggleCourseLessonModal() {
    this.showCourseLessonModal = !this.showCourseLessonModal;
  }
  addLesson() {
    console.log('Add lesson component opened');
    this.toggleCourseLessonModal();
  }
}
