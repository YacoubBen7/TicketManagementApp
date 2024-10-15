import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function statusValidator(statusOptions: string[]): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    return statusOptions.includes(value) ? null : { invalidStatus: { value } };
  };
}
