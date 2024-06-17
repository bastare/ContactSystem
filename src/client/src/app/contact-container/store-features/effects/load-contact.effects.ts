import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { concatMap, debounce, switchMap, withLatestFrom } from 'rxjs/operators';
import { ContactActions } from '../actions/contact.actions';
import { interval, of } from 'rxjs';
import { ContactRestActions } from '../actions/contact-rest.actions';
import { ContactMetaActions } from '../actions/contact-meta.actions';
import { ContactRestClient } from '../../injectable/rest-client/contact-rest-client.service';
import { Store } from '@ngrx/store';
import { AppState, selectMetaFilter, selectMetaPagination } from '../contact.reducer';

@Injectable({
  providedIn: 'root',
})
export class LoadContactEffects {
  private readonly actions$ = inject(Actions);
  private readonly contactRestClient = inject(ContactRestClient);
  private readonly store = inject(Store<AppState>);

  loadContactEffectStream$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ContactRestActions.loadContacts),
      debounce(_ => interval(250)),
      withLatestFrom(
        this.store.select(selectMetaPagination),
        this.store.select(selectMetaFilter)
      ),
      switchMap(([newQuery, previousPaginationQuery, previousFilterQuery]) =>
        this.contactRestClient.fetchContacts$({
          ...previousPaginationQuery,
          ...previousFilterQuery,
          ...newQuery
        })
          .pipe(
            switchMap((paginationList) =>
              of(
                ContactActions.clearContacts(),
                ContactActions.addContacts({ contacts: paginationList.rows }),
                ContactMetaActions.setPagination({ ...paginationList }),
                ContactMetaActions.setFilter({ ...newQuery })
              )
            )
          )
      )
    )
  );
}
