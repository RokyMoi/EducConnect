import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DynamicFormSettingsComponent } from './dynamic-form-settings.component';

describe('DynamicFormSettingsComponent', () => {
  let component: DynamicFormSettingsComponent;
  let fixture: ComponentFixture<DynamicFormSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DynamicFormSettingsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DynamicFormSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
