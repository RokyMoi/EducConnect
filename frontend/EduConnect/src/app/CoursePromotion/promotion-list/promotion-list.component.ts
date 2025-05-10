import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule, NgClass, NgIf, NgFor, NgSwitch, NgSwitchCase, NgSwitchDefault } from '@angular/common';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PromotionService } from '../../services/promotion/course-promotion.service';
import { CoursePromotion, PromotionStatus } from '../../models/promotionCourse/promotion.model';
import { MatTableDataSource, MatTable, MatColumnDef, MatHeaderCellDef, MatHeaderCell, MatCellDef, MatCell, MatHeaderRowDef, MatHeaderRow, MatRowDef, MatRow } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { SelectionModel } from '@angular/cdk/collections';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-promotion-list',
  templateUrl: './promotion-list.component.html',
  standalone: true,
  imports: [
    // Angular core modules
    CommonModule,
    NgIf,
    NgFor,
    NgClass,
    NgSwitch,
    NgSwitchCase,
    NgSwitchDefault,

    MatTable,
    MatColumnDef,
    MatHeaderCellDef,
    MatHeaderCell,
    MatCellDef,
    MatCell,
    MatHeaderRowDef,
    MatHeaderRow,
    MatRowDef,
    MatRow,
    MatPaginator,
    MatSort,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatInputModule,
    MatMenuModule,
    MatTooltipModule,
    MatProgressSpinnerModule,
  ],
  styleUrls: ['./promotion-list.component.css']
})
export class PromotionListComponent implements OnInit, AfterViewInit {
  promotions: CoursePromotion[] = [];
  dataSource = new MatTableDataSource<CoursePromotion>([]);
  selection = new SelectionModel<CoursePromotion>(true, []);
  loading = true;

  /** Definition of all available columns */
  allColumns = [
    {key: 'select', label: ''},
    {key: 'title', label: 'Title'},
    {key: 'courseName', label: 'Course'},
    {key: 'status', label: 'Status'},
    {key: 'duration', label: 'Duration'},
    {key: 'createdAt', label: 'Created At'},
    {key: 'actions', label: 'Actions'}
  ];
  /** Currently displayed columns */
  displayedColumns = this.allColumns.map(c => c.key);

  PromotionStatus = PromotionStatus;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private promotionService: PromotionService,
    private router: Router,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.initFilterPredicate();
    this.loadPromotions();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  loadPromotions(): void {
    this.loading = true;
    this.promotionService.getAllPromotions().subscribe({
      next: (data: CoursePromotion[]) => {
        this.promotions = data;

        // Calculate duration for each promotion
        data.forEach(promo => {
          promo.duration = this.calculateDuration(promo.startDate, promo.endDate);
        });

        this.dataSource.data = data;
        this.loading = false;
        this.snackBar.open('Promotions loaded', 'OK', {duration: 2000});
      },
      error: (error) => {
        console.error(error);
        this.snackBar.open('Error loading promotions', 'Close', {duration: 3000});
        this.loading = false;
      }
    });
  }

  /**
   * Calculate duration between start and end date
   * @param startTimestamp Unix timestamp in seconds
   * @param endTimestamp Unix timestamp in seconds
   * @returns Formatted duration string
   */
  calculateDuration(startTimestamp?: number, endTimestamp?: number): string {
    if (!startTimestamp || !endTimestamp) {
      return 'N/A';
    }

    // Convert to milliseconds and calculate difference
    const startDate = new Date(startTimestamp * 1000);
    const endDate = new Date(endTimestamp * 1000);

    // Check if dates are valid
    if (isNaN(startDate.getTime()) || isNaN(endDate.getTime())) {
      return 'Invalid date';
    }

    // Check if end date is before start date
    if (endDate < startDate) {
      return 'Invalid period';
    }

    // Calculate difference in days
    const diffTime = Math.abs(endDate.getTime() - startDate.getTime());
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

    // Format duration
    if (diffDays === 0) {
      return 'Same day';
    } else if (diffDays === 1) {
      return '1 day';
    } else if (diffDays < 7) {
      return `${diffDays} days`;
    } else if (diffDays < 31) {
      const weeks = Math.floor(diffDays / 7);
      const remainingDays = diffDays % 7;
      if (weeks === 1) {
        return remainingDays ? `1 week, ${remainingDays} days` : '1 week';
      } else {
        return remainingDays ? `${weeks} weeks, ${remainingDays} days` : `${weeks} weeks`;
      }
    } else {
      const months = Math.floor(diffDays / 30);
      const remainingDays = diffDays % 30;
      if (months === 1) {
        return remainingDays ? `1 month, ${remainingDays} days` : '1 month';
      } else {
        return remainingDays ? `${months} months, ${remainingDays} days` : `${months} months`;
      }
    }
  }

