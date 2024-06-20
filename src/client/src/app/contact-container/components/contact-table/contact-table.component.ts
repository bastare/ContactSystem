import { Component, OnInit, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState, } from '../../store-features/contact.reducer';
import { ContactRestActions } from '../../store-features/actions/contact-rest.actions';
import { TableComponent } from './atoms/table-content/table-content.component';
import { TablePaginatorComponent } from './atoms/table-paginator/table-paginator.component';
import { TableFilterComponent } from './atoms/table-filter/table-filter.component';
import { AddContactBtnComponent } from './atoms/add-contact-btn/add-contact-btn.component';

@Component({
  selector: 'app-contact-table',
  standalone: true,
  imports: [
    TableComponent,
    TableFilterComponent,
    TablePaginatorComponent,
    AddContactBtnComponent
  ],
  template: `
    <div class="contact-table-container">
      <div class="contact-table-container--form">
        <app-table-filter />
        <app-table-content />
        <app-table-paginator
          style="align-self: flex-end;"
        />
      </div>
      <app-add-contact-btn />
    </div>
  `,
  styleUrl: './contact-table.component.scss',
})
export class ContactTableComponent implements OnInit {
  private readonly store = inject(Store<AppState>);

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
}
