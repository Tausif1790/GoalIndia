import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart, CartItem } from '../../shared/models/cart';
import { Product } from '../../shared/models/product';
import { map } from 'rxjs';

// This service manages cart functionality, including adding/removing items, fetching the cart, and calculating totals.
@Injectable({
  providedIn: 'root'
})
export class CartService {
  baseUrl = environment.apiUrl;         // Base API URL from environment variables
  private http = inject(HttpClient);    // Injecting HttpClient for HTTP requests
  cart = signal<Cart | null>(null);     // Signal to track the current cart state

  // Computed signal to calculate total number of items in the cart
  itemCount = computed(() => {
    return this.cart()?.items.reduce((sum, item) => sum + item.quantity, 0);
  });

  // Computed signal to calculate cart totals (subtotal, shipping, discount, and total)
  totals = computed(() => {
    const cart = this.cart();
    if (!cart) return null;
    const subtotal = cart.items.reduce((sum, item) => sum + item.price * item.quantity, 0);
    const shipping = 0;         // Fixed shipping cost (can be dynamic)
    const discount = 0;         // Fixed discount (can be dynamic)
    return {
      subtotal,
      shipping,
      discount,
      total: subtotal + shipping - discount  // Calculate total price
    };
  });

  // Fetch the cart by ID and set it in the signal
  getCart(id: string) {
    return this.http.get<Cart>(this.baseUrl + 'cart?id=' + id).pipe(
      map(cart => {
        this.cart.set(cart);        // Update cart signal
        return cart;
      })
    );
  }

  // Save the cart to the server
  setCart(cart: Cart) {
    return this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe({
      next: cart => this.cart.set(cart)             // Update cart signal after saving
    });
  }

  // Add a product or item to the cart
  addItemToCart(item: CartItem | Product, quantity = 1) {
    const cart = this.cart() ?? this.createCart();  // Create a new cart if it doesn't exist
    if (this.isProduct(item)) {
      item = this.mapProductToCartItem(item);       // Convert Product to CartItem if necessary
    }
    cart.items = this.addOrUpdateItem(cart.items, item, quantity);  // Add or update item quantity
    this.setCart(cart);                             // Save the updated cart
  }

  // Remove an item from the cart or reduce its quantity
  removeItemFromCart(productId: number, quantity = 1) {
    const cart = this.cart();  // Get the current cart
    if (!cart) return;
    const index = cart.items.findIndex(x => x.productId === productId);  // Find the item in the cart
    if (index !== -1) {
      if (cart.items[index].quantity > quantity) {
        cart.items[index].quantity -= quantity;                          // Decrease the item quantity
      } else {
        cart.items.splice(index, 1);                                     // Remove item from the cart if quantity is 0
      }
      if (cart.items.length === 0) {
        this.deleteCart();                                              // Delete the cart if it's empty
      } else {
        this.setCart(cart);  // Save the updated cart
      }
    }
  }

  // Delete the cart from the server and clear local storage
  deleteCart() {
    this.http.delete(this.baseUrl  + 'cart?id=' + this.cart()?.id).subscribe({
      next: () => {
        localStorage.removeItem('cart_id');     // Remove cart ID from local storage
        this.cart.set(null);                    // Clear cart signal
      }
    });
  }

  // Helper method to add or update an item in the cart
  private addOrUpdateItem(items: CartItem[], item: CartItem, quantity: number): CartItem[] {
    const index = items.findIndex(x => x.productId === item.productId);
    if (index === -1) {                       // If item not found, add it
      item.quantity = quantity;
      items.push(item);
    } else {
      items[index].quantity += quantity;      // If item exists, update the quantity
    }
    return items;
  }

  // Helper method to map Product to CartItem
  private mapProductToCartItem(item: Product): CartItem {
    return {
      productId: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,
      pictureUrl: item.pictureUrl,
      brand: item.brand,
      type: item.type
    };
  }

  // Type guard to check if an item is a Product
  private isProduct(item: CartItem | Product): item is Product {
    return (item as Product).id !== undefined;
  }

  // Create a new cart and save the cart ID in local storage
  private createCart(): Cart {
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);  // Store cart ID in local storage
    return cart;
  }
}

