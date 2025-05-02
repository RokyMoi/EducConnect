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

import { DynamicFormComponent } from './modules/SetingsStudent/dynamic-form/dynamic-form.component';
import { SendMessageComponent } from './Messenger/send-message/send-message.component';
import { ListOfUsersComponent } from './Messenger/list-of-users/list-of-users.component';
import { CartItemsComponent } from './ShoppingCart/cart-items/cart-items.component';
import { WishlistComponent } from './modules/ShoppingCart/wishlist/wishlist.component';
import { CourseSupportedLanguagesComponent } from './modules/tutor/course/course-supported-languages/course-supported-languages.component';
import { CourseMainMaterialsComponent } from './modules/tutor/course/course-main-materials/course-main-materials.component';
import { CourseLessonsComponent } from './modules/tutor/course/self-paced-course/course-lessons/course-lessons.component';

import { CreateCourse } from '../../../../.history/frontend/EduConnect/src/app/_models/course/create-course/create-course.create-course.course.model_20250107000517';
import { CreateCourseDetailsComponent } from './modules/tutor/create-course/create-course-details/create-course-details.component';
import { AuthenticationGuardService } from './services/shared/authentication-guard.service';
import { ForbiddenAccessComponent } from './modules/shared/forbidden-access/forbidden-access/forbidden-access.component';
import { AdminDashboardComponent } from './modules/admin/admin-dashboard/admin-dashboard.component';
import { CourseTutorDashboardComponent } from './modules/tutor/course/course-tutor-dashboard/course-tutor-dashboard.component';
import { CourseTutorManagementComponent } from './modules/tutor/course/course-tutor-management/course-tutor-management.component';
import { CourseDetailsComponent } from './modules/tutor/course/course-details/course-details.component';
import { CourseThumbnailComponent } from './modules/tutor/course/course-thumbnail/course-thumbnail.component';
import { CourseTutorTeachingResourcesComponent } from './modules/tutor/course/course-teaching-resources/course-tutor-teaching-resources.component';
import { CourseTutorTeachingResourcesDetailsComponent } from './modules/tutor/course/course-tutor-teaching-resources-details/course-tutor-teaching-resources-details.component';
import { CourseTutorLessonDetailsComponent } from './modules/tutor/course/course-tutor-lesson-details/course-tutor-lesson-details.component';
import { CourseTutorLessonsComponent } from './modules/tutor/course/course-tutor-lessons/course-tutor-lessons.component';
import { CourseTutorCourseLessonResourcesComponent } from './modules/tutor/course/course-tutor-course-lesson-resources/course-tutor-course-lesson-resources.component';
import { CourseTutorLessonResourceDetailsComponent } from './modules/tutor/course/course-tutor-lesson-resource-details/course-tutor-lesson-resource-details.component';
import { StudentCourseSearchComponent } from './modules/student/course/student-course-search/student-course-search.component';
import { StudentCourseDetailsComponent } from './modules/student/course/student-course-details/student-course-details.component';
import { CourseTutorPromotionImagesComponent } from './modules/tutor/course/course-tutor-promotion-images/course-tutor-promotion-images.component';
import { CoursePromotionImagesDetailsComponent } from './modules/tutor/course/course-promotion-images-details/course-promotion-images-details.component';
import { CourseTutorAnalyticsDashboardComponent } from './modules/tutor/course/course-tutor-analytics-dashboard/course-tutor-analytics-dashboard.component';
import { CourseTutorTagsComponent } from './modules/tutor/course/course-tutor-tags/course-tutor-tags.component';
import { CollaborationDocumentDashboardComponent } from './modules/shared/collaboration-document/collaboration-document-dashboard/collaboration-document-dashboard.component';
import { CollaborationDocumentLiveEditorComponent } from './modules/shared/collaboration-document/collaboration-document-live-editor/collaboration-document-live-editor.component';
import { CollaborationDocumentInviteUsersComponent } from './modules/shared/collaboration-document/collaboration-document-invite-users/collaboration-document-invite-users.component';
import { TutorFileExplorerComponent } from './modules/tutor/tutor-file-explorer/tutor-file-explorer/tutor-file-explorer.component';

