import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {AuthenticationService} from "../services/authentication.service";

export const authGuard: CanActivateFn = (): boolean => {
  const isLoggedIn = inject(AuthenticationService).isUserAuthenticated();
  if (isLoggedIn) {
    return true;
  }
  const router = inject(Router);
  router.navigate(['/login']).then();
  return false;
};
