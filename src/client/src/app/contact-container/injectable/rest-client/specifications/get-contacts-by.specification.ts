import { QueryPropsDto } from '../dtos/query-props-dto.type';

type Props = {
  search?: string;
  searchBy?: string;
  pagination?: {
    offset?: number;
    limit?: number;
  };
};

export const getContactsForTableBySpecification: (
  props: Props
) => QueryPropsDto = ({ search, searchBy, pagination }) => ({
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

  ...pagination,
});
