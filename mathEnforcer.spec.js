const { expect } = require('chai');
const { mathEnforcer } = require('./mathEnforcer');

describe('mathEnforcer object', () => {
  describe('addFive function', () => {
    it('should return undefined for a non-number parameter', () => {
      const result = mathEnforcer.addFive('not a number');
      expect(result).to.be.undefined;
    });

    it('should correctly add 5 to a positive integer', () => {
      const result = mathEnforcer.addFive(10);
      expect(result).to.equal(15);
    });

    it('should correctly add 5 to a negative integer', () => {
      const result = mathEnforcer.addFive(-5);
      expect(result).to.equal(0);
    });

    it('should correctly add 5 to a floating-point number within 0.01 tolerance', () => {
      const result = mathEnforcer.addFive(3.999);
      expect(result).to.be.closeTo(8.999, 0.01);
    });
  });

  describe('subtractTen function', () => {
    it('should return undefined for a non-number parameter', () => {
      const result = mathEnforcer.subtractTen('not a number');
      expect(result).to.be.undefined;
    });

    it('should correctly subtract 10 from a positive integer', () => {
      const result = mathEnforcer.subtractTen(20);
      expect(result).to.equal(10);
    });

    it('should correctly subtract 10 from a negative integer', () => {
      const result = mathEnforcer.subtractTen(-8);
      expect(result).to.equal(-18);
    });

    it('should correctly subtract 10 from a floating-point number within 0.01 tolerance', () => {
      const result = mathEnforcer.subtractTen(15.999);
      expect(result).to.be.closeTo(5.999, 0.01);
    });
  });

  describe('sum function', () => {
    it('should return undefined for non-number first parameter', () => {
      const result = mathEnforcer.sum('not a number', 5);
      expect(result).to.be.undefined;
    });

    it('should return undefined for non-number second parameter', () => {
      const result = mathEnforcer.sum(10, 'not a number');
      expect(result).to.be.undefined;
    });

    it('should correctly sum two positive integers', () => {
      const result = mathEnforcer.sum(5, 10);
      expect(result).to.equal(15);
    });

    it('should correctly sum two negative integers', () => {
      const result = mathEnforcer.sum(-5, -10);
      expect(result).to.equal(-15);
    });

    it('should correctly sum two floating-point numbers within 0.01 tolerance', () => {
      const result = mathEnforcer.sum(1.234, 2.345);
      expect(result).to.be.closeTo(3.579, 0.01);
    });
  });
});
