import { Routes } from '@angular/router';
import { BodyComponent } from './landingPage/body/body.component';
import { TutorSignupComponent } from './signup/tutor/tutor-signup/tutor-signup.component';
import { RegisterStudentComponent } from './signup/Student/register-student/register-student.component';
import { LoginComponent } from './signup/login/login.component';



export const routes: Routes = [
    { path: '', component:BodyComponent },
    { path: 'student-register', component: RegisterStudentComponent },
    { path: 'tutor-signup', component: TutorSignupComponent },
    {path: 'user-signin',component:LoginComponent},
    { path: '**', component:BodyComponent },

];