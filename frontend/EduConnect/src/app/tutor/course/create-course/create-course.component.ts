import { NgClass, NgFor, NgIf } from '@angular/common';
import {
  Component,
  inject,
  ViewChild,
  ViewContainerRef,
  Type,
} from '@angular/core';
import { LeftSideBarComponent } from '../../../common/side-bar/left-side-bar/left-side-bar.component';
import { CourseBasicInformationComponent } from '../course-basic-information/course-basic-information.component';
import { AccountService } from '../../../services/account.service';
import { ReferenceService } from '../../../services/reference/reference.service';
import { CareerSignupComponent } from '../../../signup/registration-step-process/career/career-signup/career-signup/career-signup.component';
import { FloatingWarningBoxComponent } from '../../../common/floating-warning-box/floating-warning-box/floating-warning-box.component';

@Component({
  standalone: true,
  selector: 'app-create-course',
  imports: [
    NgFor,
    NgIf,
    NgClass,
    LeftSideBarComponent,
    CourseBasicInformationComponent,
    FloatingWarningBoxComponent,
  ],
  templateUrl: './create-course.component.html',
  styleUrl: './create-course.component.css',
})
export class CreateCourseComponent {
  accountService = inject(AccountService);
  referenceService = inject(ReferenceService);

  @ViewChild('dynamicContainerComponent', { read: ViewContainerRef })
  container!: ViewContainerRef;
  sidebarTitle: string = 'Steps';

  isCourseCreated: boolean = false;

  showFloatingWarningBox: boolean = false;
  floatingWarningBoxMessage: string = '';
  floatingWarningBoxMessageColor: string = '';
  floatingWarningBoxTitle: string = '';

  courseCreationSteps = [
    {
      title: 'Step 1: Basic Information',
      link: 'CourseBasicInformation',
    },
    {
      title: 'Step 2: Supported languages',
      link: 'CourseSupportedLanguages',
    },
  ];

  componentsMap: { [key: string]: Type<any> } = {
    CourseBasicInformation: CourseBasicInformationComponent,
    CourseSupportedLanguages: CareerSignupComponent,
  };

  selectedOption = 0;
  showSidebar: boolean = true;
  sidebarWidth: string = '25%';
  sidebarPadding: string = '20px';
  sidebarHeight: string = '100%';
  sidebarWindowWidth: number = window.innerWidth;
  selectOption(index: number) {
    this.selectedOption = index;
  }

  ngAfterViewInit() {
    // Load initial component
    this.onSelectComponent('CourseBasicInformation');
  }

  toggleSidebar() {
    this.showSidebar = !this.showSidebar;
  }
  onSelectComponent(componentName: string) {
    if (!this.isCourseCreated && componentName !== 'CourseBasicInformation') {
      this.selectedOption = 0;
      console.log('Please fill in the basic information first.');
      this.floatingWarningBoxTitle = 'Course must have basic information';
      this.floatingWarningBoxMessage =
        'Please fill in the basic information first.';
      this.floatingWarningBoxMessageColor = 'red';
      this.showFloatingWarningBox = true;
      setTimeout(() => {
        this.floatingWarningBoxMessage = '';
        this.floatingWarningBoxTitle = '';
        this.floatingWarningBoxMessageColor = '';
        this.showFloatingWarningBox = false;
      }, 3000);
      return;
    }
    console.log(componentName);
    this.container.clear();
    const componentType = this.componentsMap[componentName];
    if (componentType) {
      const componentRef = this.container.createComponent(componentType);
      if (componentType === CourseBasicInformationComponent) {
        componentRef.instance.referenceService = this.referenceService;
        componentRef.instance.courseCreated.subscribe((created: boolean) => {
          this.isCourseCreated = created;
        });
        componentRef.instance.nextStep.subscribe(() => {
          this.selectedOption = 1;
          this.onSelectComponent('CourseSupportedLanguages');
        });
      }
    }
  }
}
