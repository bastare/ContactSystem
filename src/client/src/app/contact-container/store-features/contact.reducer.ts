import { createFeature, createFeatureSelector, createReducer, createSelector, on } from '@ngrx/store';
import { EntityState, EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import { Contact } from './contact.model';
import { ContactActions } from './actions/contact.actions';

export const contactsFeatureKey = 'contacts';

export interface State extends EntityState<Contact> {
  _meta: {
    currentOffset: number;
    totalPages: number;
    limit: number;
    totalCount: number;
  }
}

export const adapter: EntityAdapter<Contact> = createEntityAdapter<Contact>();

export const initialState: State = adapter.getInitialState({
  _meta: {
    currentOffset: 1,
    totalPages: 0,
    limit: 10,
    totalCount: 0
  }
});

export const reducer = createReducer(
  initialState,
  on(ContactActions.addContact,
    (state, action) => adapter.addOne(action.contact, state)
  ),
  on(ContactActions.upsertContact,
    (state, action) => adapter.upsertOne(action.contact, state)
  ),
  on(ContactActions.addContacts,
    (state, action) => adapter.addMany(action.contacts, state)
  ),
  on(ContactActions.upsertContacts,
    (state, action) => adapter.upsertMany(action.contacts, state)
  ),
  on(ContactActions.updateContact,
    (state, action) => adapter.updateOne(action.contact, state)
  ),
  on(ContactActions.updateContacts,
    (state, action) => adapter.updateMany(action.contacts, state)
  ),
  on(ContactActions.deleteContact,
    (state, action) => adapter.removeOne(action.id, state)
  ),
  on(ContactActions.deleteContacts,
    (state, action) => adapter.removeMany(action.ids, state)
  ),
  on(ContactActions.loadContacts,
    (state, action) => adapter.setAll(action.contacts, state)
  ),
  on(ContactActions.clearContacts,
    state => adapter.removeAll(state)
  ),
  on(ContactActions['[META]SetPagination'], (state, action) => ({
    ...state,
    _meta: {
      ...state._meta,
      ...action
    }
  }))
);

const featureSelector = createFeatureSelector<State>(contactsFeatureKey);

export const contactsFeature = createFeature({
  name: contactsFeatureKey,
  reducer,
  extraSelectors: ({ selectContactsState }) => ({
    ...adapter.getSelectors(selectContactsState),
    selectMetaPagination: createSelector(
      featureSelector,
      (state) => ({ ...state._meta })
    )
  }),
});

export const {
  selectIds,
  selectEntities,
  selectAll,
  selectTotal,
  selectMetaPagination
} = contactsFeature;
