import { Component, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddFormComponent } from '../add-form/add-form.component';

@Component({
  selector: 'app-add-contact-btn',
  standalone: true,
  imports: [],
  template: `
    <div class="add-contact-btn-container">
      <button (click)="openAddContactDialog()">
        Add
      </button>
    </div>
  `,
  styleUrl: './add-contact-btn.component.scss'
})
export class AddContactBtnComponent {
  private readonly dialog = inject(MatDialog);

  openAddContactDialog() {
    this.dialog.open(AddFormComponent);
  }
}
