import { DateFormatPipe } from './date-format.pipe'; 

describe('DateFormatPipe', () => {
  let pipe: DateFormatPipe;

  beforeEach(() => {
    pipe = new DateFormatPipe();
  });

  it('should create an instance', () => {
    expect(pipe).toBeTruthy();
  });

  it('should transform "2024-10-15" to "October-15-2024"', () => {
    const result = pipe.transform('2024-10-15');
    expect(result).toBe('October-15-2024');
  });

  it('should transform "2000-01-01" to "January-1-2000"', () => {
    const result = pipe.transform('2000-01-01');
    expect(result).toBe('January-1-2000');
  });


});
