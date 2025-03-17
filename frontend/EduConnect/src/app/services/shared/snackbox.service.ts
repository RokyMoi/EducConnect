import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export type SnackBoxType = 'info' | 'success' | 'error' | 'warning';

@Injectable({
  providedIn: 'root',
})
export class SnackboxService {
  private snackboxSubject = new BehaviorSubject<{
    message: string;
    type: SnackBoxType;
  } | null>(null);

  snackbox$ = this.snackboxSubject.asObservable();

  constructor() {}

  showSnackbox(
    message: string,
    type: SnackBoxType = 'info',
    duration: number = 3000
  ) {
    this.snackboxSubject.next({ message, type });
    setTimeout(() => {
      this.snackboxSubject.next(null);
    }, duration);
  }
}
