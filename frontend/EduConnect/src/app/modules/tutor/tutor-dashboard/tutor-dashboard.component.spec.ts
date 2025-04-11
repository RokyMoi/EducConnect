import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TutorDashboardComponent } from './tutor-dashboard.component';

describe('TutorDashboardComponent', () => {
  let component: TutorDashboardComponent;
  let fixture: ComponentFixture<TutorDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TutorDashboardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TutorDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
