import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { GetCourseTeachingResourceResponse } from '../../../../models/course/course-tutor-controller/get-course-teaching-resource-response';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { CommonModule } from '@angular/common';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { CustomHeaderNgContentDialogBoxComponent } from '../../../shared/custom-header-ng-content-dialog-box/custom-header-ng-content-dialog-box.component';

@Component({
  selector: 'app-course-teaching-resources',
  standalone: true,
  imports: [CommonModule, CustomHeaderNgContentDialogBoxComponent],
  templateUrl: './course-teaching-resources.component.html',
  styleUrl: './course-teaching-resources.component.css',
})
export class CourseTutorTeachingResourcesComponent implements OnInit {
  courseId: string | null = null;

  resources: GetCourseTeachingResourceResponse[] = [];

  downloadProgress: { [key: string]: number } = {};
  currentDownloads: Set<string> = new Set();

  showDeleteDialog: boolean = false;
  deleteDialogMessage: string = '';
  selectedResource: GetCourseTeachingResourceResponse | null = null;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private snackboxService: SnackboxService,
    private courseTutorControllerService: CourseTutorControllerService
  ) {
    this.courseId = this.route.snapshot.paramMap.get('courseId');
  }
  ngOnInit(): void {
    this.loadAllResources();
  }
  goBack() {
    this.router.navigate(['/tutor/course/' + this.courseId]);
  }

  onAddNewResource() {
    this.router.navigate([
      '/tutor/course/teaching-resources/new/' + this.courseId,
    ]);
  }

  onViewResource(courseTeachingResourceId: string) {
    this.router.navigate([
      '/tutor/course/teaching-resources/details',
      this.courseId,
      courseTeachingResourceId,
    ]);
  }
  loadAllResources() {
    this.courseTutorControllerService
      .getAllCourseTeachingResourcesByCourseId(this.courseId as string)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.resources = response.data;
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Failed to load resources${
              error.error.message ? `, ${error.error.message}` : ''
            }`,
            'error'
          );
        },
      });
  }

  onDownloadResource(resource: GetCourseTeachingResourceResponse) {
    if (!resource.fileName || !this.courseId) return;

    const resourceId = resource.courseTeachingResourceId;
    this.currentDownloads.add(resourceId);

    this.courseTutorControllerService
      .downloadCourseTeachingResource(resourceId)
      .subscribe({
        next: (event) => {
          if (event.type === HttpEventType.DownloadProgress) {
            const progress = Math.round(
              (event.loaded / (event.total || 1)) * 100
            );
            this.downloadProgress[resourceId] = progress;
          } else if (event instanceof HttpResponse) {
            this.handleFileDownload(event.body as Blob, resource.fileName!);
            this.cleanupDownload(resourceId);
            this.snackboxService.showSnackbox(
              'Download completed successfully',
              'success'
            );
          }
        },
        error: (error) => {
          this.cleanupDownload(resourceId);
          this.snackboxService.showSnackbox(
            `Download failed: ${error.error.message || 'Unknown error'}`,
            'error'
          );
        },
      });
  }

  private handleFileDownload(blob: Blob, fileName: string) {
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    window.URL.revokeObjectURL(url);
  }

  private cleanupDownload(resourceId: string) {
    delete this.downloadProgress[resourceId];
    this.currentDownloads.delete(resourceId);
  }

  onDeleteResource(resource: GetCourseTeachingResourceResponse) {
    this.selectedResource = resource;

    this.deleteDialogMessage = `Are you sure you want to delete the resource "${
      resource.resourceUrl ? resource.resourceUrl : resource.fileName
    }"?`;
    this.showDeleteDialog = true;
  }

  onCancelDeleteDialog() {
    this.showDeleteDialog = false;
  }

  deleteResource() {
    this.showDeleteDialog = false;
    this.deleteDialogMessage = '';
    if (!this.selectedResource?.courseTeachingResourceId) return;

    this.courseTutorControllerService
      .deleteCourseTeachingResource(
        this.selectedResource?.courseTeachingResourceId as string
      )
      .subscribe({
        next: (response) => {
          this.snackboxService.showSnackbox(
            'Resource deleted successfully',
            'success'
          );
          this.selectedResource = null;
          this.loadAllResources();
        },
        error: (error) => {
          this.snackboxService.showSnackbox(
            `Failed to delete resource${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );

          this.selectedResource = null;
        },
      });
  }
}
