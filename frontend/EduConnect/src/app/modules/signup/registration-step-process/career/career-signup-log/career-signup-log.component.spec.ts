import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CareerSignupLogComponent } from './career-signup-log.component';

describe('CareerSignupLogComponent', () => {
  let component: CareerSignupLogComponent;
  let fixture: ComponentFixture<CareerSignupLogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CareerSignupLogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CareerSignupLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
