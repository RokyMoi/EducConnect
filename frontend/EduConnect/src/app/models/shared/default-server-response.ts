export interface DefaultServerResponse<T = any> {
  message: string;
  data: T;
  timestamp: string;
}
