import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { CourseStudentControllerService } from '../../../../services/course/course-student-controller.service';
import { GetCoursesByQueryResponse } from '../../../../models/course/course-student-controller/get-courses-by-query-response';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-student-course-search',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './student-course-search.component.html',
  styleUrl: './student-course-search.component.css',
})
export class StudentCourseSearchComponent implements OnInit {
  searchQuery: string = '';
  pageSize: number = 10;

  courses: GetCoursesByQueryResponse[] = [];
  numberOfItemsPerPage: number[] = [5, 10, 20, 50, 100];

  currentPage: number = 1;
  totalPages: number = 0;
  itemsPerPage: number = 10;
  totalItems: number = 0;

  selectItemsPerPageFormControl: FormControl = new FormControl<number>(10);

  paginationDisabled: boolean = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private snackboxService: SnackboxService,
    private courseStudentControllerService: CourseStudentControllerService
  ) {}
  ngOnInit(): void {
    this.fetchCourses();

    this.selectItemsPerPageFormControl.valueChanges.subscribe((value) => {
      this.itemsPerPage = value;
      this.pageSize = value;
      this.fetchCourses();
    });
  }

  fetchCourses() {
    this.paginationDisabled = true;
    console.log('Fetch courses with following parameters:');
    console.log('Search query: ' + this.searchQuery);
    console.log('Current page (Page Number): ' + this.currentPage);
    console.log('Items per page (Page Size):', this.itemsPerPage);
    console.log('Total pages: ' + this.totalPages);
    this.courseStudentControllerService
      .getCoursesByQuery({
        searchQuery: this.searchQuery,
        pageNumber: this.currentPage,
        pageSize: this.pageSize,
      })
      .subscribe({
        next: (response) => {
          console.log(response);
          this.courses = response.data;

          this.currentPage = response.pageNumber;
          this.totalPages = response.totalPages;
          this.itemsPerPage = response.pageSize;
          this.totalItems = response.totalCount;

          if (this.currentPage >= this.totalPages && this.totalPages > 0) {
            this.currentPage = 1;
          }
          if (this.totalPages === 0) {
            this.currentPage = 0;
          }
          this.paginationDisabled = false;
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Failed to load courses${
              error.error.message ? `, ${error.error.message}` : ''
            }`,
            'error'
          );
        },
      });
  }

  goBack() {
    this.router.navigate(['/student/dashboard']);
  }

  onViewCourse(courseId: string) {}

  onSearch($event: any) {
    console.log($event.target.value);
    this.searchQuery = $event.target.value;
    this.fetchCourses();
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      console.log('Fetch data for page:', this.currentPage);
      this.fetchCourses();
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.fetchCourses();
    }
  }
}
