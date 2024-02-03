import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {AuthenticationService} from "../services/authentication.service";

// Custom guard to check if the user is an administrator
export const adminGuard: CanActivateFn = (): boolean => {
  // Get the instance of AuthenticationService through dependency injection
  // Check if the user is an administrator
  const isAdmin: boolean = inject(AuthenticationService).isUserAdministrator();

  // If the user is an administrator, allow access
  if (isAdmin) {
    return true;
  }

  // If not an administrator, redirect to the login page
  const router: Router = inject(Router);
  router.navigate(['/login']).then();

  // Return false to deny access
  return false;
};
