import { Routes } from '@angular/router';
import { BodyComponent } from './landingPage/body/body.component';
import { TutorSignupComponent } from './signup/tutor/tutor-signup/tutor-signup.component';
import { RegisterStudentComponent } from './signup/Student/register-student/register-student.component';
import { LoginComponent } from './signup/login/login.component';
import { StudentDashboardComponent } from './Student/student-dashboard/student-dashboard.component';
import { TutorDashboardComponent } from './tutor/tutor-dashboard/tutor-dashboard.component';
<<<<<<< HEAD
import { LearningComponent } from './Student/learning/learning.component';
import { StudentProfileComponent } from './Student/student-profile/student-profile.component';





export const routes: Routes = [
    { path: '', component:BodyComponent},
    { path: 'student-register', component: RegisterStudentComponent },
    { path: 'tutor-signup', component: TutorSignupComponent},
    {path: 'user-signin',component:LoginComponent},
    {path:'student-dashboard',component:StudentDashboardComponent},
    {path:'tutor-dashboard',component:TutorDashboardComponent},
    {path:'student-profile',component: StudentProfileComponent},
    {path:'learning-student',component: LearningComponent},
    { path: '**', component:BodyComponent }
    

];
=======
import { PhoneNumberComponent } from './signup/registration-step-process/phone-number/phone-number.component';

export const routes: Routes = [
  { path: '', component: BodyComponent },
  { path: 'student-register', component: RegisterStudentComponent },
  { path: 'tutor-signup', component: TutorSignupComponent },
  { path: 'user-signin', component: LoginComponent },
  { path: 'student-dashboard', component: StudentDashboardComponent },
  { path: 'tutor-dashboard', component: TutorDashboardComponent },
  {
    path: 'signup/phone-number',
    component: PhoneNumberComponent,
  },
  { path: '**', component: BodyComponent },
];
>>>>>>> e62e459ed1d7fa44c20fc57eae494a1e84df3398
