import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {AuthenticationService} from "../services/authentication.service";

// Custom guard to check if the user is authenticated
export const authGuard: CanActivateFn = (): boolean => {
  // Get the instance of AuthenticationService through dependency injection
  // Check if the user is authenticated
  const isLoggedIn: boolean = inject(AuthenticationService).isUserAuthenticated();

  // If the user is authenticated, allow access
  if (isLoggedIn) {
    return true;
  }

  // If not authenticated, redirect to the login page
  const router: Router = inject(Router);
  router.navigate(['/login']).then();

  // Return false to deny access
  return false;
};
