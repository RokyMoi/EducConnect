import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseTutorLessonsComponent } from './course-tutor-lessons.component';

describe('CourseTutorLessonsComponent', () => {
  let component: CourseTutorLessonsComponent;
  let fixture: ComponentFixture<CourseTutorLessonsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseTutorLessonsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseTutorLessonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
