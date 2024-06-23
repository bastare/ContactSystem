import { QueryPropsDto } from '../dtos/query-props-dto.type';

type Props = {
  search?: string;
  searchBy?: string;
  sorting?: {
    orderBy?: string;
    isDescending?: boolean;
  }
  pagination?: {
    offset?: number;
    limit?: number;
  };
};

export const getContactsForTableBySpecification: (
  props: Props
) => QueryPropsDto = ({ search, searchBy, sorting, pagination }) => ({
  expression:
    search && searchBy
      ? `
        ${searchBy}.Contains("${search}")
          && !string.IsNullOrEmpty(FirstName)
          && !string.IsNullOrEmpty(LastName)
          && !string.IsNullOrEmpty(Email)
          && !string.IsNullOrEmpty(Phone)
      `
      : `
        !string.IsNullOrEmpty(FirstName)
          && !string.IsNullOrEmpty(LastName)
          && !string.IsNullOrEmpty(Email)
          && !string.IsNullOrEmpty(Phone)
      `,

  projection: `
    new(
      Id,
      FirstName,
      LastName,
      Title,
      Email,
      Phone,
      MiddleInitial
    )
  `,

  ...sorting,
  ...pagination
});
