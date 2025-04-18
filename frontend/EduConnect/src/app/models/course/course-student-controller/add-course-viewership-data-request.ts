import { UserCourseSourceType } from '../../../../enums/user-course-source-type.enum';

export interface AddCourseViewershipDataRequest {
  courseId: string;
  userCameFrom: UserCourseSourceType;
  clickedOn: string;
}
