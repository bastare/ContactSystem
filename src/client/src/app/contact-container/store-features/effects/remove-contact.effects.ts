import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, concatMap } from 'rxjs/operators';
import { EMPTY, of } from 'rxjs';
import { ContactRestActions } from '../actions/contact-rest.actions';
import { ContactRestClient } from '../../injectable/rest-client/contact-rest-client.service';
import { HttpErrorResponse } from '@angular/common/http';
import { NgxNotifierService } from 'ngx-notifier';

@Injectable({
  providedIn: 'root',
})
export class RemoveContactEffects {
  private readonly actions$ = inject(Actions);
  private readonly contactRestClient = inject(ContactRestClient);
  private readonly ngxNotifierService = inject(NgxNotifierService);

  removeContactEffectsStream$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ContactRestActions.deleteContact),
      concatMap(({ id }) =>
        this.contactRestClient.removeContact$({ contactId: id }).pipe(
          concatMap(() =>
            of(
              ContactRestActions.loadContacts({})
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
