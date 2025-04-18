export default interface ErrorHttpResponseData {
  message: string;
  success?: string;
  data?: any;
  error?: any;
  statusCode?: number;
  statusText?: string;
}
