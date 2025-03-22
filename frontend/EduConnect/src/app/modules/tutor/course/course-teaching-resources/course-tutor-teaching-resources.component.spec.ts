import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseTutorTeachingResourcesComponent } from './course-tutor-teaching-resources.component';

describe('CourseTeachingResourcesComponent', () => {
  let component: CourseTutorTeachingResourcesComponent;
  let fixture: ComponentFixture<CourseTutorTeachingResourcesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseTutorTeachingResourcesComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CourseTutorTeachingResourcesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
