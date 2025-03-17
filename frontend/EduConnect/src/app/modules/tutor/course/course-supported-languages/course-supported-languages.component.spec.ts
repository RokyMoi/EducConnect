import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseSupportedLanguagesComponent } from './course-supported-languages.component';

describe('CourseSupportedLanguagesComponent', () => {
  let component: CourseSupportedLanguagesComponent;
  let fixture: ComponentFixture<CourseSupportedLanguagesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseSupportedLanguagesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseSupportedLanguagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
