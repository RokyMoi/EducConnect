import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EducationLogComponent } from './education-log.component';

describe('EducationLogComponent', () => {
  let component: EducationLogComponent;
  let fixture: ComponentFixture<EducationLogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EducationLogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EducationLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
