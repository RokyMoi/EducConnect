import { Routes } from '@angular/router';
import { BodyComponent } from './landingPage/body/body.component';
import { TutorSignupComponent } from './signup/tutor/tutor-signup/tutor-signup.component';
import { RegisterStudentComponent } from './signup/Student/register-student/register-student.component';
import { LoginComponent } from './signup/login/login.component';
import { StudentDashboardComponent } from './Student/student-dashboard/student-dashboard.component';
import { TutorDashboardComponent } from './tutor/tutor-dashboard/tutor-dashboard.component';
import { LearningComponent } from './Student/learning/learning.component';
import { StudentProfileComponent } from './Student/student-profile/student-profile.component';
import { roleGuardRedirectGuard } from './_guards/role-guard-redirect.guard';




export const routes: Routes = [
    { path: '', component:BodyComponent,canActivate: [roleGuardRedirectGuard] },
    { path: 'student-register', component: RegisterStudentComponent },
    { path: 'tutor-signup', component: TutorSignupComponent },
    {path: 'user-signin',component:LoginComponent},
    {path:'student-dashboard',component:StudentDashboardComponent},
    {path:'tutor-dashboard',component:TutorDashboardComponent},
    {path:'student-profile',component: StudentProfileComponent},
    {path:'learning-student',component: LearningComponent},
    { path: '**', component:BodyComponent }
    

];