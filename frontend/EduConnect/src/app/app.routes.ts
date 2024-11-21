import { Routes } from '@angular/router';
import { BodyComponent } from './landingPage/body/body.component';
import { TutorSignupComponent } from './signup/tutor/tutor-signup/tutor-signup.component';
import { RegisterStudentComponent } from './signup/Student/register-student/register-student.component';
<<<<<<< HEAD
import { LoginComponent } from './signup/login/login.component';
=======
>>>>>>> 745a0f1c2d045dcc86d662d3afdd053dfe664c4c



export const routes: Routes = [
    { path: '', component:BodyComponent },
    { path: 'student-register', component: RegisterStudentComponent },
<<<<<<< HEAD
    { path: 'tutor-signup', component: TutorSignupComponent },
    {path: 'user-signin',component:LoginComponent},
    { path: '**', component:BodyComponent },

=======
    { path: '**', component:BodyComponent },
>>>>>>> 745a0f1c2d045dcc86d662d3afdd053dfe664c4c
];