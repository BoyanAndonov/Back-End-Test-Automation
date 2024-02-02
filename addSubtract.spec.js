const { expect } = require('chai');
const { createCalculator } = require('./addSubtract');

describe('createCalculator function', () => {
  it('should return an object with add(), subtract(), and get() functions', () => {
    const calculator = createCalculator();
    expect(calculator).to.be.an('object');
    expect(calculator).to.have.property('add').to.be.a('function');
    expect(calculator).to.have.property('subtract').to.be.a('function');
    expect(calculator).to.have.property('get').to.be.a('function');
  });

  it('should correctly add numbers using the add() function', () => {
    const calculator = createCalculator();
    calculator.add(5);
    calculator.add('10');
    const result = calculator.get();
    expect(result).to.equal(15);
  });

  it('should correctly subtract numbers using the subtract() function', () => {
    const calculator = createCalculator();
    calculator.subtract(5);
    calculator.subtract('10');
    const result = calculator.get();
    expect(result).to.equal(-15);
  });

  it('should handle negative numbers correctly', () => {
    const calculator = createCalculator();
    calculator.add(-5);
    calculator.subtract('-10');
    const result = calculator.get();
    expect(result).to.equal(-15);
  });

  it('should handle non-numeric inputs gracefully', () => {
    const calculator = createCalculator();
    calculator.add('abc');
    calculator.subtract('def');
    const result = calculator.get();
    expect(result).to.be.NaN;
  });

  it('should not allow direct modification of the internal sum', () => {
    const calculator = createCalculator();
    calculator.add(5);
    calculator.subtract(2);
    calculator.value = 100; // Try to modify the internal sum directly
    const result = calculator.get();
    expect(result).to.equal(3);
  });
});
