import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DirectMessagingsComponent } from './direct-messagings.component';

describe('DirectMessagingsComponent', () => {
  let component: DirectMessagingsComponent;
  let fixture: ComponentFixture<DirectMessagingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DirectMessagingsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DirectMessagingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
