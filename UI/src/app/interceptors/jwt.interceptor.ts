import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable} from 'rxjs';

// Injectable class implementing HttpInterceptor for JWT token handling
@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  // Intercept method to add JWT token to outgoing HTTP requests
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // Retrieve JWT token from local storage
    const token = localStorage.getItem('DaettwilerPondToken');

    // If a token is available, clone the request and set Authorization header
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    // Continue with the modified request
    return next.handle(request);
  }
}
