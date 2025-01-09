import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectDropdownGroupsComponent } from './select-dropdown-groups.component';

describe('SelectDropdownGroupsComponent', () => {
  let component: SelectDropdownGroupsComponent;
  let fixture: ComponentFixture<SelectDropdownGroupsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SelectDropdownGroupsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SelectDropdownGroupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
