import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TutorSignupComponent } from './tutor-signup.component';

describe('TutorSignupComponent', () => {
  let component: TutorSignupComponent;
  let fixture: ComponentFixture<TutorSignupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TutorSignupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TutorSignupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
