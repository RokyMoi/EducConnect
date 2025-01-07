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
  @Input() selectedOption: number = 0;

  @Output() optionSelected: EventEmitter<string> = new EventEmitter<string>();

  selectOption(option: string, index: number) {
    this.selectedOption = index;
    this.optionSelected.emit(option);
  }
}
