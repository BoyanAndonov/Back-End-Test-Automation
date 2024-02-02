const { expect } = require('chai');
const { isOddOrEven } = require('./isOddOrEven');

describe('isOddOrEven function', () => {
  it('should return undefined for a non-string parameter', () => {
    const result = isOddOrEven(42);
    expect(result).to.be.undefined;
  });

  it('should return undefined for an array parameter', () => {
    const result = isOddOrEven(['apple', 'banana']);
    expect(result).to.be.undefined;
  });

  it('should return "even" for a string with even length', () => {
    const result = isOddOrEven('even');
    expect(result).to.equal('even');
  });

  it('should return "odd" for a string with odd length', () => {
    const result = isOddOrEven('odd');
    expect(result).to.equal('odd');
  });

  it('should handle multiple strings correctly', () => {
    const result1 = isOddOrEven('apple');
    const result2 = isOddOrEven('banana');
    const result3 = isOddOrEven('cherry');

    expect(result1).to.equal('even');
    expect(result2).to.equal('odd');
    expect(result3).to.equal('even');
  });
});
