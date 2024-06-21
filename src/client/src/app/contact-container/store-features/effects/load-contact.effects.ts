import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, debounce, switchMap, withLatestFrom } from 'rxjs/operators';
import { ContactActions } from '../actions/contact.actions';
import { EMPTY, interval, of } from 'rxjs';
import { ContactRestActions } from '../actions/contact-rest.actions';
import { ContactMetaActions } from '../actions/contact-meta.actions';
import { ContactRestClient } from '../../injectable/rest-client/contact-rest-client.service';
import { Store } from '@ngrx/store';
import { AppState, selectMetaFilter, selectMetaPaginationOffsetData } from '../contact.reducer';
import { HttpErrorResponse } from '@angular/common/http';
import { NgxNotifierService } from 'ngx-notifier';

@Injectable({
  providedIn: 'root',
})
export class LoadContactEffects {
  private readonly actions$ = inject(Actions);
  private readonly contactRestClient = inject(ContactRestClient);
  private readonly store = inject(Store<AppState>);
  private readonly ngxNotifierService = inject(NgxNotifierService);

  loadContactEffectStream$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ContactRestActions.loadContacts),
      debounce(_ => interval(250)),
      withLatestFrom(
        this.store.select(selectMetaPaginationOffsetData),
        this.store.select(selectMetaFilter)
      ),
      switchMap(([newQuery, previousPaginationOffsetQueryFragment, previousFilterQueryFragment]) =>
        this.contactRestClient.fetchContacts$({
          ...previousPaginationOffsetQueryFragment,
          ...previousFilterQueryFragment,
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
            ),
            catchError(({ error }: HttpErrorResponse) => {
              this.ngxNotifierService.createToast(error.message, 'danger');

              return EMPTY;
            })
          )
      )
    )
  );
}
