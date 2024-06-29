import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { ContactRestActions } from '../../../../store-features/actions/contact-rest.actions';
import { Store } from '@ngrx/store';
import { AppState, selectMetaPaginationOffsetData } from '../../../../store-features/contact.reducer';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-table-paginator',
  standalone: true,
  imports: [
    MatPaginatorModule
  ],
  template: `
    <div>
      <mat-paginator
        [style]="{ backgroundColor: 'white' }"
        [pageIndex]="currentOffset - 1"
        [pageSizeOptions]="[pageSize]"
        [length]="totalCount"
        (page)="onPageChange($event)"
      />
    </div>
  `,
})
export class TablePaginatorComponent implements OnInit, OnDestroy {
  private readonly store = inject(Store<AppState>);
  private readonly subscriptionsForUnsubscribe: Subscription[] = [];

  private readonly contactTableMetaPageOffsetStream$ = this.store.select(selectMetaPaginationOffsetData)

  readonly pageSize: number = 10;

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
      .forEach((subscription) => subscription.unsubscribe());
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
