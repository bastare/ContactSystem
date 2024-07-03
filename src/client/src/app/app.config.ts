import {
  ApplicationConfig,
  provideZoneChangeDetection,
  isDevMode,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { contactsFeature } from './contact-container/store-features/contact.reducer';
import { provideAngularSvgIcon } from 'angular-svg-icon';
import ContactEffects from './contact-container/store-features/effects/index';


export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideStore({
      [contactsFeature.name]: contactsFeature.reducer,
    }),
    provideEffects([...ContactEffects]),
    provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() }),
    provideAnimationsAsync(),
    provideHttpClient(withInterceptorsFromDi()),
    provideAngularSvgIcon()
  ],
};
