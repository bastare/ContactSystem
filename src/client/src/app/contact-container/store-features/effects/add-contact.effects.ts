import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { concatMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { ContactRestClient } from '../../injectable/rest-client/contact-rest-client.service';
import { ContactRestActions } from '../actions/contact-rest.actions';

@Injectable({
  providedIn: 'root',
})
export class AddContactEffects {
  private readonly actions$ = inject(Actions);
  private readonly contactRestClient = inject(ContactRestClient);

  addContactEffectStream$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ContactRestActions.addContact),
      concatMap(({ contact }) =>
        this.contactRestClient.addContact$({ contactForAdd: contact }).pipe(
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
