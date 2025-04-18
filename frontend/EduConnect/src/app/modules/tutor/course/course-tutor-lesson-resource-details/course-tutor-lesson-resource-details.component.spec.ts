import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseTutorLessonResourceDetailsComponent } from './course-tutor-lesson-resource-details.component';

describe('CourseTutorLessonResourceDetailsComponent', () => {
  let component: CourseTutorLessonResourceDetailsComponent;
  let fixture: ComponentFixture<CourseTutorLessonResourceDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseTutorLessonResourceDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseTutorLessonResourceDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
