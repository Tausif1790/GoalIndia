import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';

// This component is used to test various error scenarios (e.g., 404, 400, 500)
// by making HTTP requests to different endpoints.
// It helps in testing the error handling mechanisms in the application.
@Component({
  selector: 'app-test-error',
  standalone: true,
  imports: [
    MatButton            // Angular Material Button for triggering error tests
  ],
  templateUrl: './test-error.component.html',
  styleUrl: './test-error.component.scss'
})
export class TestErrorComponent {
  baseUrl = 'https://localhost:5001/api/';  // Base URL for API
  private http = inject(HttpClient);        // Inject HttpClient for making HTTP requests
  validationErrors?: string[];              // Store validation errors from server

  get404Error() {
    this.http.get(this.baseUrl + 'buggy/notfound').subscribe({
      next: response => console.log(response),    // Log successful response
      error: error => console.log(error)          // Log 404 error
    });
  }

  get400Error() {
    this.http.get(this.baseUrl + 'buggy/badrequest').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)          // Log 400 error
    });
  }

  get401Error() {
    this.http.get(this.baseUrl + 'buggy/unauthorized').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)          // Log 401 error
    });
  }

  get500Error() {
    this.http.get(this.baseUrl + 'buggy/internalerror').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)          // Log 500 error
    });
  }

  get400ValidationError() {
    this.http.post(this.baseUrl + 'buggy/validationerror', {}).subscribe({
      next: response => console.log(response),
      error: error => this.validationErrors = error     // Store validation errors from server
    });
  }
}
