import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailabilityRecordsTableComponent } from './availability-records-table.component';

describe('AvailabilityRecordsTableComponent', () => {
  let component: AvailabilityRecordsTableComponent;
  let fixture: ComponentFixture<AvailabilityRecordsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AvailabilityRecordsTableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AvailabilityRecordsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
