import { AsyncPipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ContactState } from '../../store-features/contact-state.model';
import { Store, select } from '@ngrx/store';
import {
  AppState,
  selectAll,
  selectContactsForTable,
} from '../../store-features/contact.reducer';
import { AddFormComponent } from './atoms/add-form/add-form.component';
import { ContactRestActions } from '../../store-features/actions/contact-rest.actions';
import { TableComponent } from './atoms/table/table.component';
import { map, tap } from 'rxjs';
import { TablePaginatorComponent } from './atoms/table-paginator/table-paginator.component';
import { TableFilterComponent } from './atoms/table-filter/table-filter.component';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-contact-table',
  standalone: true,
  imports: [
    AsyncPipe,
    MatButton,
    TableComponent,
    TableFilterComponent,
    TablePaginatorComponent
  ],
  template: `
    <div class="container-table">
      <app-table-filter />
      <app-table />
      <app-table-paginator />
      <button mat-button (click)="openAddContactDialog()">Add</button>
    </div>
  `,
  styleUrl: './contact-table.component.scss',
})
export class ContactTableComponent implements OnInit {
  private readonly store: Store<AppState> = inject(Store);
  private readonly dialog: MatDialog = inject(MatDialog);

  ngOnInit() {
    this.store.dispatch(
      ContactRestActions.loadContacts({
        expression: `
          !string.IsNullOrEmpty(FirstName)
            && !string.IsNullOrEmpty(LastName)
            && !string.IsNullOrEmpty(Email)
            && !string.IsNullOrEmpty(Phone)`,
        offset: 1,
        limit: 10,
      })
    );
  }

  openAddContactDialog() {
    this.dialog.open(AddFormComponent);
  }
}
