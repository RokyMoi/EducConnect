import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { PromotionService } from '../../services/promotion/course-promotion.service';
import { CoursePromotionDetail, PromotionStatus } from '../../models/promotionCourse/promotion.model';
import { ConfirmDialogComponent } from '../../../../shared/confirm-dialog/confirm-dialog.component';
import { DatePipe, NgClass, NgFor, NgIf, LowerCasePipe } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-promotion-details',
  templateUrl: './promotion-details.component.html',
  styleUrls: ['./promotion-details.component.css'],
  standalone: true,
  imports: [
    NgClass,
    NgFor,
    NgIf,
    MatIcon,
    MatProgressSpinner,
    LowerCasePipe,
    FormsModule
  ],
  providers: [DatePipe]
})
export class PromotionDetailsComponent implements OnInit {
  promotion: CoursePromotionDetail | null = null;
  loading = true;
  mainImageId: string | null = null;
  error = false;
  PromotionStatus = PromotionStatus;

  statusOptions = [
    { value: PromotionStatus.Draft,     label: 'Draft'     },
    { value: PromotionStatus.Active,    label: 'Active'    },
    { value: PromotionStatus.Paused,    label: 'Paused'    },
    { value: PromotionStatus.Completed, label: 'Completed' },
    { value: PromotionStatus.Canceled,  label: 'Canceled'  }
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private promotionService: PromotionService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private datePipe: DatePipe
  ) { }

  ngOnInit(): void {
    this.loadPromotion();
  }

  loadPromotion(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.error = true;
      this.loading = false;
      return;
    }

    this.promotionService.getPromotionById(id).subscribe({
      next: (data: CoursePromotionDetail) => {
        this.promotion = data;
        // Set initial mainImageId if promotion has images
        if (this.promotion?.images && this.promotion.images.length > 0) {
          this.mainImageId = this.promotion.images[0].imageId;
        }
        this.loading = false;
      },
      error: (error: any) => {
        console.error('Error loading promotion details', error);
        this.snackBar.open('Error loading promotion details', 'Close', { duration: 3000 });
        this.error = true;
        this.loading = false;
      }
    });
  }

  formatDate(timestamp: number | undefined): string {
    if (!timestamp) return 'N/A';
    const date = new Date(timestamp * 1000);
    return this.datePipe.transform(date, 'dd-MM-yyyy') || 'N/A';
  }

  getStatusText(status: PromotionStatus | undefined): string {
    if (status === undefined) return 'Unknown';  // Default fallback
    return this.statusOptions.find(s => s.value === status)?.label || 'Unknown';
  }

  getStatusClass(status: PromotionStatus | undefined): string {
    if (status === undefined) return '';  // Default fallback to empty string
    switch (status) {
      case PromotionStatus.Draft:     return 'status-draft';
      case PromotionStatus.Active:    return 'status-active';
      case PromotionStatus.Paused:    return 'status-paused';
      case PromotionStatus.Completed: return 'status-completed';
      case PromotionStatus.Canceled:  return 'status-canceled';
      default:                        return '';
    }
  }

  getImageUrl(imageId: string | undefined): string {
    if (!imageId) return 'assets/images/placeholder.png';
    return this.promotionService.getImageUrl(imageId);
  }

  setMainImage(imageId: string): void {
    if (!this.promotion || !imageId) return;

    this.mainImageId = imageId;
    this.promotionService.setMainImage(this.promotion.promotionId, imageId).subscribe({
      next: () => {
        this.snackBar.open('Main image updated successfully', 'Close', { duration: 3000 });
        // No need to reload the entire promotion, just update the mainImageId
      },
      error: (error: any) => {
        console.error('Error updating main image', error);
        this.snackBar.open('Error updating main image', 'Close', { duration: 3000 });
      }
    });
  }

  updateStatus(newStatus: PromotionStatus): void {
    if (!this.promotion) return;

    // Don't show confirm dialog if status hasn't changed
    if (this.promotion.status === newStatus) return;

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '350px',
      data: {
        title: 'Confirm Status Change',
        message: `Are you sure you want to change the status to "${this.getStatusText(newStatus)}"?`
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && this.promotion) {
        this.promotionService.updateStatus(this.promotion.promotionId, newStatus).subscribe({
          next: () => {
            this.snackBar.open('Status updated successfully', 'Close', { duration: 3000 });
            if (this.promotion) {
              this.promotion.status = newStatus;
            }
          },
          error: (error: any) => {
            console.error('Error updating status', error);
            this.snackBar.open('Error updating status', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }

  editPromotion(): void {
    if (this.promotion) {
      this.router.navigate(['/promotions/edit', this.promotion.promotionId]);
    }
  }

  deletePromotion(): void {
    if (!this.promotion) return;

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '350px',
      data: {
        title: 'Confirm Delete',
        message: `Are you sure you want to delete promotion "${this.promotion.title}"?`
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && this.promotion) {
        this.promotionService.deletePromotion(this.promotion.promotionId).subscribe({
          next: () => {
            this.snackBar.open('Promotion deleted successfully', 'Close', { duration: 3000 });
            this.router.navigate(['/promotions/list']);
          },
          error: (error: any) => {
            console.error('Error deleting promotion', error);
            this.snackBar.open('Error deleting promotion', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/promotions/list']);
  }
}
