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
          <input formControlName="search" (keyup)="onFilterInsert($event)" />
        </div>
        <div class="table-filter-container--form--group">
          <select formControlName="searchBy" (change)="onSelect($event)">
            @for (entityForFilter of entitiesForFilter; track $index) {
              <option [value]="entityForFilter">
                {{ entityForFilter }}
              </option>
            }
          </select>
        </div>
      </form>
    </div>
  `,
  styleUrl: './table-filter.component.scss',
})
export class TableFilterComponent {
  private readonly store = inject(Store<AppState>);

  searchContactForm = new FormGroup({
    search: new FormControl(''),
    searchBy: new FormControl('Email', Validators.required),
  });

  entitiesForFilter: ReadonlyArray<string> = [
    'FirstName',
    'LastName',
    'Email',
    'Phone',
    'Title',
    'MiddleInitial',
  ];

  onSelect(_: Event) {
    if (!this.searchContactForm.valid)
      return;

    this.store.dispatch(
      ContactRestActions.loadContacts({
        expression: `
          ${this.searchContactForm.value.searchBy}.Contains("${this.searchContactForm.value.search}")
            && !string.IsNullOrEmpty(FirstName)
            && !string.IsNullOrEmpty(LastName)
            && !string.IsNullOrEmpty(Email)
            && !string.IsNullOrEmpty(Phone)
        `,
      })
    );
  }

  onFilterInsert(_: Event) {
    if (!this.searchContactForm.valid)
      return;

    this.store.dispatch(
      ContactRestActions.loadContacts({
        ...resolveQuery({
          ...(this.searchContactForm.value as {
            search?: string;
            searchBy: string;
          }),
        }),
      })
    );

    function resolveQuery({
      search,
      searchBy,
    }: {
      search?: string;
      searchBy: string;
    }) {
      return {
        expression: search
          ? `
            ${searchBy}.Contains("${search}")
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
        `,
      };
    }
  }
}
