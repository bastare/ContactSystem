import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { Update } from '@ngrx/entity';
import { ContactState } from '../contact-state.model';

export const ContactActions = createActionGroup({
  source: 'Contact',
  events: {
    'Load Contacts': props<{ contacts: ContactState[] }>(),
    'Add Contact': props<{ contact: ContactState }>(),
    'Upsert Contact': props<{ contact: ContactState }>(),
    'Add Contacts': props<{ contacts: ContactState[] }>(),
    'Upsert Contacts': props<{ contacts: ContactState[] }>(),
    'Update Contact': props<{ contact: Update<ContactState> }>(),
    'Update Contacts': props<{ contacts: Update<ContactState>[] }>(),
    'Delete Contact': props<{ id: string }>(),
    'Delete Contacts': props<{ ids: string[] }>(),
    'Clear Contacts': emptyProps()
  }
});
