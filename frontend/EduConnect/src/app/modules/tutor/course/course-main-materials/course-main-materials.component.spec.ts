import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseMainMaterialsComponent } from './course-main-materials.component';

describe('CourseMainMaterialsComponent', () => {
  let component: CourseMainMaterialsComponent;
  let fixture: ComponentFixture<CourseMainMaterialsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseMainMaterialsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseMainMaterialsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
