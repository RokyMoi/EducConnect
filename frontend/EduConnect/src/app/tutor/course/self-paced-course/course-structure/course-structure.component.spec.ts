import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseStructureComponent } from './course-structure.component';

describe('CourseStructureComponent', () => {
  let component: CourseStructureComponent;
  let fixture: ComponentFixture<CourseStructureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseStructureComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseStructureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
