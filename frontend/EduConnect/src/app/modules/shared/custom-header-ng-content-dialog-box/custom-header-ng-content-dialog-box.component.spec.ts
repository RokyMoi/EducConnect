import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomHeaderNgContentDialogBoxComponent } from './custom-header-ng-content-dialog-box.component';

describe('CustomHeaderNgContentDialogBoxComponent', () => {
  let component: CustomHeaderNgContentDialogBoxComponent;
  let fixture: ComponentFixture<CustomHeaderNgContentDialogBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomHeaderNgContentDialogBoxComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomHeaderNgContentDialogBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
