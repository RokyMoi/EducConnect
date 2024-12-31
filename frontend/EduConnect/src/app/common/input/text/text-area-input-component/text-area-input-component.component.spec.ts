import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextAreaInputComponentComponent } from './text-area-input-component.component';

describe('TextAreaInputComponentComponent', () => {
  let component: TextAreaInputComponentComponent;
  let fixture: ComponentFixture<TextAreaInputComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TextAreaInputComponentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TextAreaInputComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
