import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CareerSignupComponent } from './career-signup.component';

describe('CareerSignupComponent', () => {
  let component: CareerSignupComponent;
  let fixture: ComponentFixture<CareerSignupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CareerSignupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CareerSignupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
