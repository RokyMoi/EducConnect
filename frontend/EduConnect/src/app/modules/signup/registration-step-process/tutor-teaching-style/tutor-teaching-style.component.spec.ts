import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TutorTeachingStyleComponent } from './tutor-teaching-style.component';

describe('TutorTeachingStyleComponent', () => {
  let component: TutorTeachingStyleComponent;
  let fixture: ComponentFixture<TutorTeachingStyleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TutorTeachingStyleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TutorTeachingStyleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
