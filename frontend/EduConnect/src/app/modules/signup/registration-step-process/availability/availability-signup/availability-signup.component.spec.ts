import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailabilitySignupComponent } from './availability-signup.component';

describe('AvailabilitySignupComponent', () => {
  let component: AvailabilitySignupComponent;
  let fixture: ComponentFixture<AvailabilitySignupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AvailabilitySignupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AvailabilitySignupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
