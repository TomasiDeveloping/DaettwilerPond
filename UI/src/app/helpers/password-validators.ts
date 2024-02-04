import {AbstractControl, ValidationErrors, ValidatorFn} from "@angular/forms";

// Custom class containing static methods for password validation
export class PasswordValidators {

  // Validator function to check if password and confirm password match
  static passwordMatch(password: string, confirmPassword: string): ValidatorFn {
    return (formGroup: AbstractControl): { [key: string]: any } | null => {
      const passwordControl: AbstractControl<any, any> | null = formGroup.get(password);
      const confirmPasswordControl: AbstractControl<any, any> | null = formGroup.get(confirmPassword);

      // Check if controls are present in the formGroup
      if (!passwordControl || !confirmPasswordControl) {
        return null;
      }

      // Check if there are existing errors and if passwordMismatch error is not present
      if (
        confirmPasswordControl.errors &&
        !confirmPasswordControl.errors["passwordMismatch"]
      ) {
        return null;
      }

      // Compare password and confirm password values
      if (passwordControl.value !== confirmPasswordControl.value) {
        // Set passwordMismatch error and return corresponding error object
        confirmPasswordControl.setErrors({passwordMismatch: true});
        return {passwordMismatch: true};
      } else {
        // Reset errors if passwords match
        confirmPasswordControl.setErrors(null);
        return null;
      }
    };
  }

  // Validator function to check if the input value matches a given regex pattern
  static patternValidator(regex: RegExp, error: ValidationErrors): ValidatorFn {
    return (control: AbstractControl): { [p: string]: any } | null => {
      // Check if control value is empty
      if (!control.value) {
        // If control is empty, return no error
        return null;
      }

      // Test the value of the control against the provided regex pattern
      const valid = regex.test(control.value);

      // If valid, return no error; otherwise, return the error passed in the second parameter
      return valid ? null : error;
    };
  }
}
