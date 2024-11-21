import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextInputComponentComponent } from './text-input-component.component';

describe('TextInputComponentComponent', () => {
  let component: TextInputComponentComponent;
  let fixture: ComponentFixture<TextInputComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TextInputComponentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TextInputComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
