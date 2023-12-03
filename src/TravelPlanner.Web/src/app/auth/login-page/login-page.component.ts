import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatDividerModule,
    RouterLink,
  ],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss',
})
export class LoginPageComponent {
  constructor(
    private readonly _authService: AuthService,
    private readonly snackbar: MatSnackBar,
    private readonly router: Router
  ) {}

  public loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
  });

  public submit(): void {
    if (this.loginForm.valid) {
      const email = this.loginForm.get('email')?.value;
      const password = this.loginForm.get('password')?.value;

      if (!email || !password) {
        this.snackbar.open('Please enter email and password', 'OK', {
          duration: 5000,
        });
        return;
      }

      this._authService.login(email!, password!).subscribe({
        next: () => {
          this.router.navigate(['/']);
        },
        error: () => {
          this.snackbar.open('Failed to log in. Check your credentials', 'OK', {
            duration: 5000,
          });
        },
      });
    }
  }
}
