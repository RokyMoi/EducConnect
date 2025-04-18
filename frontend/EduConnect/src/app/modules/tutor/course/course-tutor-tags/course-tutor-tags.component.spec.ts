import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseTutorTagsComponent } from './course-tutor-tags.component';

describe('CourseTutorTagsComponent', () => {
  let component: CourseTutorTagsComponent;
  let fixture: ComponentFixture<CourseTutorTagsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseTutorTagsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseTutorTagsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
