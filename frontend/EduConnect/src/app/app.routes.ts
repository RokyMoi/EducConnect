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
import { PersonDetailsComponent } from './signup/registration-step-process/person-details/person-details/person-details.component';
import { EducationComponent } from './signup/registration-step-process/education/education/education.component';
import { CareerSignupComponent } from './signup/registration-step-process/career/career-signup/career-signup/career-signup.component';
import { AvailabilitySignupComponent } from './signup/registration-step-process/availability/availability-signup/availability-signup.component';
import { TutorTeachingStyleComponent } from './signup/registration-step-process/tutor-teaching-style/tutor-teaching-style.component';

export const routes: Routes = [
  { path: '', component: BodyComponent },
  { path: 'student-register', component: RegisterStudentComponent },
  { path: 'tutor-signup', component: TutorSignupComponent },
  { path: 'user-signin', component: LoginComponent },
  { path: 'student-dashboard', component: StudentDashboardComponent },
  { path: 'tutor/dashboard', component: TutorDashboardComponent },
  { path: 'student-register', component: RegisterStudentComponent },
  { path: 'student-dashboard', component: StudentDashboardComponent },
  { path: 'student-profile', component: StudentProfileComponent },
  { path: 'learning-student', component: LearningComponent },
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
  { path: '**', component: BodyComponent },
];
