const { expect } = require('chai');
const { rgbToHexColor } = require('./rgb-to-hex');

describe('rgbToHexColor function', () => {
  it('should return correct hex color for valid RGB values', () => {
    const result = rgbToHexColor(255, 99, 71);
    expect(result).to.equal('#FF6347');
  });

  it('should return correct hex color for minimum valid RGB values', () => {
    const result = rgbToHexColor(0, 0, 0);
    expect(result).to.equal('#000000');
  });

  it('should return correct hex color for maximum valid RGB values', () => {
    const result = rgbToHexColor(255, 255, 255);
    expect(result).to.equal('#FFFFFF');
  });

  it('should return undefined for non-integer red value', () => {
    const result = rgbToHexColor('invalid', 0, 0);
    expect(result).to.be.undefined;
  });

  it('should return undefined for non-integer green value', () => {
    const result = rgbToHexColor(0, 'invalid', 0);
    expect(result).to.be.undefined;
  });

  it('should return undefined for non-integer blue value', () => {
    const result = rgbToHexColor(0, 0, 'invalid');
    expect(result).to.be.undefined;
  });

  it('should return undefined for red value below the valid range', () => {
    const result = rgbToHexColor(-1, 0, 0);
    expect(result).to.be.undefined;
  });

  it('should return undefined for green value below the valid range', () => {
    const result = rgbToHexColor(0, -1, 0);
    expect(result).to.be.undefined;
  });

  it('should return undefined for blue value below the valid range', () => {
    const result = rgbToHexColor(0, 0, -1);
    expect(result).to.be.undefined;
  });

  it('should return undefined for red value above the valid range', () => {
    const result = rgbToHexColor(256, 0, 0);
    expect(result).to.be.undefined;
  });

  it('should return undefined for green value above the valid range', () => {
    const result = rgbToHexColor(0, 256, 0);
    expect(result).to.be.undefined;
  });

  it('should return undefined for blue value above the valid range', () => {
    const result = rgbToHexColor(0, 0, 256);
    expect(result).to.be.undefined;
  });
});
