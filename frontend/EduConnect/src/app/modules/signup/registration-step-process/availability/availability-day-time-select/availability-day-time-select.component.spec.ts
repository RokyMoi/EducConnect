import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailabilityDayTimeSelectComponent } from './availability-day-time-select.component';

describe('AvailabilityDayTimeSelectComponent', () => {
  let component: AvailabilityDayTimeSelectComponent;
  let fixture: ComponentFixture<AvailabilityDayTimeSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AvailabilityDayTimeSelectComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AvailabilityDayTimeSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
