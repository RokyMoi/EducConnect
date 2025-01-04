import { NgClass, NgFor } from '@angular/common';
import {
  Component,
  HostListener,
  OnDestroy,
  OnInit,
  Renderer2,
} from '@angular/core';
import { LeftSideBarComponent } from '../../../common/side-bar/left-side-bar/left-side-bar.component';

@Component({
  standalone: true,
  selector: 'app-create-course',
  imports: [NgFor, NgClass, LeftSideBarComponent],
  templateUrl: './create-course.component.html',
  styleUrl: './create-course.component.css',
})
export class CreateCourseComponent {
  sidebarTitle: string = 'Steps';
  courseCreationSteps = [
    {
      title: 'Step 1: Basic Information',
      link: 'no-link',
    },
    {
      title: 'Step 2: Supported languages',
      link: 'no-link',
    },
    {
      title: 'Step 3: Course Type',
      link: 'no-link',
    },
    {
      title: 'Step 3: Materials and Resources',
      link: 'no-link',
    },
    {
      title: 'Step 4: Schedule and Enrollment/Lesson Plan',
      link: 'no-link',
    },
    {
      title: 'Step 5: Course Testing and Evaluation',
      link: 'no-link',
    },
    {
      title: 'Step 6: Course Promotion and Marketing',
      link: 'no-link',
    },
    {
      title: 'Step 7: Pricing and Payment',
      link: 'no-link',
    },
    {
      title: 'Step 6: Review and Publish',
      link: 'no-link',
    },
    {
      title: 'Step 6: Review and Publish',
      link: 'no-link',
    },
    {
      title: 'Step 6: Review and Publish',
      link: 'no-link',
    },
    {
      title: 'Step 6: Review and Publish',
      link: 'no-link',
    },
    {
      title: 'Step 6: Review and Publish',
      link: 'no-link',
    },
    {
      title: 'Step 6: Review and Publish',
      link: 'no-link',
    },
  ];

  selectedOption = 0;
  showSidebar: boolean = true;
  sidebarWidth: string = '25%';
  sidebarPadding: string = '20px';
  sidebarHeight: string = '100%';
  sidebarWindowWidth: number = window.innerWidth;
  selectOption(index: number) {
    this.selectedOption = index;
  }

  toggleSidebar() {
    console.log('Sidebar is ' + (this.showSidebar ? 'close' : 'open'));
    this.showSidebar = !this.showSidebar;
    this.sidebarWindowWidth = window.innerWidth;
    console.log('Window width: ' + this.sidebarWindowWidth);
    this.sidebarPadding = this.showSidebar ? '20px' : '0px';
  }
  onStepChange(link: string) {
    console.log(`Navigating to ${link}`);
  }
}
