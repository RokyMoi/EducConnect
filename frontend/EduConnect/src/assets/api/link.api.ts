//File for storing API links

export default class ApiLinks {
  public static readonly baseUrl = 'http://127.0.0.1:5177/';
  public static readonly tutorRegister = ApiLinks.baseUrl + 'tutor/signup';
  public static readonly tutorEmailVerification =
    ApiLinks.baseUrl + 'tutor/verify';
  public static readonly tutorEmailVerificationCodeResend =
    ApiLinks.baseUrl + 'tutor/resend-verification-code';

  public static readonly getCurrentTutorRegistrationStatus =
    ApiLinks.baseUrl + 'tutor/signup/status';

  //Reference controller
  //Get all tutor registration status
  public static readonly getAllTutorRegistrationStatus =
    ApiLinks.baseUrl + 'reference/tutor-registration-status/all';
  //Get all countries
  public static readonly getAllCountries = ApiLinks.baseUrl + 'country/all';
  //Get all employment types
  public static readonly getAllEmploymentTypes =
    ApiLinks.baseUrl + 'reference/employment-type/all';
  //Get all industry classifications
  public static readonly getAllIndustryClassifications =
    ApiLinks.baseUrl + 'reference/industry-classification/all';
  //Get all work types
  public static readonly getAllWorkTypes =
    ApiLinks.baseUrl + 'reference/work-type/all';
  //Get all tutor teaching style types
  public static readonly getAllTutorTeachingStyleTypes =
    ApiLinks.baseUrl + 'reference/tutor-teaching-style-type/all';
  //Get all communication types
  public static readonly getAllCommunicationTypes =
    ApiLinks.baseUrl + 'reference/communication-type/all';
  //Get all engagement methods
  public static readonly getAllEngagementMethods =
    this.baseUrl + 'reference/engagement-method/all';
  //Get all learning categories and subcategories
  public static readonly getAllLearningCategoriesAndSubcategories =
    ApiLinks.baseUrl + 'reference/learning-category-subcategory/all';
  //Get all difficulty levels
  public static readonly getAllLearningDifficultyLevels =
    ApiLinks.baseUrl + 'reference/learning-difficulty-level/all';
  //Get all course types
  public static readonly getAllCourseTypes =
    ApiLinks.baseUrl + 'reference/course-type/all';
  public static readonly getAllLanguages =
    ApiLinks.baseUrl + 'reference/language/all';

  //PersonPhoneNumber controller
  //Add phone number
  public static readonly addPhoneNumber =
    ApiLinks.baseUrl + 'person/phone-number';

  //PersonDetails controller
  //Add person details
  public static readonly addPersonDetails = ApiLinks.baseUrl + 'person/details';

  //PersonEducationInformation controller
  //Add education information
  public static readonly addEducationInformation =
    ApiLinks.baseUrl + 'person/education';
  //Get all education information
  public static readonly getAllEducationInformation =
    ApiLinks.baseUrl + 'person/education/all';
  //Update education information by id from the body
  public static readonly updateEducationInformation =
    ApiLinks.baseUrl + 'person/education';
  //Delete education information by id
  public static readonly deleteEducationInformation =
    ApiLinks.baseUrl + 'person/education';

  //PersonCareerInformation controller
  //Get all career information by person id
  public static readonly getAllCareerInformation =
    ApiLinks.baseUrl + 'person/career/all';
  //Add career information
  public static readonly addCareerInformation =
    ApiLinks.baseUrl + 'person/career';
  public static readonly updateCareerInformation =
    ApiLinks.baseUrl + 'person/career';
  public static readonly deleteCareerInformation =
    ApiLinks.baseUrl + 'person/career';

