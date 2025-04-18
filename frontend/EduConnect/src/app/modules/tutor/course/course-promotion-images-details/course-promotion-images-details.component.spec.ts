import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoursePromotionImagesDetailsComponent } from './course-promotion-images-details.component';

describe('CoursePromotionImagesDetailsComponent', () => {
  let component: CoursePromotionImagesDetailsComponent;
  let fixture: ComponentFixture<CoursePromotionImagesDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CoursePromotionImagesDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CoursePromotionImagesDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
