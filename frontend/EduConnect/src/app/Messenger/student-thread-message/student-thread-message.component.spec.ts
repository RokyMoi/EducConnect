import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentThreadMessageComponent } from './student-thread-message.component';

describe('StudentThreadMessageComponent', () => {
  let component: StudentThreadMessageComponent;
  let fixture: ComponentFixture<StudentThreadMessageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentThreadMessageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentThreadMessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
