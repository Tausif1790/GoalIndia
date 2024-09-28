import { inject, Injectable } from '@angular/core';
import { CartService } from './cart.service';
import { of } from 'rxjs';

// This service initializes the cart by checking local storage for an existing cart ID and fetching the cart.
@Injectable({
  providedIn: 'root'
})
export class InitService {
  private cartService = inject(CartService);  // Injecting CartService

  // Initialize the cart by checking for a cart ID in local storage
  init() {
    const cartId = localStorage.getItem('cart_id');  // Get cart ID from local storage
    const cart$ = cartId ? this.cartService.getCart(cartId) : of(null);     // Fetch cart if ID exists
    return cart$;         // Return observable of cart
  }
}
