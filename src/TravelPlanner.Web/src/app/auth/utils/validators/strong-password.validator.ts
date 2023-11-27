import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class StrongPasswordValidator {
  public static isValid(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) {
        return null;
      }

      const hasUpperCase = /[A-Z]+/.test(value);
      const hasLowerCase = /[a-z]+/.test(value);
      const hasNumeric = /[0-9]+/.test(value);
      const hasSpecial = /[$@$!%*?^&\(\)\-\_=+\[\{\]\}\\\|;:'",<.>\/\ #]/.test(
        value
      );
      const hasCorrectLength = /.{8,}/.test(value);

      const isPasswordValid =
        hasUpperCase &&
        hasLowerCase &&
        hasNumeric &&
        hasSpecial &&
        hasCorrectLength;
      return !isPasswordValid ? { passwordStrength: true } : null;
    };
  }
}
