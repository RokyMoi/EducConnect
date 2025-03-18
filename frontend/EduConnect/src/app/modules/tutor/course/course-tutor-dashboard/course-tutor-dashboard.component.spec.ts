import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseTutorDashboardComponent } from './course-tutor-dashboard.component';

describe('CourseTutorDashboardComponent', () => {
  let component: CourseTutorDashboardComponent;
  let fixture: ComponentFixture<CourseTutorDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseTutorDashboardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseTutorDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
