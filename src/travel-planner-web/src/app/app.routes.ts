import { Routes } from '@angular/router';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { LoginPageComponent } from './auth/login-page/login-page.component';
import { MyTripsComponent } from './trips/my-trips/my-trips.component';
import { ContactComponent } from './common/contact/contact.component';

export const routes: Routes = [
  {
    path: '',
    component: LandingPageComponent,
  },
  {
    path: 'login',
    component: LoginPageComponent,
  },
  {
    path: 'my-trips',
    component: MyTripsComponent,
  },
  {
    path: 'contact',
    component: ContactComponent,
  },
];
