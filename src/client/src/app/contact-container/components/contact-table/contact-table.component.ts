import { Component, OnInit, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Store } from '@ngrx/store';
import { AppState, } from '../../store-features/contact.reducer';
import { AddFormComponent } from './atoms/add-form/add-form.component';
import { ContactRestActions } from '../../store-features/actions/contact-rest.actions';
import { TableComponent } from './atoms/table-content/table-content.component';
import { TablePaginatorComponent } from './atoms/table-paginator/table-paginator.component';
import { TableFilterComponent } from './atoms/table-filter/table-filter.component';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-contact-table',
  standalone: true,
  imports: [
    MatButton,
    TableComponent,
    TableFilterComponent,
    TablePaginatorComponent
  ],
  template: `
    <div class="container-table">
      <app-table-filter />
      <app-table-content />
      <app-table-paginator />
      <button mat-button (click)="openAddContactDialog()">Add</button>
    </div>
  `,
  styleUrl: './contact-table.component.scss',
})
export class ContactTableComponent implements OnInit {
  private readonly store = inject(Store<AppState>);
  private readonly dialog = inject(MatDialog);

  ngOnInit() {
    this.store.dispatch(
      ContactRestActions.loadContacts({
        expression: `
          !string.IsNullOrEmpty(FirstName)
            && !string.IsNullOrEmpty(LastName)
            && !string.IsNullOrEmpty(Email)
            && !string.IsNullOrEmpty(Phone)
        `,
        projection: `
          new(
            Id,
            FirstName,
            LastName,
            Title,
            Email,
            Phone,
            MiddleInitial
          )
        `
      })
    );
  }

  openAddContactDialog() {
    this.dialog.open(AddFormComponent);
  }
}
