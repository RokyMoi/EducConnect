import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseTutorPromotionImagesComponent } from './course-tutor-promotion-images.component';

describe('CourseTutorPromotionImagesComponent', () => {
  let component: CourseTutorPromotionImagesComponent;
  let fixture: ComponentFixture<CourseTutorPromotionImagesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseTutorPromotionImagesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseTutorPromotionImagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
