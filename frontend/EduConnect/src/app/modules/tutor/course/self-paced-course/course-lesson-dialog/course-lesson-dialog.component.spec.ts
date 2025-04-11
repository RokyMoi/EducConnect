import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseLessonDialogComponent } from './course-lesson-dialog.component';

describe('CourseLessonDialogComponent', () => {
  let component: CourseLessonDialogComponent;
  let fixture: ComponentFixture<CourseLessonDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseLessonDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseLessonDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
