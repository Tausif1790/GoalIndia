import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";
import { HttpClient } from '@angular/common/http';
import { Product } from './shared/models/product';
import { Pagination } from './shared/models/pagination';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  baseUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);          // Inject HttpClient for making API calls
  title : any = "GaolIndia";
  products: Product[] = [];

  // ngOnInit() is the best lifecycle hook to make the API call,
  // ensuring that the component is fully initialized before the HTTP request is made.
  ngOnInit(): void {
    // The HttpClient makes a GET request to the /products endpoint using the
    // this.http.get<Pagination<Product>>(this.baseUrl + 'products') method.
    // The Pagination<Product> is used to type the response, so TypeScript knows
    // what structure to expect (in this case, pagination containing an array of Product).
    this.http.get<Pagination<Product>>(this.baseUrl + 'products').subscribe({
      next: response => this.products = response.data,      // Handle the success response
      error: error => console.log(error),                   // Handle any errors
      complete: () => console.log('complete')               // Log completion of the request
    });
  }
}
