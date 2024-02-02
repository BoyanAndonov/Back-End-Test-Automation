const { expect } = require('chai');
const { isSymmetric } = require('./checkForSymmetry');

describe('isSymmetric function', () => {
  it('should return true for an empty array', () => {
    const result = isSymmetric([]);
    expect(result).to.be.true;
  });

  it('should return true for a symmetric array with numbers', () => {
    const result = isSymmetric([1, 2, 3, 3, 2, 1]);
    expect(result).to.be.true;
  });

  it('should return true for a symmetric array with strings', () => {
    const result = isSymmetric(['a', 'b', 'c', 'c', 'b', 'a']);
    expect(result).to.be.true;
  });

  it('should return true for an array with a single element', () => {
    const result = isSymmetric([1]);
    expect(result).to.be.true;
  });

  it('should return false for an array that is not symmetric', () => {
    const result = isSymmetric([1, 2, 3, 4, 5]);
    expect(result).to.be.false;
  });

  it('should return false for an array with different data types', () => {
    const result = isSymmetric([1, '2', 2, '1']);
    expect(result).to.be.false;
  });

  it('should return false for non-array input', () => {
    const result = isSymmetric('not an array');
    expect(result).to.be.false;
  });

  it('should return false for null input', () => {
    const result = isSymmetric(null);
    expect(result).to.be.false;
  });

  it('should return false for undefined input', () => {
    const result = isSymmetric(undefined);
    expect(result).to.be.false;
  });
});
