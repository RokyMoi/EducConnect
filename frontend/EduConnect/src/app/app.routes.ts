import { Routes } from '@angular/router';
import { BodyComponent } from './landingPage/body/body.component';
import { TutorSignupComponent } from './signup/tutor/tutor-signup/tutor-signup.component';
import { RegisterStudentComponent } from './signup/Student/register-student/register-student.component';
import { LoginComponent } from './signup/login/login.component';
import { StudentDashboardComponent } from './Student/student-dashboard/student-dashboard.component';
import { TutorDashboardComponent } from './tutor/tutor-dashboard/tutor-dashboard.component';

import { LearningComponent } from './Student/learning/learning.component';
import { StudentProfileComponent } from './Student/student-profile/student-profile.component';

import { PhoneNumberComponent } from './signup/registration-step-process/phone-number/phone-number.component';
import { PhotoComponent } from './Photo/photo-comp/photo-comp.component';
import { MainSettingsComponent } from './Settings/main-settings/main-settings.component';
import { MessagesComponent } from './Messenger/messages/messages.component';
import { DirectMessagingsComponent } from './Messenger/direct-messagings/direct-messagings.component';
import { StudentThreadMessageComponent } from './Messenger/student-thread-message/student-thread-message.component';
import { CourseLandingPageComponent } from './Courses/course-landing-page/course-landing-page.component';

import { PersonDetailsComponent } from './signup/registration-step-process/person-details/person-details/person-details.component';
import { EducationComponent } from './signup/registration-step-process/education/education/education.component';
import { CareerSignupComponent } from './signup/registration-step-process/career/career-signup/career-signup/career-signup.component';
import { AvailabilitySignupComponent } from './signup/registration-step-process/availability/availability-signup/availability-signup.component';
import { TutorTeachingStyleComponent } from './signup/registration-step-process/tutor-teaching-style/tutor-teaching-style.component';
<<<<<<< HEAD
import { Component } from '@angular/core';
import { DynamicFormComponent } from './SetingsStudent/dynamic-form/dynamic-form.component';
import { SendMessageComponent } from './Messenger/send-message/send-message.component';
import { ListOfUsersComponent } from './Messenger/list-of-users/list-of-users.component';
import { CartItemsComponent } from './ShoppingCart/cart-items/cart-items.component';
import { WishlistComponent } from './ShoppingCart/wishlist/wishlist.component';

export const routes: Routes = [
  { path: '', component: BodyComponent },
  {path: "settings",component:MainSettingsComponent},
  { path: 'student-register', component: RegisterStudentComponent },
  { path: 'tutor-signup', component: TutorSignupComponent },
  { path: 'user-signin', component: LoginComponent },
  {path: 'student-profile', component: StudentProfileComponent},
  {path: 'learning-student', component: LearningComponent},
  { path: 'student-dashboard', component: StudentDashboardComponent },
  { path: 'tutor-dashboard', component: TutorDashboardComponent },
  {path: 'dynamicForm',component: DynamicFormComponent},
  {path: 'student-message-preview',component: MessagesComponent},
  {path: 'direct-message',component: DirectMessagingsComponent},
  {path: 'studentMessageThread/:id',component: StudentThreadMessageComponent},
  {path: 'viewOfAllCourses',component:CourseLandingPageComponent},
  {path: 'send-message',component:SendMessageComponent},
  {path: 'Shopping-Cart', component: CartItemsComponent},
  {path: 'course-wishlist', component: WishlistComponent},
  {path: 'ListOfUsers', component: ListOfUsersComponent},
 
=======
import { CreateCourseComponent } from './tutor/course/create-course/create-course.component';
import { CourseSupportedLanguagesComponent } from './tutor/course/course-supported-languages/course-supported-languages.component';

export const routes: Routes = [
  { path: '', component: BodyComponent },
  { path: 'settings', component: MainSettingsComponent },
  { path: 'student-register', component: RegisterStudentComponent },
  { path: 'tutor-signup', component: TutorSignupComponent },
  { path: 'user-signin', component: LoginComponent },
  { path: 'student-profile', component: StudentProfileComponent },
  { path: 'learning-student', component: LearningComponent },
  { path: 'student-dashboard', component: StudentDashboardComponent },
  { path: 'tutor-dashboard', component: TutorDashboardComponent },
  { path: 'photouploadcomponent', component: PhotoComponent },
  { path: 'student-message-preview', component: MessagesComponent },
  { path: 'direct-message', component: DirectMessagingsComponent },
  {
    path: 'studentMessageThread/:id',
    component: StudentThreadMessageComponent,
  },
  { path: 'viewOfAllCourses', component: CourseLandingPageComponent },

>>>>>>> d93e7ce8e2cd19478839a575de236d1244ad0fd8
  {
    path: 'signup/phone-number',
    component: PhoneNumberComponent,
  },
  {
    path: 'signup/personal-information',
    component: PersonDetailsComponent,
  },
  {
    path: 'signup/education',
    component: EducationComponent,
  },
  {
    path: 'signup/career',
    component: CareerSignupComponent,
  },
  {
    path: 'signup/availability',
    component: AvailabilitySignupComponent,
  },
  {
    path: 'signup/tutor/teaching-style',
    component: TutorTeachingStyleComponent,
  },
  {
    path: 'tutor/course/create',
    component: CreateCourseComponent,
  },
  {
    path: 'tutor/course/supported-languages',
    component: CourseSupportedLanguagesComponent,
  },
  { path: '**', component: BodyComponent },
];


