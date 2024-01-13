import {AbstractControl, AsyncValidatorFn} from "@angular/forms";
import {map, of, switchMap, timer} from "rxjs";
import {FishCatchService} from "../services/fish-catch.service";

export function catchDayExistsValidator(
  licenceId: string,
  catchService: FishCatchService,
): AsyncValidatorFn {
  return (control: AbstractControl) => {
    return timer(500).pipe(
      switchMap(() => {
        if (!control.value) {
          return of(null);
        }
        return catchService.checkCatchDateExists(licenceId, control.value).pipe(
          map((result: boolean) => {
            return result ? {catchDayExists: true} : null;
          })
        );
      })
    );
  };
}
