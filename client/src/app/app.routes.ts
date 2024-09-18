import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { ShopComponent } from './features/shop/shop.component';
import { ProductDetailsComponent } from './features/shop/product-details/product-details.component';

export const routes: Routes = [
    {path: '', component: HomeComponent},               // each route is an object
    {path: 'shop', component: ShopComponent},
    // :id => product-details companent, dynamic route 
    {path: 'shop/:id', component: ProductDetailsComponent},
    // wild card
    {path: '**', redirectTo: '', pathMatch: 'full'}     // if none of the above route then redirect to home page
];
