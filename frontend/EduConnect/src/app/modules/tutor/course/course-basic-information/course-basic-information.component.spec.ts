import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseBasicInformationComponent } from './course-basic-information.component';

describe('CourseBasicInformationComponent', () => {
  let component: CourseBasicInformationComponent;
  let fixture: ComponentFixture<CourseBasicInformationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseBasicInformationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseBasicInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
