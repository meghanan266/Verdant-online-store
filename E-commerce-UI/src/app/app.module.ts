import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { ProductsComponent } from './products/products.component';

import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import { FooterComponent } from './footer/footer.component';
import { LoginComponent } from './login/login.component';
import { ProductItemComponent } from './product-item/product-item.component';
import { ProfileComponent } from './profile/profile.component';
import { AlertComponent } from './shared/alert/alert.component';
import { LoaderComponent } from './shared/loader/loader.component'
import { LoadingInterceptor } from './shared/interceptor/loading.interceptor';
import { CartComponent } from './cart/cart.component';
import { DialogBoxComponent } from './shared/dialog-box/dialog-box.component';
import { TokenInterceptor } from './shared/interceptor/token.interceptor';
import { OrderComponent } from './order/order.component';
import { AddressComponent } from './address/address.component';
import { FaqComponent } from './faq/faq.component';
import { VideosComponent } from './videos/videos.component';
import { ContactUsComponent } from './contact-us/contact-us.component';
import { StoryComponent } from './story/story.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    HomeComponent,
    ProductsComponent,
    FooterComponent,
    LoginComponent,
    ProductItemComponent,
    ProfileComponent,
    AlertComponent,
    LoaderComponent,
    CartComponent,
    DialogBoxComponent,
    OrderComponent,
    AddressComponent,
    FaqComponent,
    VideosComponent,
    ContactUsComponent,
    StoryComponent,
    ResetPasswordComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule, 
    RouterModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true
    },
    {
      provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
