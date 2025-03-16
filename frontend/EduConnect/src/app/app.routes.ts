import { Routes } from '@angular/router';
import { BodyComponent } from './landingPage/body/body.component';
import { RegisterStudentComponent } from './modules/signup/Student/register-student/register-student.component';
import { LoginComponent } from './modules/signup/login/login.component';
import { StudentDashboardComponent } from './Student/student-dashboard/student-dashboard.component';
import { TutorDashboardComponent } from './modules/tutor/tutor-dashboard/tutor-dashboard.component';

import { TutorSignupComponent } from './modules/signup/tutor/tutor-signup/tutor-signup.component';
import { LearningComponent } from './Student/learning/learning.component';
import { StudentProfileComponent } from './Student/student-profile/student-profile.component';

import { PhoneNumberComponent } from './modules/signup/registration-step-process/phone-number/phone-number.component';
import { PhotoComponent } from './modules/Photo/photo-comp/photo-comp.component';
import { MainSettingsComponent } from './modules/Settings/main-settings/main-settings.component';
import { MessagesComponent } from './Messenger/messages/messages.component';
import { DirectMessagingsComponent } from './Messenger/direct-messagings/direct-messagings.component';
import { StudentThreadMessageComponent } from './Messenger/student-thread-message/student-thread-message.component';
import { CourseLandingPageComponent } from './Courses/course-landing-page/course-landing-page.component';

import { PersonDetailsComponent } from './modules/signup/registration-step-process/person-details/person-details/person-details.component';
import { EducationComponent } from './modules/signup/registration-step-process/education/education/education.component';
import { CareerSignupComponent } from './modules/signup/registration-step-process/career/career-signup/career-signup/career-signup.component';

import { AvailabilitySignupComponent } from './modules/signup/registration-step-process/availability/availability-signup/availability-signup.component';
import { TutorTeachingStyleComponent } from './modules/signup/registration-step-process/tutor-teaching-style/tutor-teaching-style.component';
import { CourseSupportedLanguagesComponent } from './modules/tutor/course/course-supported-languages/course-supported-languages.component';
import { CourseMainMaterialsComponent } from './modules/tutor/course/course-main-materials/course-main-materials.component';
import { ConfirmCourseTypeComponent } from './modules/tutor/course/confirm-course-type/confirm-course-type.component';
import { CourseLessonsComponent } from './modules/tutor/course/self-paced-course/course-lessons/course-lessons.component';

import { CreateCourse } from '../../../../.history/frontend/EduConnect/src/app/_models/course/create-course/create-course.create-course.course.model_20250107000517';
import { CreateCourseDetailsComponent } from './modules/tutor/create-course/create-course-details/create-course-details.component';
import { AuthenticationGuardService } from './services/shared/authentication-guard.service';
import { ForbiddenAccessComponent } from './modules/shared/forbidden-access/forbidden-access/forbidden-access.component';
import { AdminDashboardComponent } from './modules/admin/admin-dashboard/admin-dashboard.component';

export const routes: Routes = [
  { path: 'index', component: BodyComponent },
  { path: '', component: BodyComponent },
  { path: 'settings', component: MainSettingsComponent },
  { path: 'student-register', component: RegisterStudentComponent },
  { path: 'tutor-signup', component: TutorSignupComponent },
  { path: 'login', component: LoginComponent },
  { path: 'student-profile', component: StudentProfileComponent },
  { path: 'learning-student', component: LearningComponent },
  { path: 'student/dashboard', component: StudentDashboardComponent },

  { path: 'photouploadcomponent', component: PhotoComponent },
  { path: 'student-message-preview', component: MessagesComponent },
  { path: 'direct-message', component: DirectMessagingsComponent },
  {
    path: 'studentMessageThread/:id',
    component: StudentThreadMessageComponent,
  },
  { path: 'viewOfAllCourses', component: CourseLandingPageComponent },

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
    path: 'tutor/dashboard',
    component: TutorDashboardComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/create',
    component: CreateCourseDetailsComponent,
  },

  //Admin routes
  {
    path: 'admin/dashboard',
    component: AdminDashboardComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'admin' },
  },
  {
    path: 'forbidden',
    component: ForbiddenAccessComponent,
  },
  { path: '**', component: BodyComponent },
];
