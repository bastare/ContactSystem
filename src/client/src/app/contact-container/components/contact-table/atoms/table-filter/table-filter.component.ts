import { Component, inject } from '@angular/core';
import { ContactRestActions } from '../../../../store-features/actions/contact-rest.actions';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../store-features/contact.reducer';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { getContactsForTableBySpecification } from '../../../../injectable/rest-client/specifications/get-contacts-by.specification';

@Component({
  selector: 'app-table-filter',
  standalone: true,
  imports: [ReactiveFormsModule],
  template: `
    <div class="table-filter-container">
      <form
        class="table-filter-container--form"
        [formGroup]="searchContactForm"
      >
        <div class="table-filter-container--form--group">
          <div class="table-filter-container--form--group--item">
            <input
              type="text"
              placeholder="Search"
              formControlName="search"
              (keyup)="onFilterInsert($event)"
            />
          </div>
          <div class="table-filter-container--form--group--item">
            /*
              TODO: Use structural directive:
              - create programmatically 'select' & 'div'
              - hide 'select' & show 'div' only
              - create & sync events for 'div' from 'select'
            */
            <select formControlName="searchBy" (change)="onSelect($event)">
              @for (entityForFilter of entitiesForFilter; track $index) {
                <option [value]="entityForFilter">
                  {{ entityForFilter }}
                </option>
              }
            </select>
          </div>
        </div>
      </form>
    </div>
  `,
  styleUrl: './table-filter.component.scss',
})
export class TableFilterComponent {
  private readonly store = inject(Store<AppState>);

  readonly entitiesForFilter: ReadonlyArray<string> = [
    'FirstName',
    'LastName',
    'Email',
    'Phone',
    'Title',
    'MiddleInitial',
  ];

  searchContactForm = new FormGroup({
    search: new FormControl(''),
    searchBy: new FormControl('Email', Validators.required),
  });

  onSelect(_: Event) {
    if (!this.searchContactForm.valid)
      return;

    this.fetchContacts();
  }

  onFilterInsert(_: KeyboardEvent) {
    if (!this.searchContactForm.valid)
      return;

    this.fetchContacts();
  }

  private fetchContacts() {
    this.store.dispatch(
      ContactRestActions.loadContacts(
        getContactsForTableBySpecification({
          ...(this.searchContactForm.value as {
            search?: string;
            searchBy: string;
          }),
        }),
      )
    );
  }
}
