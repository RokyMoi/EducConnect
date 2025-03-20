import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseThumbnailComponent } from './course-thumbnail.component';

describe('CourseThumbnailComponent', () => {
  let component: CourseThumbnailComponent;
  let fixture: ComponentFixture<CourseThumbnailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseThumbnailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseThumbnailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
