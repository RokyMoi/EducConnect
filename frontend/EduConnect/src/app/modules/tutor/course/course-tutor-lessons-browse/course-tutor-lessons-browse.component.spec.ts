import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseTutorLessonsBrowseComponent } from './course-tutor-lessons-browse.component';

describe('CourseTutorLessonsBrowseComponent', () => {
  let component: CourseTutorLessonsBrowseComponent;
  let fixture: ComponentFixture<CourseTutorLessonsBrowseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseTutorLessonsBrowseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseTutorLessonsBrowseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
