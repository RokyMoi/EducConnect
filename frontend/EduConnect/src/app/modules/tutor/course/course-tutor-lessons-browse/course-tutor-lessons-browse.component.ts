import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { GetCourseLessonByContentFullTextSearchResponse } from '../../../../models/course/course-tutor-controller/get-course-lesson-by-content-full-text-search-response';
import { PublishedStatus } from '../../../../../enums/published-status.enum';

@Component({
  selector: 'app-course-tutor-lessons-browse',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './course-tutor-lessons-browse.component.html',
  styleUrl: './course-tutor-lessons-browse.component.css',
})
export class CourseTutorLessonsBrowseComponent implements OnInit {
  courseId: string = '';

  private searchSubject: Subject<string> = new Subject<string>();

  searchQuery: string = '';
  lessons: GetCourseLessonByContentFullTextSearchResponse[] = [];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private courseTutorControllerService: CourseTutorControllerService
  ) {
    this.courseId = this.route.snapshot.paramMap.get('courseId') as string;
  }
  ngOnInit(): void {
    this.initializeSearchListener();
  }

  initializeSearchListener() {
    this.searchSubject
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe((query) => {
        this.searchQuery = query;
        this.search();
      });
  }

  goBack() {
    this.router.navigate(['/tutor/course/lessons', this.courseId]);
  }

  onSearch(event: Event) {
    const searchTerm = (event.target as HTMLInputElement).value.trim();
    this.searchSubject.next(searchTerm);
  }

  search() {
    console.log('User searched for: ', this.searchQuery);
    if (this.searchQuery.trim() === '') {
      return;
    }
    this.courseTutorControllerService
      .getCourseLessonContentByFullTextSearch(this.searchQuery)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.lessons = response.data;
        },
        error: (error) => {
          console.log(error);
        },
      });
  }

  getPublishedStatus(publishedStatus: number): string {
    return PublishedStatus[publishedStatus];
  }

  onViewLesson(lesson: GetCourseLessonByContentFullTextSearchResponse) {}

  onViewLessonResources(lessonId: string) {
    this.router.navigate([
      'tutor/course/lessons/resources',
      this.courseId,
      lessonId,
    ]);
  }
}
