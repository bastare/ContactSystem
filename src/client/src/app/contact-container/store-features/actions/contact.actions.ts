import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { Update } from '@ngrx/entity';

import { Contact } from '../contact.model';

export const ContactActions = createActionGroup({
  source: 'Contact/API',
  events: {
    'Load Contacts': props<{ contacts: Contact[] }>(),
    'Add Contact': props<{ contact: Contact }>(),
    'Upsert Contact': props<{ contact: Contact }>(),
    'Add Contacts': props<{ contacts: Contact[] }>(),
    'Upsert Contacts': props<{ contacts: Contact[] }>(),
    'Update Contact': props<{ contact: Update<Contact> }>(),
    'Update Contacts': props<{ contacts: Update<Contact>[] }>(),
    'Delete Contact': props<{ id: string }>(),
    'Delete Contacts': props<{ ids: string[] }>(),
    'Clear Contacts': emptyProps(),

    //TODO: Create separate feature for this
    '[REST/API] Add Contact': props<{ contact: Contact }>(),
    '[REST/API] Load Contacts': props<{ expression?: string, orderBy?: string, isDescending?: boolean, offset?: number, limit?: number }>(),
    '[REST/API] Delete Contact': props<{ id: number }>(),
    '[REST/API] Update Contact': props<{ contact: Contact }>(),

    //TODO: Create separate feature for this
    '[META] SetPagination': props<{
      expression?: string;
      orderBy?: string;
      isDescending?: boolean
      currentOffset?: number;
      limit?: number;
      totalPages?: number;
      totalCount?: number;
    }>(),
  }
});