  /** Set up global filter that searches only in title */
  initFilterPredicate() {
    this.dataSource.filterPredicate = (item: CoursePromotion, filter: string) => {
      return item.title.toLowerCase().includes(filter.trim().toLowerCase());
    };
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue;
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  /** Row selection for bulk actions */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.filteredData.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.dataSource.filteredData.forEach(row => this.selection.select(row));
  }

  /** Bulk delete example */
  deleteSelected() {
    const ids = this.selection.selected.map(p => p.promotionId);
    // Here I would call a service for bulk delete
    console.log('Deleting bulk', ids);
    this.selection.clear();
    this.loadPromotions();
  }

  /** Individual actions **/
  editPromotion(p: CoursePromotion) {
    this.router.navigate(['/promotions/edit', p.promotionId]);
  }

  viewPromotion(p: CoursePromotion) {
    this.router.navigate(['/promotions/view', p.promotionId]);
  }

  createPromotion() {
    this.router.navigate(['/promotions/create']);
  }

  deletePromotion(id: string) {
    this.promotionService.deletePromotion(id).subscribe({
      next: () => {
        this.snackBar.open('Deleted', 'Close', {duration: 2000});
        this.loadPromotions();
      },
      error: () => this.snackBar.open('Delete error', 'Close', {duration: 2000})
    });
  }

  updateStatus(p: CoursePromotion, status: PromotionStatus) {
    this.promotionService.updateStatus(p.promotionId, status).subscribe({
      next: () => {
        p.status = status;
        this.snackBar.open('Status updated', 'Close', {duration: 2000});
      },
      error: () => this.snackBar.open('Status error', 'Close', {duration: 2000})
    });
  }

  /** Helper functions for display */
  getStatusText(s: PromotionStatus) {
    switch (s) {
      case PromotionStatus.Draft:
        return 'Draft';
      case PromotionStatus.Active:
        return 'Active';
      case PromotionStatus.Paused:
        return 'Paused';
      case PromotionStatus.Completed:
        return 'Completed';
      case PromotionStatus.Canceled:
        return 'Canceled';
      default:
        return 'Unknown';
    }
  }

  getStatusClass(s: PromotionStatus) {
    return `status-${s.toString().toLowerCase()}`;
  }

  formatDate(ts: number) {
    return ts ? new Date(ts * 1000).toLocaleDateString() : 'N/A';
  }

  /** Export CSV example */
  exportCSV() {
    // Header: title, courseName, status, startDate, endDate, createdAt, duration
    const header = [
      `"Title"`,
      `"Course"`,
      `"Status"`,
      `"Start Date"`,
      `"End Date"`,
      `"Duration"`,
      `"Created At"`
    ].join(',');

    // Rows: take from filteredData
    const rows = this.dataSource.filteredData.map(row => {
      const start = row.startDate ? new Date(row.startDate * 1000).toLocaleDateString() : 'N/A';
      const end = row.endDate ? new Date(row.endDate * 1000).toLocaleDateString() : 'N/A';
      return [
        `"${row.title}"`,
        `"${row.courseName}"`,
        `"${this.getStatusText(row.status)}"`,
        `"${start}"`,
        `"${end}"`,
        `"${row.duration || 'N/A'}"`,
        `"${this.formatDate(row.createdAt)}"`
      ].join(',');
    });

    // Compile CSV
    const csv = [header, ...rows].join('\n');
    const blob = new Blob([csv], {type: 'text/csv'});
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'promotions.csv';
    a.click();
    window.URL.revokeObjectURL(url);
  }
}
