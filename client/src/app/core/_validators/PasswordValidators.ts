import { AbstractControl, Validator, ValidatorFn } from '@angular/forms';

export class PasswordValidators {
  static MatchPassword(control: AbstractControl) {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (
      password &&
      confirmPassword &&
      password.value !== confirmPassword.value
    ) {
      confirmPassword.setErrors({ passwordsMisMatch: true });
    }
    return null;
  }
}
