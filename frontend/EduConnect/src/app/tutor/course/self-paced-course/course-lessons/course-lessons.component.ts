import { Component, Input, OnInit } from '@angular/core';
import { SubmitButtonComponent } from '../../../../common/button/submit-button/submit-button.component';
import { CourseLessonModalComponent } from '../course-lesson-modal/course-lesson-modal.component';
import { NgFor, NgIf } from '@angular/common';
import { CourseCreateService } from '../../../../services/course/course-create-service.service';
import { CourseLessonShorthand } from '../../../../_models/course/course-lesson/course-lesson-shorthand.model';
import { FloatingWarningBoxComponent } from '../../../../common/floating-warning-box/floating-warning-box/floating-warning-box.component';
import { CourseDeleteDialogComponent } from '../course-delete-dialog/course-delete-dialog.component';

@Component({
  standalone: true,
  selector: 'app-course-lessons',
  imports: [
    SubmitButtonComponent,
    CourseLessonModalComponent,
    NgIf,
    NgFor,
    FloatingWarningBoxComponent,
    CourseDeleteDialogComponent,
  ],
  templateUrl: './course-lessons.component.html',
  styleUrl: './course-lessons.component.css',
})
export class CourseLessonsComponent implements OnInit {
  @Input() courseCreateService: CourseCreateService = new CourseCreateService();
  @Input() courseId: string = 'e6c87eb6-f462-4516-8d17-ddbd9b3b1def';

  addLessonButtonText: string = 'Add lesson';
  addLessonButtonColor: string = 'blue';
  addLessonButtonMargin: string = '10% 0';
  addLessonButtonWidth: string = '100%';

  showCourseLessonModal: boolean = false;

  //Variable that tracks the visibility of the dialog for deleting the selected course lesson
  showCourseLessonDeleteDialog: boolean = false;

  isCreateOrEditMode: boolean = true;

  modalTitle: string = 'Add Lesson';

  courseLessonList: CourseLessonShorthand[] = [];

  totalSizeOfSupplementaryMaterialsForTheCourse: number = 0;

  //Variable to track the selected course lesson to delete
  selectedCourseLessonIndex: number = -1;

  //Track sorting state
  currentSortColumn: keyof CourseLessonShorthand | null = null;
  isAscending: boolean = true;

  //Variable that holds the selected course lesson id
  selectedCourseLessonId: string = '';
  ngOnInit(): void {
    this.loadCourseLessons();
  }

  toggleCourseLessonModal() {
    this.loadCourseLessons();
    this.showCourseLessonModal = !this.showCourseLessonModal;
  }
  addLesson() {
    console.log('Add lesson component opened');
    this.selectedCourseLessonId = '';
    this.toggleCourseLessonModal();
  }

  loadCourseLessons() {
    console.log('Loading course lessons...');
    this.courseCreateService
      .getCourseLessonShorthandListByCourseId(this.courseId)
      .subscribe((response) => {
        console.log(response);

        if (response.success === 'true') {
          this.courseLessonList = response.data.courseLesson;
          console.log(this.courseLessonList);
          this.totalSizeOfSupplementaryMaterialsForTheCourse =
            this.courseLessonList.reduce(
              (sum, lesson) =>
                sum + lesson.courseLessonSupplementaryMaterialTotalSize,
              0
            );
        }
        if (response.success !== 'true') {
          this.courseLessonList = [];
        }
        console.log('Course lesson list length:', this.courseLessonList.length);
      });
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('ba', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  }

  sortByColumn(column: keyof CourseLessonShorthand) {
    if (this.currentSortColumn === column) {
      this.isAscending = !this.isAscending;
    } else {
      this.currentSortColumn = column;
      this.isAscending = true;
    }

    this.courseLessonList.sort((a, b) => {
      const valueA = a[column];
      const valueB = b[column];

      if (valueA < valueB) return this.isAscending ? -1 : 1;
      if (valueA > valueB) return this.isAscending ? 1 : -1;
      return 0;
    });
  }

  openEditCourseLessonModal(courseLessonId: string, index: number) {
    console.log(
      'Edit course lesson modal opened for course lesson with id: ' +
        courseLessonId
    );
    console.log('Selected course lesson index: ' + index);
    console.log('Selected course lesson: ', this.courseLessonList[index]);
    console.log(
      'Selected course lesson created at: ',
      this.courseLessonList[index].createdAt
    );
    this.isCreateOrEditMode = false;
    this.selectedCourseLessonIndex = index;
    this.selectedCourseLessonId = courseLessonId;
    this.toggleCourseLessonModal();
  }

  //Toggles the dialog for deleting the selected course lesson
  openCourseLessonDeleteDialog(courseLessonId: string, index: number) {
    console.log('Selected course lesson id: ', courseLessonId);
    console.log('Selected course lesson index: ', index);
    console.log(
      'Selected course lesson properties:',
      this.courseLessonList[index]
    );
    this.showCourseLessonDeleteDialog = true;

    //Assign the selected course lesson index to the global variable selectedCourseLessonIndex
    this.selectedCourseLessonIndex = index;
    this.selectedCourseLessonId = courseLessonId;
  }

  closeDeleteDialog() {
    this.selectedCourseLessonId = '';
    this.selectedCourseLessonIndex = -1;
    this.showCourseLessonDeleteDialog = false;
  }

  closeDeleteDialogAndRefreshCourseLessonList() {
    console.log('Closing delete dialog and refreshing course lesson list...');
    this.selectedCourseLessonId = '';
    this.selectedCourseLessonIndex = -1;
    this.showCourseLessonDeleteDialog = false;
    this.showCourseLessonModal = false;
    this.loadCourseLessons();
  }
}
