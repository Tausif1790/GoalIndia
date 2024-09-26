import { Injectable } from '@angular/core';

// The BusyService controls the loading state. It increments a request
// count when a request starts and decrements it when the request completes.
// If there are no active requests, the loading indicator is hidden.
@Injectable({
  providedIn: 'root'        // Makes this service available across the entire app
})
export class BusyService {
  loading = false;  // Flag to track whether loading is active
  busyRequestCount = 0;    // Count of active HTTP requests

  busy() {
    this.busyRequestCount++;    // Increment the count on request start
    this.loading = true;        // Set loading to true to show indicator
  }

  idle() {
    this.busyRequestCount--;    // Decrement the count on request completion
    if (this.busyRequestCount <= 0) {  // If no active requests, hide the loading indicator
      this.busyRequestCount = 0;
      this.loading = false;
    }
  }
}
