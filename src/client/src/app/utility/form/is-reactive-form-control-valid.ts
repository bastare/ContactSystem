import { FormGroup } from '@angular/forms';

export const isReactiveFormControlValid = (
  form: FormGroup,
  controlName: string,
  rules: readonly string[]
) => rules.every((rule) => !form.get(controlName)?.getError(rule));
