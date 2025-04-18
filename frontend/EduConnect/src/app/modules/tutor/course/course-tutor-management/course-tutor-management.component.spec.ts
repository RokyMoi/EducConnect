import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseTutorManagementComponent } from './course-tutor-management.component';

describe('CourseTutorManagementComponent', () => {
  let component: CourseTutorManagementComponent;
  let fixture: ComponentFixture<CourseTutorManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseTutorManagementComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseTutorManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
