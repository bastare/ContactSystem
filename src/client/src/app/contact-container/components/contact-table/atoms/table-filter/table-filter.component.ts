import { Component, ElementRef, ViewChild, inject } from '@angular/core';
import { MatInputModule, MatLabel } from '@angular/material/input';
import { ContactRestActions } from '../../../../store-features/actions/contact-rest.actions';
import { Store } from '@ngrx/store';
import { AppState } from '../../../../store-features/contact.reducer';
import { MatOption, MatSelect, MatSelectChange } from '@angular/material/select';

@Component({
  selector: 'app-table-filter',
  standalone: true,
  imports: [MatInputModule, MatLabel, MatSelect, MatOption],
  template: `
    <mat-form-field>
      <mat-label>Search</mat-label>
      <input
        matInput
        (keyup)="onFilterInsert($event)"
        placeholder="Email"
        #input
      />
    </mat-form-field>
    <mat-form-field>
      <mat-label>Search By</mat-label>
      <mat-select
        (selectionChange)="onSelect($event)"
        #select
      >
        @for (entityForFilter of entitiesForFilter; track $index) {
        <mat-option [value]="entityForFilter">
          {{ entityForFilter }}
        </mat-option>
        }
      </mat-select>
    </mat-form-field>
  `,
  styleUrl: './table-filter.component.scss',
})
export class TableFilterComponent {
  @ViewChild('input') inputRef!: ElementRef;
  @ViewChild(MatSelect) select!: MatSelect;

  private readonly store = inject(Store<AppState>);

  entitiesForFilter: ReadonlyArray<string> = [
    'FirstName',
    'LastName',
    'Email',
    'Phone',
    'Title',
    'MiddleInitial',
  ];

  onSelect(matSelectChange: MatSelectChange) {
    this.store.dispatch(
      ContactRestActions.loadContacts({
        expression: `
          ${matSelectChange.value}.Contains("${this.inputRef.nativeElement.value}")
            && !string.IsNullOrEmpty(FirstName)
            && !string.IsNullOrEmpty(LastName)
            && !string.IsNullOrEmpty(Email)
            && !string.IsNullOrEmpty(Phone)
        `
      })
    );
  }

  onFilterInsert({ target }: KeyboardEvent) {
    this.store.dispatch(
      ContactRestActions.loadContacts({
        ...resolveQuery({
          filterValue: (target as HTMLInputElement)?.value,
          entity: this.select.value
        }),
      })
    );

    function resolveQuery({ filterValue, entity }: { filterValue?: string, entity: string }) {
      return {
        expression: filterValue
          ? `
            ${entity}.Contains("${filterValue}")
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
