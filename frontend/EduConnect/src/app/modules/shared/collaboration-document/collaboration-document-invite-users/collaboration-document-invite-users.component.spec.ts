import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CollaborationDocumentInviteUsersComponent } from './collaboration-document-invite-users.component';

describe('CollaborationDocumentInviteUsersComponent', () => {
  let component: CollaborationDocumentInviteUsersComponent;
  let fixture: ComponentFixture<CollaborationDocumentInviteUsersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CollaborationDocumentInviteUsersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CollaborationDocumentInviteUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
