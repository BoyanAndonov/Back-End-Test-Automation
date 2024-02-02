function sumOfEvenAndOddDigits(number) {
    let oddSum = 0;
    let evenSum = 0;

    // Convert the number to a string to iterate through each digit
    const numberString = number.toString();

    // Iterate through each character (digit) in the string
    for (let i = 0; i < numberString.length; i++) {
        const digit = parseInt(numberString[i]);

        // Check if the digit is even or odd
        if (digit % 2 === 0) {
            evenSum += digit;
        } else {
            oddSum += digit;
        }
    }

    return `Odd sum = ${oddSum}, Even sum = ${evenSum}`;
}

// Examples
console.log(sumOfEvenAndOddDigits(1000435));