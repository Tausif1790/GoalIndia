import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../../shared/models/product';
import { CurrencyPipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatDivider } from '@angular/material/divider';
import { CartService } from '../../../core/services/cart.service';
import { FormsModule } from '@angular/forms';

// This component handles the display of product details and allows users to add or update items in the cart.
@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [
    CurrencyPipe,
    MatButton,
    MatIcon,
    MatFormField,
    MatInput,
    MatLabel,
    MatDivider,
    FormsModule
  ],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit {
  private shopService = inject(ShopService);  // Injecting ShopService to fetch product data
  private activatedRoute = inject(ActivatedRoute);  // Injecting ActivatedRoute to access route parameters
  private cartService = inject(CartService);  // Injecting CartService to manage cart actions
  product?: Product;  // Product object to store product details
  quantityInCart = 0;  // Number of items in the cart for the current product
  quantity = 1;  // Selected quantity to add to cart

  ngOnInit(): void {
    this.loadProduct();  // Load product details on component initialization
  }

  // Load product details based on the ID from the route
  loadProduct() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');   // Get product ID from route
    if (!id) return;
    this.shopService.getProduct(+id).subscribe({                  // Fetch product by ID (+ casts string to number)
      next: product => {
        this.product = product;                                   // Set the fetched product
        this.updateQuantityInCart();                              // Update the quantity of the product in the cart
      },
      error: error => console.log(error)                          // Log errors
    });
  }

  // Update the cart with the selected quantity
  updateCart() {
    if (!this.product) return;
    if (this.quantity > this.quantityInCart) {
      const itemsToAdd = this.quantity - this.quantityInCart;
      this.quantityInCart += itemsToAdd;                          // Update the quantity in the cart
      this.cartService.addItemToCart(this.product, itemsToAdd);   // Add items to cart
    } else {
      const itemsToRemove = this.quantityInCart - this.quantity;
      this.quantityInCart -= itemsToRemove;                       // Decrease the quantity in the cart
      this.cartService.removeItemFromCart(this.product.id, itemsToRemove);      // Remove items from cart
    }
  }

  // Update the quantity of the product in the cart
  updateQuantityInCart() {
    this.quantityInCart = this.cartService.cart()?.items
      .find(x => x.productId === this.product?.id)?.quantity || 0;  // Find the quantity of the product in the cart
    this.quantity = this.quantityInCart || 1;                       // Set the selected quantity
  }

  // Get button text based on whether the item is already in the cart
  getButtonText() {
    return this.quantityInCart > 0 ? 'Update cart' : 'Add to cart';  // Display 'Update cart' or 'Add to cart'
  }
}