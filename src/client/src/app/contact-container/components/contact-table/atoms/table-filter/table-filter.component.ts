import { Component, inject } from '@angular/core';
import { MatInputModule, MatLabel } from '@angular/material/input';
import { ContactRestActions } from '../../../../store-features/actions/contact-rest.actions';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../store-features/contact.reducer';

@Component({
  selector: 'app-table-filter',
  standalone: true,
  imports: [MatInputModule, MatLabel],
  template: `
    <mat-form-field>
      <mat-label>Search by email</mat-label>
      <input
        matInput
        (keyup)="onFilterInsert($event)"
        placeholder="Email"
        #input
      />
    </mat-form-field>
  `,
  styleUrl: './table-filter.component.scss',
})
export class TableFilterComponent {
  private readonly store: Store<AppState> = inject(Store);

  onFilterInsert({ target }: KeyboardEvent) {
    this.store.dispatch(
      ContactRestActions.loadContacts({
        ...resolveQueryDto({
          filterValue: (target as HTMLInputElement)?.value,
        }),
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
        `,
      };
    }
  }
}
