import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { StrongPasswordValidator } from '../utils/validators/strong-password.validator';
import { MatchPasswordValidator } from '../utils/validators/match-password.validator';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-register-page',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatInputModule,
    ReactiveFormsModule,
    MatButtonModule,
  ],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.scss',
})
export class RegisterPageComponent {
  public registerForm = new FormGroup(
    {
      username: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [
        Validators.required,
        StrongPasswordValidator.isValid(),
      ]),
      repeatPassword: new FormControl('', [Validators.required]),
    },
    {
      validators: MatchPasswordValidator.isMatching(),
    }
  );

  public submit(): void {}
}
