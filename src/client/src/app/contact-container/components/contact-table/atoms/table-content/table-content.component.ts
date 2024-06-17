import {
  AfterViewInit,
  Component,
  OnDestroy,
  OnInit,
  ViewChild,
  inject,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Store, select } from '@ngrx/store';
import { AppState, selectAll } from '../../../../store-features/contact.reducer';
import { ContactRestActions } from '../../../../store-features/actions/contact-rest.actions';
import { ContactState } from '../../../../store-features/contact-state.model';
import { EditFormComponent } from '../edit-form/edit-form.component';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { Subscription } from 'rxjs';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-table-content',
  standalone: true,
  imports: [
    MatInputModule,
    MatTableModule,
    MatSortModule,
    MatButton
  ],
  template: `
    <div class="mat-elevation-z8">
      <table
        mat-table
        [dataSource]="dataSource"
        matSort
        (matSortChange)="onSort($event)"
      >
        <ng-container matColumnDef="id">
          <th mat-header-cell *matHeaderCellDef mat-sort-header="Id">ID</th>
          <td mat-cell *matCellDef="let contact">{{ contact.id }}</td>
        </ng-container>

        <ng-container matColumnDef="firstName">
          <th mat-header-cell *matHeaderCellDef mat-sort-header="FirstName">
            First Name
          </th>
          <td mat-cell *matCellDef="let contact">{{ contact.firstName }}</td>
        </ng-container>

        <ng-container matColumnDef="lastName">
          <th mat-header-cell *matHeaderCellDef mat-sort-header="LastName">
            Last Name
          </th>
          <td mat-cell *matCellDef="let contact">{{ contact.lastName }}</td>
        </ng-container>

        <ng-container matColumnDef="email">
          <th mat-header-cell *matHeaderCellDef mat-sort-header="Email">
            Email
          </th>
          <td mat-cell *matCellDef="let contact">{{ contact.email }}</td>
        </ng-container>

        <ng-container matColumnDef="phone">
          <th mat-header-cell *matHeaderCellDef mat-sort-header="Phone">
            Phone
          </th>
          <td mat-cell *matCellDef="let contact">{{ contact.phone }}</td>
        </ng-container>

        <ng-container matColumnDef="title">
          <th mat-header-cell *matHeaderCellDef mat-sort-header="Title">
            Title
          </th>
          <td mat-cell *matCellDef="let contact">{{ contact.title }}</td>
        </ng-container>

        <ng-container matColumnDef="middleInitial">
          <th mat-header-cell *matHeaderCellDef mat-sort-header="MiddleInitial">
            Middle Initial
          </th>
          <td mat-cell *matCellDef="let contact">
            {{ contact.middleInitial }}
          </td>
        </ng-container>

        <ng-container matColumnDef="actions">
          <th mat-header-cell *matHeaderCellDef mat-sort-header>Actions</th>
          <td mat-cell *matCellDef="let contact">
            <button mat-button (click)="openEditContactDialog(contact)">
              Edit
            </button>
            <button mat-button (click)="removeContact(contact.id)">
              Delete
            </button>
          </td>
        </ng-container>

        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let contact; columns: displayedColumns"></tr>

        <tr class="mat-row" *matNoDataRow>
          <td class="mat-cell" colspan="4">No data matching the filter</td>
        </tr>
      </table>
    </div>
  `,
  styleUrl: './table-content.component.scss',
})
export class TableComponent implements OnInit, AfterViewInit, OnDestroy {
  private readonly store = inject(Store<AppState>);
  private readonly dialog = inject(MatDialog);
  private readonly subscriptionsForUnsubscribe: Subscription[] = [];

  displayedColumns: ReadonlyArray<string> = [
    'id',
    'firstName',
    'lastName',
    'email',
    'phone',
    'title',
    'middleInitial',
    'actions',
  ];

  dataSource!: MatTableDataSource<ContactState>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  contactsTableDataStream$ = this.store.pipe(select(selectAll));

  ngOnInit() {
    this.subscriptionsForUnsubscribe.push(
      this.contactsTableDataStream$.subscribe((contactsTableData) => {
        this.dataSource = new MatTableDataSource(contactsTableData);
      })
    );
  }

  ngOnDestroy() {
    this.subscriptionsForUnsubscribe
      .forEach((subscription) => subscription.unsubscribe());
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  openEditContactDialog(data: ContactState) {
    this.dialog.open(EditFormComponent, { data });
  }

  removeContact(contactId: number) {
    this.store.dispatch(ContactRestActions.deleteContact({ id: contactId }));
  }

  onSort({ active, direction }: Sort) {
    this.store.dispatch(
      ContactRestActions.loadContacts({
        orderBy: active,
        isDescending: direction === 'desc',
      })
    );
  }
}
