import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmCourseTypeComponent } from './confirm-course-type.component';

describe('ConfirmCourseTypeComponent', () => {
  let component: ConfirmCourseTypeComponent;
  let fixture: ComponentFixture<ConfirmCourseTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfirmCourseTypeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfirmCourseTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
