import { FormGroup } from '@angular/forms';

export const isReactiveFormValid = (
  form: FormGroup,
  controlName: string,
  rules: readonly string[]
) => rules.every((rule) => !form.get(controlName)?.getError(rule));
