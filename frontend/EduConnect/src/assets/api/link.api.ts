//File for storing API links

export default class ApiLinks {
  public static readonly baseUrl = 'http://127.0.0.1:5177/';
  public static readonly tutorRegister = ApiLinks.baseUrl + 'tutor/signup';
  public static readonly tutorEmailVerification =
    ApiLinks.baseUrl + 'tutor/verify';
  public static readonly tutorEmailVerificationCodeResend =
    ApiLinks.baseUrl + 'tutor/resend-verification-code';
}
