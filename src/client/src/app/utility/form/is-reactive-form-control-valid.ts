import { FormGroup } from '@angular/forms';

export const isReactiveFormControlValid = (
  { get }: FormGroup,
  controlName: string,
  rules: readonly string[]
) => !rules.every((rule) => get(controlName)?.getError(rule));
