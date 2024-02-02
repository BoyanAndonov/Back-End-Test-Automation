function rotateArray(arr, rotations) {
    const length = arr.length;

    // Ensure the number of rotations is within the length of the array
    rotations = rotations % length;

    // Perform the rotations
    const rotatedArray = arr.slice(rotations).concat(arr.slice(0, rotations));

    // Print the resulting array elements separated by a single space
    console.log(rotatedArray.join(' '));
}
rotateArray([51, 47, 32, 61, 21], 2)