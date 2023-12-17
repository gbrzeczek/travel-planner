import { Component } from '@angular/core';
import { ToastService } from '../common/toasts/toast.service';

@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.scss',
})
export class LandingPageComponent {
  constructor(private readonly _toastService: ToastService) {}

  public showToast(): void {
    this._toastService.error('Hello World!');
  }
}
