import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { concatMap, withLatestFrom } from 'rxjs/operators';
import { ContactActions } from '../actions/contact.actions';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs';
import { Store } from '@ngrx/store';
import { State, selectMetaPagination } from '../contact.reducer';

@Injectable()
export class PatchContactEffects {
  patchContactEffectsStream$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ContactActions['[REST/API]UpdateContact']),
      withLatestFrom(this.store.select(selectMetaPagination)),
      concatMap(([{ contact }, meta]) =>
        // TODO: Create wrap for REST client & add url in config
        this.http
          .patch('http://localhost:5000/api/v1/contacts', contact)
          .pipe(
            concatMap(() =>
              of(
                ContactActions['[REST/API]LoadContacts']({
                  offset: meta.currentOffset,
                  limit: meta.limit
                }),
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
