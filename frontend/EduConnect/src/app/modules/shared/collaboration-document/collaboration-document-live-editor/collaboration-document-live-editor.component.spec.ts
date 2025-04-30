import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CollaborationDocumentLiveEditorComponent } from './collaboration-document-live-editor.component';

describe('CollaborationDocumentLiveEditorComponent', () => {
  let component: CollaborationDocumentLiveEditorComponent;
  let fixture: ComponentFixture<CollaborationDocumentLiveEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CollaborationDocumentLiveEditorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CollaborationDocumentLiveEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
