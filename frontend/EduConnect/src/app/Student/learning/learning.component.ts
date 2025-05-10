import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { MyCoursesService, CourseItem, PaginatedResponse } from '../../services/Shopping-Cart/my-courses.service';
import { NgForOf, NgIf } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-learning',
  standalone: true,
  imports: [MatCardModule, NgForOf, NgIf, MatPaginatorModule, MatProgressSpinnerModule],
  templateUrl: './learning.component.html',
  styleUrl: './learning.component.css'
})
export class LearningComponent implements OnInit {
  courses: CourseItem[] = [];
  loading = false;

  // Pagination properties
  totalItems: number = 0;
  pageSize: number = 5;
  pageIndex: number = 0;
  pageSizeOptions: number[] = [5, 10, 25];

  constructor(
    private http: HttpClient,
    private router: Router,
    private accountService: AccountService,
    private myCoursesService: MyCoursesService
  ) {}

  ngOnInit(): void {
    this.loadCourses();
  }

  loadCourses(): void {
    this.loading = true;
    this.myCoursesService.getMyCourses(this.pageIndex + 1, this.pageSize).subscribe({
      next: (response: PaginatedResponse<CourseItem>) => {
        this.courses = response.data;
        this.totalItems = response.totalCount;
        this.loading = false;
        console.log("Courses loaded", this.courses);
      },
      error: error => {
        console.error("Error loading courses", error.error);
        this.loading = false;
      }
    });
  }

  handlePageEvent(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadCourses();
  }
}
