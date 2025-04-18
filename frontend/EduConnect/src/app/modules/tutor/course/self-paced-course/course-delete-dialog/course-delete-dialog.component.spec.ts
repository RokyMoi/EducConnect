import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseDeleteDialogComponent } from './course-delete-dialog.component';

describe('CourseDeleteDialogComponent', () => {
  let component: CourseDeleteDialogComponent;
  let fixture: ComponentFixture<CourseDeleteDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseDeleteDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseDeleteDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
