import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCourseDetailsComponent } from './create-course-details.component';

describe('CreateCourseDetailsComponent', () => {
  let component: CreateCourseDetailsComponent;
  let fixture: ComponentFixture<CreateCourseDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateCourseDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateCourseDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
