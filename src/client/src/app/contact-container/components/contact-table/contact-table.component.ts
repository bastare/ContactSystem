import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, OnInit, ViewChild, inject } from '@angular/core';
import {
  MatPaginator,
  MatPaginatorModule,
  PageEvent,
} from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ContactState } from '../../store-features/contact-state.model';
import { Store, select } from '@ngrx/store';
import {
  AppState,
  selectAll,
  selectContactsForTable,
  selectMetaPagination,
} from '../../store-features/contact.reducer';
import { MatButton } from '@angular/material/button';
import { AddFormComponent } from './atoms/add-form/add-form.component';
import { EditFormComponent } from './atoms/edit-form/edit-form.component';
import { ContactRestActions } from '../../store-features/actions/contact-rest.actions';

@Component({
  selector: 'app-contact-table',
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatButton,
  ],
  template: `
    <div class="container-table">
      <mat-form-field>
        <mat-label>Search by email</mat-label>
        <input
          matInput
          (keyup)="onFilterInsert($event)"
          placeholder="Email"
          #input
        />
      </mat-form-field>
      <div class="mat-elevation-z8">
        <table
          mat-table
          [dataSource]="dataSource"
          matSort
          (matSortChange)="onSort($event)"
        >
          <ng-container matColumnDef="id">
            <th
              mat-header-cell
              *matHeaderCellDef
              mat-sort-header="Id"
            >
              ID
            </th>
            <td mat-cell *matCellDef="let contact">{{ contact.id }}</td>
          </ng-container>

          <ng-container matColumnDef="firstName">
            <th
              mat-header-cell
              *matHeaderCellDef
              mat-sort-header="FirstName"
            >
              First Name
            </th>
            <td mat-cell *matCellDef="let contact">{{ contact.firstName }}</td>
          </ng-container>

          <ng-container matColumnDef="lastName">
            <th
              mat-header-cell
              *matHeaderCellDef
              mat-sort-header="LastName"
            >
              Last Name
            </th>
            <td mat-cell *matCellDef="let contact">{{ contact.lastName }}</td>
          </ng-container>

          <ng-container matColumnDef="email">
            <th
              mat-header-cell
              *matHeaderCellDef
              mat-sort-header="Email"
            >
              Email
            </th>
            <td mat-cell *matCellDef="let contact">{{ contact.email }}</td>
          </ng-container>

          <ng-container matColumnDef="phone">
            <th
              mat-header-cell
              *matHeaderCellDef
              mat-sort-header="Phone"
            >
              Phone
            </th>
            <td mat-cell *matCellDef="let contact">{{ contact.phone }}</td>
          </ng-container>

          <ng-container matColumnDef="title">
            <th
              mat-header-cell
              *matHeaderCellDef
              mat-sort-header="Title">
              Title
            </th>
            <td mat-cell *matCellDef="let contact">{{ contact.title }}</td>
          </ng-container>

          <ng-container matColumnDef="middleInitial">
            <th
              mat-header-cell
              *matHeaderCellDef
              mat-sort-header="MiddleInitial"
            >
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
      <div class="mat-elevation-z8">
        <!-- TODO: Refactor this shit -->
        <mat-paginator
          [pageIndex]="(contactsTableData$ | async)!.pagination.currentOffset - 1"
          [pageSizeOptions]="[10]"
          [length]="(contactsTableData$ | async)!.pagination.totalCount"
          aria-label="Contacts paginator"
          (page)="onPageChange($event)"
        >
        </mat-paginator>
      </div>

      <button mat-button (click)="openAddContactDialog()">Add</button>
    </div>
  `,
  styleUrl: './contact-table.component.scss',
})
export class ContactTableComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  private readonly store: Store<AppState> = inject(Store);
  private readonly dialog: MatDialog = inject(MatDialog);

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
  contactsTableData$ = this.store.pipe(select(selectContactsForTable));

  dataSource!: MatTableDataSource<ContactState>;

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

    this.contactsTableData$.subscribe((data) => {
      this.dataSource = new MatTableDataSource(data.rows);
    });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  onSort({active, direction}: Sort) {
    this.store.dispatch(
      ContactRestActions.loadContacts({
        orderBy: active,
        isDescending: direction === 'desc',
      })
    );
  }

  onFilterInsert({ target }: KeyboardEvent) {
    this.store.dispatch(
      ContactRestActions.loadContacts({
        ...resolveQueryDto({ filterValue: (target as HTMLInputElement)?.value }),
      })
    );

    function resolveQueryDto({ filterValue }: { filterValue?: string }) {
      return {
        expression: filterValue
          ? `
            Email.Contains("${filterValue}")
              && !string.IsNullOrEmpty(FirstName)
              && !string.IsNullOrEmpty(LastName)
              && !string.IsNullOrEmpty(Email)
              && !string.IsNullOrEmpty(Phone)
          `
          : `
            !string.IsNullOrEmpty(FirstName)
              && !string.IsNullOrEmpty(LastName)
              && !string.IsNullOrEmpty(Email)
              && !string.IsNullOrEmpty(Phone)
        `
      };
    }
  }

  onPageChange(event: PageEvent) {
    this.store.dispatch(
      ContactRestActions.loadContacts({
        offset: event.pageIndex + 1,
        limit: event.pageSize,
      })
    );
  }

  openEditContactDialog(data: ContactState) {
    this.dialog.open(EditFormComponent, { data });
  }

  openAddContactDialog() {
    this.dialog.open(AddFormComponent);
  }

  removeContact(contactId: number) {
    this.store.dispatch(
      ContactRestActions.deleteContact({ id: contactId })
    );
  }
}
