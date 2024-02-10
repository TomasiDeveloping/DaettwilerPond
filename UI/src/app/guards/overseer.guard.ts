import { CanActivateFn } from '@angular/router';

export const overseerGuard: CanActivateFn = (route, state) => {
  return true;
};
