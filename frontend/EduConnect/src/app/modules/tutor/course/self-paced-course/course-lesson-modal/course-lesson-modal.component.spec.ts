import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseLessonModalComponent } from './course-lesson-modal.component';

describe('CourseLessonModalComponent', () => {
  let component: CourseLessonModalComponent;
  let fixture: ComponentFixture<CourseLessonModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseLessonModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseLessonModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
