import { NgClass, NgFor } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-left-side-bar',
  imports: [NgFor, NgClass],
  templateUrl: './left-side-bar.component.html',
  styleUrl: './left-side-bar.component.css',
})
export class LeftSideBarComponent {
  @Input() sidebarItems: { title: string; link: string }[] = [];
  @Input() sidebarTitle: string = 'Title';
  @Input() showSidebar: boolean = true;
  @Input() selectedIndex: number = 0;

  @Output() requestOptionChange: EventEmitter<{
    option: string;
    index: number;
  }> = new EventEmitter<{ option: string; index: number }>();

  selectOption(option: string, index: number) {
    this.requestOptionChange.emit({ option, index });
  }

  updateSelectedIndex(index: number) {
    this.selectedIndex = index;
  }
}
