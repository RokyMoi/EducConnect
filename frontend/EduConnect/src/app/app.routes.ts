import { Routes } from '@angular/router';
import { BodyComponent } from './landingPage/body/body.component';
import { TutorSignupComponent } from './signup/tutor/tutor-signup/tutor-signup.component';

export const routes: Routes = [
  { path: '', component: BodyComponent },
  { path: 'tutor-signup', component: TutorSignupComponent },
  { path: '**', component: BodyComponent },
];
