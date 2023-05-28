import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {AuthenticationService} from "../services/authentication.service";

export const adminGuard: CanActivateFn = () => {
  const isAdmin: boolean = inject(AuthenticationService).isUserAdministrator();
  if (isAdmin) {
    return true;
  }
  const router: Router = inject(Router);
  router.navigate(['/login']).then();
  return false;
};
