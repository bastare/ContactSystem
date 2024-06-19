import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, concatMap, tap } from 'rxjs/operators';
import { EMPTY, of } from 'rxjs';
import { ContactRestClient } from '../../injectable/rest-client/contact-rest-client.service';
import { ContactRestActions } from '../actions/contact-rest.actions';
import { NgxNotifierService } from 'ngx-notifier';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AddContactEffects {
  private readonly actions$ = inject(Actions);
  private readonly contactRestClient = inject(ContactRestClient);
  private readonly ngxNotifierService = inject(NgxNotifierService);

  addContactEffectStream$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ContactRestActions.addContact),
      concatMap(({ contact }) =>
        this.contactRestClient.addContact$({ contactForAdd: contact }).pipe(
          concatMap(() =>
            of(
              ContactRestActions.loadContacts({}),
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
