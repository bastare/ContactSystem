import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { concatMap } from 'rxjs/operators';
import { ContactActions } from '../actions/contact.actions';
import { of } from 'rxjs';
import { ContactRestActions } from '../actions/contact-rest.actions';
import { ContactRestClient } from '../../injectable/rest-client/contact-rest-client.service';

@Injectable({
  providedIn: 'root',
})
export class PatchContactEffects {
  private readonly actions$ = inject(Actions);
  private readonly contactRestClient = inject(ContactRestClient);

  patchContactEffectsStream$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ContactRestActions.updateContact),
      concatMap(({ contact }) =>
        this.contactRestClient.patchContact({ contactForPatch: contact }).pipe(
          concatMap((patchedContact) =>
            of(
              ContactActions.updateContact({
                contact: {
                  id: patchedContact.id,
                  changes: { ...patchedContact },
                },
              })
            )
          )
        )
      )
    )
  );
}
