import { Component, inject } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { MatDivider } from '@angular/material/divider';
import { MatListOption, MatSelectionList } from '@angular/material/list';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-filters-dialog',
  standalone: true,
  imports: [
    MatDivider,
    MatSelectionList,
    MatListOption,
    MatButton,
    FormsModule
  ],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss'
})
export class FiltersDialogComponent {
  shopService = inject(ShopService);                                  // Injecting the ShopService to access brand/type data
  private dialogRef = inject(MatDialogRef<FiltersDialogComponent>);   // Injecting MatDialogRef to control the dialog (open/close)
  data = inject(MAT_DIALOG_DATA);                                     // Injecting data passed to the dialog (selected brands/types)

  // Arrays to hold the selected brands and types
  selectedBrands: string[] = this.data.selectedBrands;
  selectedTypes: string[] = this.data.selectedTypes;

  // Method to close the dialog and pass the selected filters
  applyFilters() {
    this.dialogRef.close({          // close event to apply and close filter dialog
      selectedBrands: this.selectedBrands,
      selectedTypes: this.selectedTypes
    })
  }

}
