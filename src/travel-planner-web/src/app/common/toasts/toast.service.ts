import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Toast } from './models/toast';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private toastSubject$ = new Subject<Toast>();
  public readonly toast$ = this.toastSubject$.asObservable();
  constructor() {}

  public success(message: string): void {
    const toast: Toast = {
      message,
      type: 'success',
    };
    this.toastSubject$.next(toast);
  }

  public error(message: string): void {
    const toast: Toast = {
      message,
      type: 'error',
    };
    this.toastSubject$.next(toast);
  }

  public warning(message: string): void {
    const toast: Toast = {
      message,
      type: 'warning',
    };
    this.toastSubject$.next(toast);
  }
}
