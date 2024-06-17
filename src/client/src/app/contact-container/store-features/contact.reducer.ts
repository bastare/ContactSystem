import {
  createFeature,
  createFeatureSelector,
  createReducer,
  createSelector,
  on,
} from '@ngrx/store';
import { EntityState, EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import { ContactState } from './contact-state.model';
import { ContactActions } from './actions/contact.actions';
import { ContactMetaActions } from './actions/contact-meta.actions';

export const contactsFeatureKey = 'contacts';

export interface AppState extends EntityState<ContactState> {
  _meta: {
    pagination: {
      currentOffset: number;
      totalPages: number;
      limit: number;
      totalCount: number;
    };
    filter: {
      expression?: string;
      orderBy?: string;
      isDescending?: boolean;
      projection?: string;
    };
  };
}

export const adapter: EntityAdapter<ContactState> =
  createEntityAdapter<ContactState>();

export const initialState: AppState = adapter.getInitialState({
  _meta: {
    pagination: {
      currentOffset: 1,
      totalPages: 0,
      limit: 10,
      totalCount: 0,
    },
    filter: {},
  },
});

export const reducer = createReducer(
  initialState,
  on(ContactActions.addContact, (state, action) =>
    adapter.addOne(action.contact, state)
  ),
  on(ContactActions.upsertContact, (state, action) =>
    adapter.upsertOne(action.contact, state)
  ),
  on(ContactActions.addContacts, (state, action) =>
    adapter.addMany(action.contacts, state)
  ),
  on(ContactActions.upsertContacts, (state, action) =>
    adapter.upsertMany(action.contacts, state)
  ),
  on(ContactActions.updateContact, (state, action) =>
    adapter.updateOne(action.contact, state)
  ),
  on(ContactActions.updateContacts, (state, action) =>
    adapter.updateMany(action.contacts, state)
  ),
  on(ContactActions.deleteContact, (state, action) =>
    adapter.removeOne(action.id, state)
  ),
  on(ContactActions.deleteContacts, (state, action) =>
    adapter.removeMany(action.ids, state)
  ),
  on(ContactActions.loadContacts, (state, action) =>
    adapter.setAll(action.contacts, state)
  ),
  on(ContactActions.clearContacts,
    (state) => adapter.removeAll(state)
  ),

  on(ContactMetaActions.setPagination, (state, action) => ({
    ...state,
    _meta: {
      ...state._meta,
      pagination: {
        ...state._meta.pagination,
        ...action
      },
    },
  })),
  on(ContactMetaActions.setFilter, (state, action) => ({
    ...state,
    _meta: {
      ...state._meta,
      filter: {
        ...state._meta.filter,
        ...action
      },
    },
  }))
);

const featureSelector = createFeatureSelector<AppState>(contactsFeatureKey);

export const contactsFeature = createFeature({
  name: contactsFeatureKey,
  reducer,
  extraSelectors: ({ selectContactsState }) => ({
    ...adapter.getSelectors(selectContactsState),

    selectContactsForTable: createSelector(featureSelector, (state) => ({
      rows: Object.values(state.entities) as ContactState[],
      pagination: state._meta.pagination,
      filter: state._meta.filter,
    })),

    selectMetaPagination: createSelector(featureSelector, (state) => ({
      ...state._meta.pagination
    })),

    selectMetaFilter: createSelector(featureSelector, (state) => ({
      ...state._meta.filter
    })),

    selectMetaPaginationOffsetData: createSelector(featureSelector, (state) => ({
      currentOffset: state._meta.pagination.currentOffset,
      totalCount: state._meta.pagination.totalCount
    })),
  })
});

export const {
  selectIds,
  selectEntities,
  selectAll,
  selectTotal,

  selectContactsForTable,
  selectMetaPagination,
  selectMetaFilter,
  selectMetaPaginationOffsetData
} = contactsFeature;
