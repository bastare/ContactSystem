import {
  AfterViewInit,
  Component,
  OnDestroy,
  OnInit,
  ViewChild,
  inject
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Store } from '@ngrx/store';
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
import { SvgIconComponent } from 'angular-svg-icon';

@Component({
  selector: 'app-table-content',
  standalone: true,
  imports: [
    MatInputModule,
    MatTableModule,
    MatSortModule,
    MatButton,
    SvgIconComponent
  ],
  template: `
    <div class="table-content-container">
      <table
        [style]="{ backgroundColor: 'white' }"
        mat-table
        matSort
        [dataSource]="dataSource"
        (matSortChange)="onSort($event)"
      >
        <ng-container matColumnDef="id">
          <th mat-header-cell *matHeaderCellDef mat-sort-header="Id">ID</th>
          <td mat-cell *matCellDef="let contact">{{ contact?.id }}</td>
        </ng-container>

        <ng-container matColumnDef="firstName">
          <th mat-header-cell *matHeaderCellDef mat-sort-header="FirstName">
            First Name
          </th>
          <td mat-cell *matCellDef="let contact">{{ contact?.firstName }}</td>
        </ng-container>

        <ng-container matColumnDef="lastName">
          <th mat-header-cell *matHeaderCellDef mat-sort-header="LastName">
            Last Name
          </th>
          <td mat-cell *matCellDef="let contact">{{ contact?.lastName }}</td>
        </ng-container>

        <ng-container matColumnDef="email">
          <th mat-header-cell *matHeaderCellDef mat-sort-header="Email">
            Email
          </th>
          <td mat-cell *matCellDef="let contact">{{ contact?.email }}</td>
        </ng-container>

        <ng-container matColumnDef="phone">
          <th mat-header-cell *matHeaderCellDef mat-sort-header="Phone">
            Phone
          </th>
          <td mat-cell *matCellDef="let contact">{{ contact?.phone }}</td>
        </ng-container>

        <ng-container matColumnDef="title">
          <th mat-header-cell *matHeaderCellDef mat-sort-header="Title">
            Title
          </th>
          <td mat-cell *matCellDef="let contact">{{ contact?.title }}</td>
        </ng-container>

        <ng-container matColumnDef="middleInitial">
          <th mat-header-cell *matHeaderCellDef mat-sort-header="MiddleInitial">
            Middle Initial
          </th>
          <td mat-cell *matCellDef="let contact">
            {{ contact?.middleInitial }}
          </td>
        </ng-container>

        <ng-container matColumnDef="actions">
          <th mat-header-cell *matHeaderCellDef></th>
          <td mat-cell *matCellDef="let contact">
            <div class="table-content-container--btn-group">
              <!-- TODO: Put 'on hover' animation 'red & green' etc  -->
              <button type="button" (click)="openEditContactDialog(contact)">
                <svg-icon
                  src="contact/edit-icon.svg"
                  alt="edit-icon"
                />
              </button>
              <button type="button" (click)="removeContact(contact.id)">
                <svg-icon
                  src="contact/trash-icon.svg"
                  alt="trash-icon"
                />
              </button>
            </div>
          </td>
        </ng-container>

        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let contact; columns: displayedColumns"></tr>
      </table>
    </div>
  `,
  styleUrl: './table-content.component.scss',
})
export class TableComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  private readonly store = inject(Store<AppState>);
  private readonly dialog = inject(MatDialog);

  private readonly subscriptionsForUnsubscribe: Subscription[] = [];

  private readonly contactsTableDataStream$ = this.store.select(selectAll);

  readonly displayedColumns: readonly string[] = [
    'id',
    'firstName',
    'lastName',
    'email',
    'phone',
    'title',
    'middleInitial',
    'actions',
  ];

  dataSource: MatTableDataSource<ContactState> = new MatTableDataSource([] as ContactState[]);

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
