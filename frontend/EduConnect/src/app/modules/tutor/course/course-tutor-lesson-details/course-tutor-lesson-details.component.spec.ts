import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseTutorLessonDetailsComponent } from './course-tutor-lesson-details.component';

describe('CourseTutorLessonDetailsComponent', () => {
  let component: CourseTutorLessonDetailsComponent;
  let fixture: ComponentFixture<CourseTutorLessonDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseTutorLessonDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseTutorLessonDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
