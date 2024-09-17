import { Component, inject } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../shared/models/product';
import { MatCard } from '@angular/material/card';
import { ProductItemComponent } from "./product-item/product-item.component";
import { MatDialog } from '@angular/material/dialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { ShopParams } from '../../shared/models/shopParams';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../shared/models/pagination';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    MatCard,
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator,
    FormsModule
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent {
  private shopService = inject(ShopService);      // Injecting ShopService to fetch product data and filter options
  private dialogService = inject(MatDialog);      // Injecting MatDialog to manage dialog windows
  products?: Pagination<Product>;                 // Variables to hold product data and pagination information
  sortOptions = [                                 // Sorting options for the product list
    {name: 'Alphabetical', value: 'name'},  
    {name: 'Price: Low-High', value: 'priceAsc'},
    {name: 'Price: High-Low', value: 'priceDesc'},
  ]
  shopParams = new ShopParams();                  // Object to store the current parameters for filtering and pagination
  pageSizeOptions = [5,10,15,20]                  // Available page sizes for pagination

  // Initialization logic when the component is created
  ngOnInit() {
    this.initialiseShop();
  }
  
  // Method to fetch initial shop data (brands, types, and products)
  initialiseShop() {
    this.shopService.getTypes();
    this.shopService.getBrands();
    this.getProducts();                                  // Fetches products based on the current shopParams
  }

  // Method to fetch products based on shopParams
  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: response => this.products = response,        // Success handler
      error: error => console.error(error)               // Error handler
    });
  }

  // Event handler for search input change
  onSearchChange() {
    this.shopParams.pageNumber = 1;                      // Reset page number to 1 after search
    this.getProducts();                                  // Fetch products based on updated search
  }

  // Event handler for pagination (page change)
  handlePageEvent(event: PageEvent) {
    this.shopParams.pageNumber = event.pageIndex + 1;    // Update page number
    this.shopParams.pageSize = event.pageSize;           // Update page size
    this.getProducts();                                  // Fetch products based on new pagination values
  }

  // Event handler for sorting options change
  onSortChange(event: MatSelectionListChange) {
    const selectedOption = event.options[0];             // Get selected sorting option
    if (selectedOption) {
      this.shopParams.sort = selectedOption.value;       // Update sort parameter
      this.shopParams.pageNumber = 1;                    // Reset page number
      this.getProducts();                                // Fetch products with new sorting
    }
  }

  // Method to open the filters dialog and update the filters after it's closed
  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',                                 // Minimum dialog width
      data: {                                            // Passing current selected filters to the dialog
        selectedBrands: this.shopParams.brands,
        selectedTypes: this.shopParams.types
      }
    });
    dialogRef.afterClosed().subscribe({                       // Subscribe to the dialog close event
      next: result => {                                       // After the dialog closes
        if (result) {                                         // Check if there is a result
          this.shopParams.brands = result.selectedBrands;     // Update selected brands
          this.shopParams.types = result.selectedTypes;       // Update selected types
          this.shopParams.pageNumber = 1;                     // Reset page number
          this.getProducts();                                 // Fetch products based on updated filters
        }
      }
    });
  }
}
