import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { ContactRestActions } from '../../../../store-features/actions/contact-rest.actions';
import { Store, select } from '@ngrx/store';
import { AppState, selectMetaPaginationOffsetData } from '../../../../store-features/contact.reducer';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-table-paginator',
  standalone: true,
  imports: [
    MatPaginatorModule
  ],
  template: `
    <div class="mat-elevation-z8">
      <mat-paginator
        [pageIndex]="currentOffset - 1"
        [pageSizeOptions]="[10]"
        [length]="totalCount"
        aria-label="Contacts paginator"
        (page)="onPageChange($event)"
      />
    </div>
  `,
  styleUrl: './table-paginator.component.scss',
})
export class TablePaginatorComponent implements OnInit, OnDestroy {
  private readonly store = inject(Store<AppState>);
  private readonly subscriptionsForUnsubscribe: Subscription[] = [];

  contactTableMetaPageOffsetStream$ = this.store.pipe(select(selectMetaPaginationOffsetData))

  currentOffset: number = 1;
  totalCount: number = 0;

  ngOnInit() {
    this.subscriptionsForUnsubscribe.push(
      this.contactTableMetaPageOffsetStream$.subscribe(({ currentOffset, totalCount }) => {
        this.currentOffset = currentOffset;
        this.totalCount = totalCount;
      })
    );
  }

  ngOnDestroy() {
    this.subscriptionsForUnsubscribe
      .forEach(({ unsubscribe }) => unsubscribe());
  }

  onPageChange({ pageIndex, pageSize }: PageEvent) {
    this.store.dispatch(
      ContactRestActions.loadContacts({
        offset: pageIndex + 1,
        limit: pageSize,
      })
    );
  }
}
