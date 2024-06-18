import { Component } from '@angular/core';
import { ContactTableComponent } from './components/contact-table/contact-table.component';

@Component({
  selector: 'app-contact-container',
  standalone: true,
  imports: [ContactTableComponent],
  template: `
    <div class="contact-container">
      <app-contact-table></app-contact-table>
    </div>`,
  styleUrl: './contact-container.component.scss'
})
export class ContactContainerComponent { }