export const routes: Routes = [
  { path: 'index', component: BodyComponent },
  { path: '', component: BodyComponent },
  { path: 'settings', component: MainSettingsComponent },
  { path: 'student-register', component: RegisterStudentComponent },
  { path: 'tutor-signup', component: TutorSignupComponent },
  { path: 'login', component: LoginComponent },
  { path: 'student-profile', component: StudentProfileComponent },
  { path: 'learning-student', component: LearningComponent },

  {
    path: 'student/course/search',
    component: StudentCourseSearchComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'student' },
  },
  {
    path: 'student/dashboard',
    component: StudentDashboardComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'student' },
  },
  {
    path: 'student/course/details/:courseId',
    component: StudentCourseDetailsComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'student' },
  },

  { path: 'photouploadcomponent', component: PhotoComponent },
  { path: 'student-message-preview', component: MessagesComponent },
  { path: 'direct-message', component: DirectMessagingsComponent },
  {
    path: 'studentMessageThread/:id',
    component: StudentThreadMessageComponent,
  },
  { path: 'viewOfAllCourses', component: CourseLandingPageComponent },
  { path: 'student-profile', component: StudentProfileComponent },
  { path: 'learning-student', component: LearningComponent },
  { path: 'student-dashboard', component: StudentDashboardComponent },
  { path: 'tutor-dashboard', component: TutorDashboardComponent },
  { path: 'dynamicForm', component: DynamicFormComponent },
  { path: 'student-message-preview', component: MessagesComponent },
  { path: 'direct-message', component: DirectMessagingsComponent },
  {
    path: 'studentMessageThread/:id',
    component: StudentThreadMessageComponent,
  },
  { path: 'viewOfAllCourses', component: CourseLandingPageComponent },
  { path: 'send-message', component: SendMessageComponent },
  { path: 'Shopping-Cart', component: CartItemsComponent },
  { path: 'course-wishlist', component: WishlistComponent },
  { path: 'ListOfUsers', component: ListOfUsersComponent },

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
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course',
    component: CourseTutorDashboardComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/:courseId',
    component: CourseTutorManagementComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/details/:courseId',
    component: CourseDetailsComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/thumbnail/:courseId',
    component: CourseThumbnailComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/teaching-resources/:courseId',
    component: CourseTutorTeachingResourcesComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/teaching-resources/new/:courseId',
    component: CourseTutorTeachingResourcesDetailsComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/teaching-resources/details/:courseId/:resourceId',
    component: CourseTutorTeachingResourcesDetailsComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/lessons/:courseId',
    component: CourseTutorLessonsComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/lessons/new/:courseId',
    component: CourseTutorLessonDetailsComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/lessons/details/:courseId/:lessonId',
    component: CourseTutorLessonDetailsComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/lessons/resources/:courseId/:lessonId',
    component: CourseTutorCourseLessonResourcesComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/lessons/resources/new/:courseId/:lessonId',
    component: CourseTutorLessonResourceDetailsComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/lessons/resources/details/:courseId/:lessonId/:resourceId',
    component: CourseTutorLessonResourceDetailsComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/promotion/:courseId',
    component: CourseTutorPromotionImagesComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/promotion/new/:courseId',
    component: CoursePromotionImagesDetailsComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/promotion/details/:courseId/:imageId',
    component: CoursePromotionImagesDetailsComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/analytics/:courseId',
    component: CourseTutorAnalyticsDashboardComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/course/tags/:courseId',
    component: CourseTutorTagsComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'tutor/files',
    component: TutorFileExplorerComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },

  //Admin routes
  {
    path: 'admin/dashboard',
    component: AdminDashboardComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'admin' },
  },

  //Shared routes
  {
    path: 'tutor/collaboration',
    component: CollaborationDocumentDashboardComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'student/collaboration',
    component: CollaborationDocumentDashboardComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'student' },
  },
  {
    path: 'admin/collaboration',
    component: CollaborationDocumentDashboardComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'admin' },
  },
  {
    path: 'tutor/collaboration/document/:documentId',
    component: CollaborationDocumentLiveEditorComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'student/collaboration/document/:documentId',
    component: CollaborationDocumentLiveEditorComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'student' },
  },
  {
    path: 'admin/collaboration/document/:documentId',
    component: CollaborationDocumentLiveEditorComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'admin' },
  },
  {
    path: 'tutor/collaboration/invite/:documentId',
    component: CollaborationDocumentInviteUsersComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'tutor' },
  },
  {
    path: 'student/collaboration/invite/:documentId',
    component: CollaborationDocumentInviteUsersComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'student' },
  },
  {
    path: 'admin/collaboration/invite/:documentId',
    component: CollaborationDocumentInviteUsersComponent,
    canActivate: [AuthenticationGuardService],
    data: { requiredRole: 'admin' },
  },
  {
    path: 'forbidden',
    component: ForbiddenAccessComponent,
  },
  { path: '**', component: BodyComponent },
];
