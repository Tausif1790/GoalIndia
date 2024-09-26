import { HttpErrorResponse, HttpEvent, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service';

// This interceptor handles various HTTP errors and responds accordingly
// by showing error messages or redirecting the user to specific pages.
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);                // Injecting Angular Router service for navigation
  const snackbar = inject(SnackbarService);     // Injecting SnackbarService for displaying error messages
  
  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {    // Catch HTTP errors
      if (err.status === 400) {  // Handle 400 Bad Request
        if (err.error.errors) {
          const modelStateErrors = [];
          for (const key in err.error.errors) {  // Collect validation errors from the backend
            if (err.error.errors[key]) {
              modelStateErrors.push(err.error.errors[key]);
            }
          }
          throw modelStateErrors.flat();                  // Throw validation errors back to the component
        } else {
          snackbar.error(err.error.title || err.error);   // Display error message via Snackbar
        }
      }
      if (err.status === 401) {               // Handle 401 Unauthorized
        snackbar.error(err.error.title || err.error);
      }
      if (err.status === 404) {               // Handle 404 Not Found
        router.navigateByUrl('/not-found');   // Redirect to 'not-found' page
      }
      if (err.status === 500) {               // Handle 500 Internal Server Error
        const navigationExtras: NavigationExtras = {state: {error: err.error}};
        router.navigateByUrl('/server-error', navigationExtras);  // Redirect to 'server-error' page with error details
      }
      return throwError(() => err);           // Return the error to be handled by the component
    })
  );
};
