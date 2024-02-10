import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {AuthenticationService} from "../services/authentication.service";

// Define a guard function to check if the user is an overseer or higher
export const overseerGuard: CanActivateFn = (): boolean => {

  // Inject AuthenticationService to access user role information
  const isOverseer: boolean = inject(AuthenticationService).isUserOverseerOrHigher();

  // If the user is an overseer or higher, allow access
  if (isOverseer) {
    return true;
  }

  // If the user is not an overseer or higher, redirect to the login page
  const router: Router = inject(Router);
  router.navigate(['/login']).then();

  // Return false to deny access
  return false;
};
