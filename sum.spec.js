import{ sum } from './sum.js' 
import { expect } from 'chai';

describe('sum function', () => {
    it('should return 0 for an empty array', () => {
      const result = sum([]);
      expect(result).to.equal(0);
    });
  
    it('should return the correct sum for an array with positive numbers', () => {
      const result = sum([1, 2, 3, 4, 5]);
      expect(result).to.equal(15);
    });
  
    it('should return the correct sum for an array with negative numbers', () => {
      const result = sum([-1, -2, -3, -4, -5]);
      expect(result).to.equal(-15);
    });
  
    it('should handle arrays with a mix of positive and negative numbers', () => {
      const result = sum([-1, 2, -3, 4, -5]);
      expect(result).to.equal(-3);
    });
  
    it('should handle arrays with floating-point numbers', () => {
      const result = sum([1.5, 2.5, 3.5]);
      expect(result).to.equal(7.5);
    });
  
    it('should ignore non-numeric elements in the array', () => {
      const result = sum([1, 2, '3', 'abc', true]);
      expect(result).to.equal(3);
    });
  });