import { Component } from '@angular/core';
import { ContactTableComponent } from './components/contact-table/contact-table.component';
import { NgxNotifierComponent } from 'ngx-notifier';

@Component({
  selector: 'app-contact-container',
  standalone: true,
  imports: [ContactTableComponent, NgxNotifierComponent],
  template: `
    <div class="contact-container">
      <ngx-notifier />
      <app-contact-table />
    </div>`,
  styleUrl: './contact-container.component.scss'
})
export class ContactContainerComponent { }
