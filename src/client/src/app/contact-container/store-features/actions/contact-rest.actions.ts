import { createActionGroup, props } from '@ngrx/store';
import { ContactState } from '../contact-state.model';
import { Update } from '@ngrx/entity';
import { QueryPropsDto } from '../../injectable/rest-client/dtos/query-props-dto.type';

export const ContactRestActions = createActionGroup({
  source: 'Contact/REST',
  events: {
    'Add Contact': props<{ contact: ContactState }>(),
    'Load Contacts': props<QueryPropsDto>(),
    'Delete Contact': props<{ id: number }>(),
    'Update Contact': props<{ contact: Update<ContactState> }>(),
  },
});
