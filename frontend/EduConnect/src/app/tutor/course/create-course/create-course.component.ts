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
import { CourseSupportedLanguagesComponent } from '../course-supported-languages/course-supported-languages.component';
import { CourseCreateService } from '../../../services/course/course-create-service.service';

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
  courseCreateService = inject(CourseCreateService);

  @ViewChild('dynamicContainerComponent', { read: ViewContainerRef })
  container!: ViewContainerRef;
  sidebarTitle: string = 'Steps';

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
    CourseSupportedLanguages: CourseSupportedLanguagesComponent,
  };

  selectedOption = 0;
  showSidebar: boolean = true;
  sidebarWidth: string = '25%';
  sidebarPadding: string = '20px';
  sidebarHeight: string = '100%';
  sidebarWindowWidth: number = window.innerWidth;

  //Variable to store the course id after the course is created
  courseId: string = '';
  //Variable flag to determine if the course is being created or edited
  //True - for create
  //False - for edit
  isCreateOrEditMode: boolean = true;

  // Add a new property to track the previous valid index
  previousValidIndex: number = 0;

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
    console.log(componentName);
    console.log('Course to edit', this.courseId);
    console.log(
      'Component is in',
      this.isCreateOrEditMode ? 'create' : 'edit',
      'mode'
    );

    if (this.isCreateOrEditMode && componentName !== 'CourseBasicInformation') {
      console.log('Basic information is required first');
      this.selectedOption = this.previousValidIndex; // Reset to previous valid index
      this.showFloatingWarningBox = true;
      this.floatingWarningBoxMessage =
        'We need you to fill in the basic information first';
      this.floatingWarningBoxMessageColor = 'red';
      this.floatingWarningBoxTitle = 'Cannot proceed';
      setTimeout(() => {
        this.showFloatingWarningBox = false;
      }, 3000);
      return;
    }

    // If navigation is allowed, update the previous valid index
    this.previousValidIndex = this.selectedOption;

    var componentType = this.componentsMap[componentName];
    this.container.clear();

    if (componentType) {
      const componentRef = this.container.createComponent(componentType);
      if (componentType === CourseBasicInformationComponent) {
        componentRef.instance.referenceService = this.referenceService;
        componentRef.instance.isCreateOrEditMode = this.isCreateOrEditMode;
        componentRef.instance.courseId = this.courseId;
        componentRef.instance.provideCourseId.subscribe((courseId: string) => {
          this.courseId = courseId;
          this.isCreateOrEditMode = false;
          console.log('Provided course id: ' + this.courseId);
        });
        componentRef.instance.goToNextStep.subscribe(() => {
          this.onSelectComponent('CourseSupportedLanguages');
        });
      }
      console.log('Component opened');
      if (componentType === CourseSupportedLanguagesComponent) {
        componentRef.instance.referenceService = this.referenceService;
        componentRef.instance.createCourseService = this.courseCreateService;
        componentRef.instance.courseId = this.courseId;
      }
    }
  }
}
