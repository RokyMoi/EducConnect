import { NgClass, NgFor } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-left-side-bar',
  imports: [NgFor, NgClass],
  templateUrl: './left-side-bar.component.html',
  styleUrl: './left-side-bar.component.css',
})
export class LeftSideBarComponent {
  @Input() sidebarTitle: string = 'Title';
  @Input() sidebarWidth = '25%';
  @Input() sidebarPadding = '0px';
  @Input() sidebarOptions: { title: string; link: string }[] = [];
  @Input() selectedOption: number = 0;
  @Input() showSidebar: boolean = true;

  selectOption(index: number) {
    this.selectedOption = index;
  }
}
