import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddressComponent } from './address/address.component';
import { CartComponent } from './cart/cart.component';
import { ContactUsComponent } from './contact-us/contact-us.component';
import { FaqComponent } from './faq/faq.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { OrderComponent } from './order/order.component';
import { ProductItemComponent } from './product-item/product-item.component';
import { ProductsComponent } from './products/products.component';
import { ProfileComponent } from './profile/profile.component';
import { StoryComponent } from './story/story.component';
import { VideosComponent } from './videos/videos.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { DashboardComponent } from './dashboard-page/dashboard/dashboard.component';
import { AuthGuard, RedirectGuard } from './guards/auth.guard';
import { DashboardProductComponent } from './dashboard-page/dashboard-product/dashboard-product.component';
import { DashboardUsersComponent } from './dashboard-page/dashboard-users/dashboard-users.component';
import { QuickLinksComponent } from './quick-links/quick-links.component';

const routes: Routes = [
  {
    path: '', component: HomeComponent, pathMatch: 'full', canActivate: [RedirectGuard]
  },
  {
    path: 'home', component: HomeComponent, pathMatch: 'full'
  },
  {
    path: 'products', component: ProductsComponent, pathMatch: 'full'
  },
  {
    path: 'products/:id/:name', component: ProductItemComponent, pathMatch: 'full'
  },
  {
    path: 'login', component: LoginComponent, pathMatch: 'full'
  },
  {
    path: 'profile', component: ProfileComponent, pathMatch: 'full'
  },
  {
    path: 'cart', component: CartComponent, pathMatch: 'full'
  },
  {
    path: 'orders/:status', component: OrderComponent, pathMatch: 'full', canActivate: [AuthGuard], data: { role: 'User' }
  },
  {
    path: 'orders', component: OrderComponent, pathMatch: 'full', canActivate: [AuthGuard], data: { role: 'User' }
  },
  {
    path: 'address', component: AddressComponent, pathMatch: 'full', canActivate: [AuthGuard], data: { role: 'User' }
  },
  {
    path: 'cart/address', component: AddressComponent, pathMatch: 'full', canActivate: [AuthGuard], data: { role: 'User' }
  },
  {
    path: 'story', component: StoryComponent, pathMatch: 'full'
  },
  {
    path: 'videos', component: VideosComponent, pathMatch: 'full'
  },
  {
    path: 'contact-us', component: ContactUsComponent, pathMatch: 'full'
  },
  {
    path: 'faq', component: FaqComponent, pathMatch: 'full'
  },
  {
    path: 'reset-pwd', component: ResetPasswordComponent, pathMatch: 'full'
  },
  {
    path: 'dashboard', component: DashboardComponent, pathMatch: 'full', canActivate: [AuthGuard], data: { role: 'Admin' },
  },
  {
    path: 'dashboard/products', component: DashboardProductComponent, pathMatch: 'full', canActivate: [AuthGuard], data: { role: 'Admin' },
  },
  {
    path: 'dashboard/users', component: DashboardUsersComponent, pathMatch: 'full', canActivate: [AuthGuard], data: { role: 'Admin' },
  },
  {
    path: 'privacy-policy', component: QuickLinksComponent, pathMatch: 'full'
  },
  {
    path: 'return-policy', component: QuickLinksComponent, pathMatch: 'full'
  },
  {
    path: 'shipping-policy', component: QuickLinksComponent, pathMatch: 'full'
  },
  {
    path: 'term-conditions', component: QuickLinksComponent, pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled', onSameUrlNavigation: 'reload', useHash: true },), CommonModule],
  exports: [RouterModule]
})
export class AppRoutingModule { }
