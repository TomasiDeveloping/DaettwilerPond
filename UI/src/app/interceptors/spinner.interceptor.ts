import {inject, Injectable} from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import {finalize, Observable} from 'rxjs';
import {SpinnerService} from "../services/spinner.service";

// Injectable class implementing HttpInterceptor for spinner handling during HTTP requests
@Injectable()
export class SpinnerInterceptor implements HttpInterceptor {

  // Inject the SpinnerService to manage the spinner state
  private readonly _spinnerService: SpinnerService = inject(SpinnerService);

  // Intercept method to handle spinner state during HTTP requests
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // Exclude specific URL from spinner handling (CheckCatchDateExists in this case)
    if (request.url.includes('CheckCatchDateExists')) {
      return next.handle(request);
    }

    // Activate the spinner before making the request
    this._spinnerService.busy();

    // Continue with the request and deactivate the spinner after completion
    return next.handle(request).pipe(
      finalize(() => {
        this._spinnerService.idle();
      })
    );
  }
}
