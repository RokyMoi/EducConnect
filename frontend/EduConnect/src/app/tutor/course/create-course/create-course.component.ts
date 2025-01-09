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

  @ViewChild(LeftSideBarComponent) leftSidebar!: LeftSideBarComponent;

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

  //Flag variable to determine if the basic information step has been completed
  isBasicInformationStepCompleted: boolean = false;
  //Flag variable to determine if the supported languages step has been completed
  isSupportedLanguagesStepCompleted: boolean = false;

  ngAfterViewInit() {
    // Load initial component
    this.onSelectComponent('CourseBasicInformation');
  }

  toggleSidebar() {
    this.showSidebar = !this.showSidebar;
  }
  onSelectIndex(index: number) {
    this.selectedOption = index;
    console.log('Selected index:', index);
  }
  onSelectComponent(componentName: string) {
    console.log('Selected component: ', componentName);

    this.container.clear();
    const componentType = this.componentsMap[componentName];
    if (componentType) {
      const componentRef = this.container.createComponent(componentType);

      //Pass and receive values from the CourseBasicInformationComponent
      if (componentName === 'CourseBasicInformation') {
        componentRef.instance.courseId = this.courseId;
        componentRef.instance.isCreateOrEditMode = this.isCreateOrEditMode;
        componentRef.instance.referenceService = this.referenceService;

        componentRef.instance.provideCourseId.subscribe((courseId: string) => {
          this.courseId = courseId;
          console.log('Course Id: ', courseId);
          this.isBasicInformationStepCompleted = true;
          this.isCreateOrEditMode = false;
        });

        componentRef.instance.goToNextStep.subscribe(() => {
          this.handleOptionChangeRequest({
            option: 'CourseSupportedLanguages',
            index: 1,
          });
          this.onSelectComponent('CourseSupportedLanguages');
        });
      }

      //Pass and receive values from the CourseSupportedLanguagesComponent
      if (componentName === 'CourseSupportedLanguages') {
        console.log(
          'Is create course service initialized:',
          this.courseCreateService
        );
        componentRef.instance.courseId = this.courseId;
        componentRef.instance.isCreateOrEditMode = this.isCreateOrEditMode;
        componentRef.instance.referenceService = this.referenceService;
        componentRef.instance.createCourseService = this.courseCreateService;
      }
    }
  }
  handleOptionChangeRequest(event: { option: string; index: number }) {
    // Check if user can switch based on current step
    if (event.index === 1 && !this.isBasicInformationStepCompleted) {
      console.log(
        'Cannot switch to supported languages - complete basic information first'
      );
      return;
    }

    this.selectedOption = event.index;
    this.leftSidebar.updateSelectedIndex(event.index);
    this.onSelectComponent(event.option);
  }
}
