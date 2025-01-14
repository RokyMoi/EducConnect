import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseLessonsComponent } from './course-lessons.component';

describe('CourseLessonsComponent', () => {
  let component: CourseLessonsComponent;
  let fixture: ComponentFixture<CourseLessonsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseLessonsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseLessonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
