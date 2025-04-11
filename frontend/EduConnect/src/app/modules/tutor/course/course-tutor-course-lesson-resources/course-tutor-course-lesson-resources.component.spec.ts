import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseTutorCourseLessonResourcesComponent } from './course-tutor-course-lesson-resources.component';

describe('CourseTutorCourseLessonResourcesComponent', () => {
  let component: CourseTutorCourseLessonResourcesComponent;
  let fixture: ComponentFixture<CourseTutorCourseLessonResourcesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseTutorCourseLessonResourcesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseTutorCourseLessonResourcesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
