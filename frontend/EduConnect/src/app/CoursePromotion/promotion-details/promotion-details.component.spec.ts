import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PromotionDetailsComponent } from './promotion-details.component';

describe('PromotionDetailsComponent', () => {
  let component: PromotionDetailsComponent;
  let fixture: ComponentFixture<PromotionDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PromotionDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PromotionDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
