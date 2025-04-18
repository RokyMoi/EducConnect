import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { GetAllCourseTagsByCourseIdResponse } from '../../../../models/course/course-tutor-controller/get-all-course-tags-by-course-id-response';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { GetAllTagsByTutorResponse } from '../../../../models/course/course-tutor-controller/get-all-tags-by-tutor-response';
import { GetTagsBySearchResponse } from '../../../../models/course/course-tutor-controller/get-tags-by-search-response';
import { FormsModule, NgModel } from '@angular/forms';

@Component({
  selector: 'app-course-tutor-tags',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './course-tutor-tags.component.html',
  styleUrl: './course-tutor-tags.component.css',
})
export class CourseTutorTagsComponent implements OnInit {
  courseId: string = '';
  courseTags: GetAllCourseTagsByCourseIdResponse[] = [];
  tagsByTutor: GetAllTagsByTutorResponse[] = [];
  tagsByAll: GetTagsBySearchResponse[] = [];

  searchQueryTagsByAll: string = '';
  currentPageTagsByAll: number = 1;
  totalPagesTagsByAll: number = 0;
  pageSizeTagsByAll: number = 10;

  searchQueryTagsByTutor: string = '';
  filteredTagsByTutor: GetAllTagsByTutorResponse[] = [];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private courseTutorControllerService: CourseTutorControllerService,
    private snackboxService: SnackboxService
  ) {
    this.courseId = route.snapshot.paramMap.get('courseId') as string;
  }

  ngOnInit(): void {
    this.fetchCourseTags();
    this.fetchTagsByTutor();
    this.fetchTagsByAllBySearch();
  }

  goBack() {
    this.router.navigate(['/tutor/course', this.courseId]);
  }

  addNewTag() {}

  fetchCourseTags() {
    this.courseTutorControllerService
      .getAllCourseTagsByCourseId(this.courseId)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.courseTags = response.data;
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Failed to load course tags${
              error.error.message ? `: ${error.error.message}` : ''
            }`,
            'error'
          );
        },
      });
  }

  removeTagFromCourse(tag: GetAllCourseTagsByCourseIdResponse) {
    this.courseTutorControllerService
      .removeTagFromCourse({
        courseId: this.courseId,
        tagId: tag.tagId,
      })
      .subscribe({
        next: (response) => {
          console.log(response);
          this.fetchCourseTags();
          this.snackboxService.showSnackbox(
            `Tag ${tag.name} removed from course`,
            'success'
          );
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Failed to remove tag ${tag.name} from course${
              error.error.message ? `: ${error.error.message}` : ''
            }`,
            'error'
          );
        },
      });
  }

  fetchTagsByTutor() {
    this.courseTutorControllerService
      .getAllTagsByTutor(this.courseId)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.tagsByTutor = response.data;
          this.filteredTagsByTutor = [...this.tagsByTutor];
          this.searchTagsByTutor();
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Failed to load tags by tutor${
              error.error.message ? `: ${error.error.message}` : ''
            }`,
            'error'
          );
        },
      });
  }

  fetchTagsByAllBySearch() {
    console.log(this.searchQueryTagsByAll);
    this.courseTutorControllerService
      .getTagsBySearch({
        searchQuery: this.searchQueryTagsByAll,
        pageNumber: this.currentPageTagsByAll,
        pageSize: this.pageSizeTagsByAll,
        containsTagCourseId: this.courseId,
      })
      .subscribe({
        next: (response) => {
          console.log(response);
          this.tagsByAll = response.data.items;
          this.totalPagesTagsByAll = response.data.totalPages;
          if (this.tagsByAll.length == 0) {
            this.currentPageTagsByAll = 1;
          }
        },
        error: (error) => {
          console.log(error);
        },
      });
  }

  onChangePageTagsByAll(pageNumber: number) {
    if (pageNumber > this.totalPagesTagsByAll) {
      this.currentPageTagsByAll = 1;
    } else if (pageNumber < 1) {
      this.currentPageTagsByAll = this.totalPagesTagsByAll;
    } else {
      this.currentPageTagsByAll = pageNumber;
    }

    this.fetchTagsByAllBySearch();
  }

  onChangeNumberOfPagesTagsByAll() {
    console.log(this.pageSizeTagsByAll);
    this.fetchTagsByAllBySearch();
  }

  assignTagToCourse(tagId: string, tagName: string) {
    this.courseTutorControllerService
      .assignTagToCourse({
        courseId: this.courseId,
        tagId: tagId,
      })
      .subscribe({
        next: (response) => {
          console.log(response);
          this.fetchCourseTags();
          this.fetchTagsByTutor();
          this.snackboxService.showSnackbox(
            `Tag ${tagName} assigned to course`,
            'success'
          );
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Failed to assign tag ${tagName} to course${
              error.error.message ? `: ${error.error.message}` : ''
            }`,
            'error'
          );
        },
      });
  }

  createTag(tagName: string) {
    if (tagName.trim().length === 0) {
      this.snackboxService.showSnackbox(`Tag name cannot be empty`, 'error');
      return;
    }
    this.courseTutorControllerService
      .createOrUpdateCourseTag({
        tagId: null,
        name: tagName,
      })
      .subscribe({
        next: (response) => {
          console.log(response);
          this.searchQueryTagsByTutor = '';
          this.fetchTagsByTutor();
          this.fetchTagsByTutor();
          this.fetchCourseTags();
          this.fetchTagsByAllBySearch();
          this.snackboxService.showSnackbox(
            `Tag ${tagName} created`,
            'success'
          );
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Failed to create tag ${tagName}${
              error.error.message ? `: ${error.error.message}` : ''
            }`,
            'error'
          );
        },
      });
  }

  searchTagsByTutor() {
    console.log(this.searchQueryTagsByTutor);
    if (!this.searchQueryTagsByTutor) {
      this.filteredTagsByTutor = [...this.tagsByTutor];
      return;
    }

    const searchTerm = this.searchQueryTagsByTutor.trim().toLowerCase();
    console.log(searchTerm);
    this.filteredTagsByTutor = this.tagsByTutor.filter((tag) =>
      tag.name.toLowerCase().includes(searchTerm)
    );
  }

  deleteTag(tagId: string, tagName: string) {
    return this.courseTutorControllerService.deleteTag(tagId).subscribe({
      next: (response) => {
        console.log(response);
        this.snackboxService.showSnackbox(`Tag ${tagName} deleted`, 'success');
        this.fetchTagsByTutor();
      },
      error: (error) => {
        console.log(error);
        this.snackboxService.showSnackbox(
          `Failed to delete tag ${tagName}${
            error.error.message ? `: ${error.error.message}` : ''
          }`,
          'error'
        );
      },
    });
  }
}
