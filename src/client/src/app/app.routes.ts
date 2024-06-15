import { Routes } from '@angular/router';
import { ContactContainerComponent } from './contact-container/contact-container.component';

export const routes: Routes = [
  {
    path: "",
    pathMatch: "full",
    redirectTo: "contacts",
  },
  {
    path: 'contacts',
    component: ContactContainerComponent
  }
];
