import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FloatingWarningBoxComponent } from './floating-warning-box.component';

describe('FloatingWarningBoxComponent', () => {
  let component: FloatingWarningBoxComponent;
  let fixture: ComponentFixture<FloatingWarningBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FloatingWarningBoxComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FloatingWarningBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
