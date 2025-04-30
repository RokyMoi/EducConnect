import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CollaborationDocumentDashboardComponent } from './collaboration-document-dashboard.component';

describe('CollaborationDocumentDashboardComponent', () => {
  let component: CollaborationDocumentDashboardComponent;
  let fixture: ComponentFixture<CollaborationDocumentDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CollaborationDocumentDashboardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CollaborationDocumentDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
