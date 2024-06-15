import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { concatMap, debounce } from 'rxjs/operators';
import { ContactActions } from '../actions/contact.actions';
import { HttpClient, HttpParams } from '@angular/common/http';
import { interval, of } from 'rxjs';
import { ContactForTableDto } from '../../dtos/contact-for-table-dto.type';
import { PaginationList } from '../../dtos/pagination-list.type';

@Injectable()
export class LoadContactEffects {
  loadContacts$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ContactActions['[REST/API]LoadContacts']),
      debounce(_ => interval(250)),
      concatMap((queryProp) =>
        // TODO: Create wrap for REST client & add url in config
        this.http
          .get<PaginationList<ContactForTableDto>>(
            'http://localhost:5000/api/v1/contacts',
            {
              params: new HttpParams().appendAll({ ...queryProp }),
            }
          )
          .pipe(
            concatMap((paginationList) =>
              of(
                ContactActions.clearContacts(),
                ContactActions.addContacts({ contacts: paginationList.rows }),
                ContactActions['[META]SetPagination']({ ...paginationList })
              )
            )
          )
      )
    )
  );

  constructor(
    private readonly actions$: Actions,
    private readonly http: HttpClient
  ) {}
}
