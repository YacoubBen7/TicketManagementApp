import { FormControl } from '@angular/forms';
import { statusValidator } from './status_validator'; 

describe('statusValidator', () => {
  const validStatuses = ['Open', 'Closed'];

  it('should return null when the status is valid', () => {
    const control = new FormControl('Open');
    const validator = statusValidator(validStatuses);
    const result = validator(control);
    expect(result).toBeNull();
  });

  it('should return null when the status is an empty string and allowed in the options', () => {
    const control = new FormControl('');
    const validator = statusValidator([...validStatuses, '']);
    const result = validator(control);
    expect(result).toBeNull();
  });

  it('should handle a null value as invalid when not in the options', () => {
    const control = new FormControl(null);
    const validator = statusValidator(validStatuses);
    const result = validator(control);
    expect(result).toEqual({ invalidStatus: { value: null } });
  });

  it('should return a validation error when the status is an empty string but not allowed in the options', () => {
    const control = new FormControl('');
    const validator = statusValidator(validStatuses);
    const result = validator(control);
    expect(result).toEqual({ invalidStatus: { value: '' } });
  });
});
