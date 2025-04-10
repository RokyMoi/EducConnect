import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseTutorAnalyticsDashboardComponent } from './course-tutor-analytics-dashboard.component';

describe('CourseTutorAnalyticsDashboardComponent', () => {
  let component: CourseTutorAnalyticsDashboardComponent;
  let fixture: ComponentFixture<CourseTutorAnalyticsDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseTutorAnalyticsDashboardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseTutorAnalyticsDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
