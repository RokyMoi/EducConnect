import {Component, ChangeDetectionStrategy, inject, OnDestroy, OnInit, ChangeDetectorRef} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatCardModule } from '@angular/material/card';
import {MatTabsModule} from '@angular/material/tabs';
import {CommonModule} from '@angular/common';


import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import {HttpClient} from '@angular/common/http';
import {PromotionService} from '../../services/promotion/course-promotion.service';
import {finalize, tap} from 'rxjs';


@Component({
  selector: 'app-for-you-page',
  standalone: true,
  imports: [CommonModule,MatCardModule, MatButtonModule, MatDividerModule, MatIconModule,MatTabsModule,MatToolbarModule,RouterLink,RouterLinkActive],
  templateUrl: './for-you-page.component.html',
  styleUrls: ['./for-you-page.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ForYouPageComponent implements OnInit{
  promotions: any;
  loading = true;
  constructor(
    protected promotionLoad: PromotionService,
    private cd: ChangeDetectorRef,    // ← inject this,
    private router: Router
  ) {}


  ngOnInit() {
    this.loadPromotions();
  }

  loadPromotions() {
    this.loading = true;
    this.promotionLoad.getPost().pipe(
      tap(response => console.log('Received promotions:', response)),
      finalize(() => {
        this.loading = false;
        this.cd.markForCheck();
      })
    ).subscribe({
      next: (response) => {
        if (Array.isArray(response)) {
          this.promotions = response.map(p => ({
            ...p,
            currentImageIndex: 0
          }));
        } else {
          console.error('Expected array but got:', response);
          this.promotions = [];
        }
        this.cd.markForCheck();
      },
      error: (err) => {
        console.error('Error loading promotions:', err);
        this.cd.markForCheck();
      }
    });
  }





  prevImage(promotion: any) {
    if (promotion.currentImageIndex > 0) {
      promotion.currentImageIndex--;
    } else {
      promotion.currentImageIndex = promotion.images.length - 1;
    }
  }

  nextImage(promotion: any) {
    console.log("Current image index: ", promotion.currentImageIndex)
    console.log('Promotion images:', promotion.images);

    if (promotion.currentImageIndex < promotion.images.length - 1) {
      promotion.currentImageIndex++;
    } else {
      promotion.currentImageIndex = 0;
    }
  }

  nextTutorImage() {

  }

  prevTutorImage() {

  }

  OtvoriKurs(promotion: any) {
    this.router.navigate(
      ['/student/course/details', promotion.courseId]
    );
  }
}
