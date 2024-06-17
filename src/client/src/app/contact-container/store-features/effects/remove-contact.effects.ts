import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { concatMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { ContactRestActions } from '../actions/contact-rest.actions';
import { ContactRestClient } from '../../injectable/rest-client/contact-rest-client.service';

@Injectable({
  providedIn: 'root',
})
export class RemoveContactEffects {
  private readonly actions$ = inject(Actions);
  private readonly contactRestClient = inject(ContactRestClient);

  removeContactEffectsStream$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ContactRestActions.deleteContact),
      concatMap(({ id }) =>
        this.contactRestClient.removeContact$({ contactId: id }).pipe(
          concatMap(() =>
            of(
              ContactRestActions.loadContacts({})
            )
          )
        )
      )
    )
  );
}
