import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import {
  MatPaginator,
  MatPaginatorModule,
  PageEvent,
} from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Contact } from '../../store-features/contact.model';
import { Store, select } from '@ngrx/store';
import {
  State,
  selectAll,
  selectMetaPagination,
} from '../../store-features/contact.reducer';
import { ContactActions } from '../../store-features/actions/contact.actions';
import { MatButton } from '@angular/material/button';
import { AddFormComponent } from './atoms/add-form/add-form.component';
import { EditFormComponent } from './atoms/edit-form/edit-form.component';

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
        <table mat-table [dataSource]="dataSource" matSort>
          <ng-container matColumnDef="id">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>ID</th>
            <td mat-cell *matCellDef="let contact">{{ contact.id }}</td>
          </ng-container>

          <ng-container matColumnDef="firstName">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>
              First Name
            </th>
            <td mat-cell *matCellDef="let contact">{{ contact.firstName }}</td>
          </ng-container>

          <ng-container matColumnDef="lastName">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Last Name</th>
            <td mat-cell *matCellDef="let contact">{{ contact.lastName }}</td>
          </ng-container>

          <ng-container matColumnDef="email">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Email</th>
            <td mat-cell *matCellDef="let contact">{{ contact.email }}</td>
          </ng-container>

          <ng-container matColumnDef="phone">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Phone</th>
            <td mat-cell *matCellDef="let contact">{{ contact.phone }}</td>
          </ng-container>

          <ng-container matColumnDef="title">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Title</th>
            <td mat-cell *matCellDef="let contact">{{ contact.title }}</td>
          </ng-container>

          <ng-container matColumnDef="middleInitial">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>
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
        <!-- TODO: Refactor this shit -->
        <mat-paginator
          [pageIndex]="(metaPagination$ | async)!.currentOffset - 1"
          [pageSizeOptions]="[10]"
          [length]="(metaPagination$ | async)!.totalCount"
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
export class ContactTableComponent implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

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

  // TODO: Make it under one select
  data$ = this.store.pipe(select(selectAll));
  metaPagination$ = this.store.pipe(select(selectMetaPagination));

  dataSource!: MatTableDataSource<Contact>;

  ngOnInit() {
    this.store.dispatch(
      ContactActions['[REST/API]LoadContacts']({
        offset: 1,
        limit: 10,
      })
    );

    this.data$.subscribe((data) => {
      this.dataSource = new MatTableDataSource(data);
    });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  onFilterInsert({ target }: KeyboardEvent) {
    this.store.dispatch(
      ContactActions['[REST/API]LoadContacts']({
        ...resolveQueryDto({ filterValue: (target as HTMLInputElement).value }),
      })
    );

    function resolveQueryDto({ filterValue }: { filterValue?: string }) {
      return filterValue
        ? { expression: `Email.Contains("${filterValue}")` }
        : {};
    }
  }

  onPageChange(event: PageEvent) {
    this.store.dispatch(
      ContactActions['[REST/API]LoadContacts']({
        offset: event.pageIndex + 1,
        limit: event.pageSize,
      })
    );
  }

  openEditContactDialog(data: Contact) {
    this.dialog.open(EditFormComponent, { data });
  }

  openAddContactDialog() {
    this.dialog.open(AddFormComponent);
  }

  removeContact(contactId: number) {
    this.store.dispatch(
      ContactActions['[REST/API]DeleteContact']({ id: contactId })
    );
  }

  constructor(
    private readonly store: Store<State>,
    private readonly dialog: MatDialog
  ) {}
}
