import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentCourseDetailsComponent } from './student-course-details.component';

describe('StudentCourseDetailsComponent', () => {
  let component: StudentCourseDetailsComponent;
  let fixture: ComponentFixture<StudentCourseDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentCourseDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentCourseDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
