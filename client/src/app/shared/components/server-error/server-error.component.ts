import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { MatCard } from '@angular/material/card';
import { Router } from '@angular/router';

// This component is used to display server error details when the user is redirected to the /server-error route.
@Component({
  selector: 'app-server-error',
  standalone: true,
  imports: [
    MatCard       // Angular Material Card for displaying error info
  ],
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.scss'
})
export class ServerErrorComponent {
  error?: any;  // Error object that stores error details

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();  // Get navigation state
    this.error = navigation?.extras.state?.['error'];       // Retrieve error passed via state
  }
}

