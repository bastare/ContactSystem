import { createActionGroup, props } from '@ngrx/store';

export const ContactMetaActions = createActionGroup({
  source: 'Contact/META',
  events: {
    'Set Pagination': props<{
      currentOffset?: number;
      limit?: number;
      totalPages?: number;
      totalCount?: number;
    }>(),
    'Set Filter': props<{
      expression?: string;
      orderBy?: string;
      isDescending?: boolean;
      projection?: string;
    }>(),
  },
});
