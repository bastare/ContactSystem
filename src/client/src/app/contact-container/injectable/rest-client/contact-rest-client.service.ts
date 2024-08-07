import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { PaginationListDto } from './dtos/pagination-dto-list.type';
import { ContactForTableDto } from './dtos/contact-for-table-dto.type';
import { ContactState } from '../../store-features/contact-state.model';
import { environment } from '../../../../environments/environment';
import { QueryPropsDto } from './dtos/query-props-dto.type';

@Injectable({
  providedIn: 'root',
})
export class ContactRestClient {
  private readonly http = inject(HttpClient);

  fetchContacts$(queryProps: QueryPropsDto) {
    return this.http.get<PaginationListDto<ContactForTableDto>>(
      `${environment.restApiV1}/contacts`,
      {
        params: new HttpParams()
          .appendAll(queryProps),
      }
    );
  }

  addContact$({ contactForAdd }: { contactForAdd: ContactState }) {
    return this.http.post<ContactForTableDto>(
      `${environment.restApiV1}/contacts`,
      contactForAdd
    );
  }

  removeContact$({ contactId }: { contactId: number }) {
    return this.http.delete(
      `${environment.restApiV1}/contacts/${contactId}`
    );
  }

  patchContact$({ contactId, contactForPatch }: { contactId: number, contactForPatch: ContactState }) {
    return this.http.patch<ContactForTableDto>(
      `${environment.restApiV1}/contacts/${contactId}`,
      contactForPatch
    );
  }
}
