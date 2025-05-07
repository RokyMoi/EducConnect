import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VoiceInjectorComponent } from './voice-injector.component';

describe('VoiceInjectorComponent', () => {
  let component: VoiceInjectorComponent;
  let fixture: ComponentFixture<VoiceInjectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VoiceInjectorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VoiceInjectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
