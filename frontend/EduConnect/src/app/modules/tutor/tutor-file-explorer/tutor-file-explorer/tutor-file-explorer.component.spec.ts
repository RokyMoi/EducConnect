import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TutorFileExplorerComponent } from './tutor-file-explorer.component';

describe('TutorFileExplorerComponent', () => {
  let component: TutorFileExplorerComponent;
  let fixture: ComponentFixture<TutorFileExplorerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TutorFileExplorerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TutorFileExplorerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
