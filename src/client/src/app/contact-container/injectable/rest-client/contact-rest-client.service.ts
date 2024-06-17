import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { PaginationListDto } from './dtos/pagination-dto-list.type';
import { ContactForTableDto } from './dtos/contact-for-table-dto.type';
import { ContactState } from '../../store-features/contact-state.model';

@Injectable({
  providedIn: 'root',
})
export class ContactRestClient {
  private readonly http = inject(HttpClient);

  fetchContacts$(queryProps: {
    expression?: string;
    orderBy?: string;
    isDescending?: boolean;
    offset?: number;
    limit?: number;
    projection?: string;
  }) {
    return this.http.get<PaginationListDto<ContactForTableDto>>(
      'http://localhost:5000/api/v1/contacts',
      {
        params: new HttpParams().appendAll({ ...queryProps }),
      }
    );
  }

  addContact$({ contactForAdd }: { contactForAdd: ContactState }) {
    return this.http.post<ContactForTableDto>(
      'http://localhost:5000/api/v1/contacts',
      contactForAdd
    );
  }

  removeContact$({ contactId }: { contactId: number }) {
    return this.http.delete<ContactForTableDto>(
      `http://localhost:5000/api/v1/contacts/${contactId}`
    );
  }

  patchContact({ contactForPatch }: { contactForPatch: ContactState }) {
    return this.http.patch<ContactForTableDto>(
      'http://localhost:5000/api/v1/contacts',
      contactForPatch
    );
  }
}
