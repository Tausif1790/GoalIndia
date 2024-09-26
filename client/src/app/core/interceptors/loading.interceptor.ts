import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { delay, finalize } from 'rxjs';
import { BusyService } from '../services/busy.service';

// This interceptor manages a loading indicator, showing it when an HTTP request
// is in progress and hiding it when the request completes.
export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const busyService = inject(BusyService);    // Injecting BusyService to control loading state

  busyService.busy();                         // Start loading indicator

  return next(req).pipe(
    delay(500),                               // Add a delay to simulate a loading state
    finalize(() => busyService.idle())        // Stop loading indicator after request completes
  );
};

