import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class MatchPasswordValidator {
  public static isMatching(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const password = control.get('password')?.value;
      const repeatPassword = control.get('repeatPassword')?.value;
      if (password && repeatPassword && password !== repeatPassword) {
        return { matchingPasswordsError: true };
      }
      return null;
    };
  }
}
