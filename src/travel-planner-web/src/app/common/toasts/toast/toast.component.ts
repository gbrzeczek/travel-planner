import { Component, OnInit } from '@angular/core';
import { ToastService } from '../toast.service';
import { Toast } from '../models/toast';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [NgClass],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.scss',
})
export class ToastComponent implements OnInit {
  public toast?: Toast;
  constructor(private readonly _toastService: ToastService) {}

  ngOnInit(): void {
    this._toastService.toast$.subscribe((toast) => {
      this.toast = toast;
      setTimeout(() => {
        this.toast = undefined;
      }, 5000);
    });
  }

  public getToastClassess(): string {
    if (!this.toast) {
      return '';
    }

    switch (this.toast.type) {
      case 'success':
        return 'bg-green-100 text-green-500 dark:bg-green-800 dark:text-green-200';
      case 'error':
        return 'bg-red-100 text-red-500 dark:bg-red-800 dark:text-red-200';
      case 'warning':
        return 'bg-orange-100 text-orange-500 dark:bg-orange-700 dark:text-orange-200';
      default:
        return '';
    }
  }
}
