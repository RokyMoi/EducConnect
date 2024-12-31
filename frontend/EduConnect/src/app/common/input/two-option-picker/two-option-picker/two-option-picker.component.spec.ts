import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TwoOptionPickerComponent } from './two-option-picker.component';

describe('TwoOptionPickerComponent', () => {
  let component: TwoOptionPickerComponent;
  let fixture: ComponentFixture<TwoOptionPickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TwoOptionPickerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TwoOptionPickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
