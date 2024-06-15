import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { concatMap, withLatestFrom } from 'rxjs/operators';
import { ContactActions } from '../actions/contact.actions';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs';
import { State, selectMetaPagination } from '../contact.reducer';
import { Store } from '@ngrx/store';

@Injectable()
export class RemoveContactEffects {
  removeContactEffectsStream$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ContactActions['[REST/API]DeleteContact']),
      withLatestFrom(this.store.select(selectMetaPagination)),
      concatMap(([{ id }, meta]) =>
        // TODO: Create wrap for REST client & add url in config
        this.http.delete(`http://localhost:5000/api/v1/contacts/${id}`)
          .pipe(
            concatMap(() =>
              of(
                ContactActions['[REST/API]LoadContacts']({
                  offset: meta.currentOffset,
                  limit: meta.limit
                })
              )
            )
          )
      )
    )
  );

  constructor(
    private readonly actions$: Actions,
    private readonly http: HttpClient,
    private readonly store: Store<State>
  ) { }
}
