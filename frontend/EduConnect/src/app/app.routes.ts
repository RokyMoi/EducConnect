import { Routes } from '@angular/router';
import { BodyComponent } from './landingPage/body/body.component';
import { TutorSignupComponent } from './signup/tutor/tutor-signup/tutor-signup.component';
import { RegisterStudentComponent } from './signup/Student/register-student/register-student.component';
import { LoginComponent } from './signup/login/login.component';
import { StudentDashboardComponent } from './Student/student-dashboard/student-dashboard.component';




export const routes: Routes = [
    { path: '', component:BodyComponent },
    { path: 'student-register', component: RegisterStudentComponent },
    { path: 'tutor-signup', component: TutorSignupComponent },
    {path: 'user-signin',component:LoginComponent},
    {path:'student-dashboard',component:StudentDashboardComponent},
    { path: '**', component:BodyComponent },

];