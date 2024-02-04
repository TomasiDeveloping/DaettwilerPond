import {AbstractControl, AsyncValidatorFn} from "@angular/forms";
import {map, of, switchMap, timer} from "rxjs";
import {FishCatchService} from "../services/fish-catch.service";

/**
 * Asynchronous validator function to check if a catch day exists.
 *
 * @param licenceId - The ID of the fishing license associated with the catch.
 * @param catchService - The FishCatchService for making asynchronous validation requests.
 * @returns An asynchronous validator function for checking if a catch day exists.
 */
export function catchDayExistsValidator(
  licenceId: string,
  catchService: FishCatchService,
): AsyncValidatorFn {
  return (control: AbstractControl) => {
    // Using timer and switchMap to handle asynchronous validation with a delay
    return timer(500).pipe(
      switchMap(() => {
        // If the control value is empty, consider it as valid
        if (!control.value) {
          return of(null);
        }

        // Making an asynchronous request to the catch service to check if the catch day exists
        return catchService.checkCatchDateExists(licenceId, control.value).pipe(
          map((result: boolean) => {
            // Returning validation result based on the response from the catch service
            return result ? {catchDayExists: true} : null;
          })
        );
      })
    );
  };
}
