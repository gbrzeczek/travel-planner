import { Component } from '@angular/core';
import { LoginService } from '../login.service';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

interface LoginForm {
  email: FormControl<string>;
  password: FormControl<string>;
}

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss',
})
export class LoginPageComponent {
  public readonly loginForm = new FormGroup<LoginForm>({
    email: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.email, Validators.required],
    }),
    password: new FormControl<string>('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
  });

  constructor(private readonly _loginService: LoginService) {}

  public submit(): void {
    this._loginService.login(this.loginForm.getRawValue()).subscribe({
      next: () => {
        // TODO: handle success
        console.log('success');
      },
      error: () => {
        // TODO: handle error
        console.log('error');
      },
    });
  }
}
