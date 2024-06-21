import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, concatMap } from 'rxjs/operators';
import { ContactActions } from '../actions/contact.actions';
import { EMPTY, of } from 'rxjs';
import { ContactRestActions } from '../actions/contact-rest.actions';
import { ContactRestClient } from '../../injectable/rest-client/contact-rest-client.service';
import { ContactState } from '../contact-state.model';
import { HttpErrorResponse } from '@angular/common/http';
import { NgxNotifierService } from 'ngx-notifier';

@Injectable({
  providedIn: 'root',
})
export class PatchContactEffects {
  private readonly actions$ = inject(Actions);
  private readonly contactRestClient = inject(ContactRestClient);
  private readonly ngxNotifierService = inject(NgxNotifierService);

  patchContactEffectsStream$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ContactRestActions.updateContact),
      concatMap(({ contact: { id, changes } }) =>
        this.contactRestClient.patchContact$({
          contactId: id as number,
          contactForPatch: changes as ContactState
        }).pipe(
          concatMap((patchedContact) =>
            of(
              ContactActions.updateContact({
                contact: {
                  id: patchedContact.id,
                  changes: { ...patchedContact },
                },
              })
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