  //PersonTimeAvailability controller
  //Add time availability
  public static readonly addTimeAvailability =
    ApiLinks.baseUrl + 'person/availability';
  //Get all time availability
  public static readonly getAllTimeAvailability =
    ApiLinks.baseUrl + 'person/availability/all';
  //Update time availability
  public static readonly updateTimeAvailability =
    ApiLinks.baseUrl + 'person/availability';
  //Delete time availability
  public static readonly deleteTimeAvailability =
    ApiLinks.baseUrl + 'person/availability';

  //TutorTeachingInformation controller
  //Add tutor teaching information
  public static readonly addTutorTeachingInformation =
    ApiLinks.baseUrl + 'tutor/teaching';

  //CourseCreate controller
  //Add course basic information
  public static readonly addCourseBasicInformation =
    ApiLinks.baseUrl + 'course/create/basic';
  //Add language support to the course
  public static readonly addLanguageSupportToCourse =
    ApiLinks.baseUrl + 'course/create/language';
  //Upload course main material
  public static readonly uploadCourseMainMaterial =
    ApiLinks.baseUrl + 'course/create/main-materials';

  //Course controller
  //Get Course basic information by course id
  public static readonly getCourseBasicInformation =
    ApiLinks.baseUrl + 'course/basic';
  ///Get all languages supported by the course
  public static readonly getCourseSupportedLanguages =
    ApiLinks.baseUrl + 'course/supported-language/';
  //Delete language support from the course
  public static readonly deleteLanguageSupportFromCourse =
    ApiLinks.baseUrl + 'course/supported-language/remove';
  //Get all main materials of the course
  public static readonly getCourseMainMaterials =
    ApiLinks.baseUrl + 'course/main-material/all';
  //Delete main material from the course
  public static readonly deleteCourseMainMaterial =
    ApiLinks.baseUrl + 'course/main-material';
  //Download main material from the course
  public static readonly downloadCourseMainMaterial =
    ApiLinks.baseUrl + 'course/main-material/download';
  //Update course type
  public static readonly updateCourseTypeByCourseId =
    ApiLinks.baseUrl + 'course/basic/course-type';

  //CourseLesson controller
  //Create course lesson
  public static readonly createCourseLesson =
    ApiLinks.baseUrl + 'course/lesson/create';
  //Create course lesson content
  public static readonly createCourseLessonContent =
    ApiLinks.baseUrl + 'course/lesson/content';
  public static readonly uploadCourseLessonSupplementaryMaterial =
    ApiLinks.baseUrl + 'course/lesson/supplementary-material';
  public static readonly getCourseLessonSupplementaryMaterial =
    ApiLinks.baseUrl + 'course/lesson/supplementary-material/all';
  public static readonly downloadCourseLessonSupplementaryMaterial =
    ApiLinks.baseUrl + 'course/lesson/supplementary-material/download';
  public static readonly deleteCourseLessonSupplementaryMaterial =
    ApiLinks.baseUrl + 'course/lesson/supplementary-material';
  public static readonly getAllCourseLessonsByCourse =
    ApiLinks.baseUrl + 'course/lesson/all';
  public static readonly deleteCourseLessonAndAssociatedData =
    ApiLinks.baseUrl + 'course/lesson/delete';
  public static readonly getCourseLessonWithContentAndSupplementaryMaterialsByCourseLesson =
    ApiLinks.baseUrl + 'course/lesson';
  public static readonly updateCourseLessonAndCourseLessonContent =
    ApiLinks.baseUrl + 'course/lesson';

  //PersonController base link
  public static readonly PersonControllerUrl = `${this.baseUrl}person`;

  //ReferenceController base link
  public static readonly ReferenceControllerUrl = `${this.baseUrl}reference`;

  //CourseTutorController base link
  public static readonly CourseTutorControllerUrl = `${this.baseUrl}tutor/course`;

  //CourseStudentController base link
  public static readonly CourseStudentControllerUrl = `${this.baseUrl}student/course`;

  //CollaborationDocumentController base link
  public static readonly CollaborationDocumentControllerUrl = `${this.baseUrl}collaboration-document`;
}
