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

  //PersonPhoneNumber controller
  //Add phone number
  public static readonly addPhoneNumber =
    ApiLinks.baseUrl + 'person/phone-number';
  
  
  //PersonDetails controller
  //Add person details
  public static readonly addPersonDetails = ApiLinks.baseUrl + 'person/details';
}
