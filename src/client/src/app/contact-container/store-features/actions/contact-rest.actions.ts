import { createActionGroup, props } from '@ngrx/store';
import { ContactState } from '../contact-state.model';

export const ContactRestActions = createActionGroup({
  source: 'Contact/REST',
  events: {
    'Add Contact': props<{ contact: ContactState }>(),
    'Load Contacts': props<
      | {
          expression?: string;
          projection?: string;
          orderBy?: string;
          isDescending?: boolean;
          offset?: number;
          limit?: number;
        }
      | undefined
    >(),
    'Delete Contact': props<{ id: number }>(),
    'Update Contact': props<{ contact: ContactState }>(),
  },
});
