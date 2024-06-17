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
import { AddContactEffects } from './contact-container/store-features/effects/add-contact.effects';
import { PatchContactEffects } from './contact-container/store-features/effects/patch-contact.effects';
import { RemoveContactEffects } from './contact-container/store-features/effects/remove-contact.effects';
import { LoadContactEffects } from './contact-container/store-features/effects/load-contact.effects';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideStore({
      [contactsFeature.name]: contactsFeature.reducer,
    }),
    provideEffects([LoadContactEffects, AddContactEffects, PatchContactEffects, RemoveContactEffects]),
    provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() }),
    provideAnimationsAsync(),
    provideHttpClient(withInterceptorsFromDi())
  ],
};
