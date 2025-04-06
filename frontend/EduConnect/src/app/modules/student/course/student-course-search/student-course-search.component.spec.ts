import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentCourseSearchComponent } from './student-course-search.component';

describe('StudentCourseSearchComponent', () => {
  let component: StudentCourseSearchComponent;
  let fixture: ComponentFixture<StudentCourseSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentCourseSearchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentCourseSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
