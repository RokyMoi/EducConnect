import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseTutorTeachingResourcesDetailsComponent } from './course-tutor-teaching-resources-details.component';

describe('CourseTutorTeachingResourcesDetailsComponent', () => {
  let component: CourseTutorTeachingResourcesDetailsComponent;
  let fixture: ComponentFixture<CourseTutorTeachingResourcesDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseTutorTeachingResourcesDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseTutorTeachingResourcesDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
