function validatePassword(password) {
    let isValid = true;
    let messages = [];

    // Check the length of the password
    if (password.length < 6 || password.length > 10) {
        isValid = false;
        messages.push("Password must be between 6 and 10 characters");
    }

    // Check if the password consists only of letters and digits
    if (!/^[a-zA-Z0-9]+$/.test(password)) {
        isValid = false;
        messages.push("Password must consist only of letters and digits");
    }

    // Check if the password has at least 2 digits
    const digitCount = password.replace(/[^0-9]/g, "").length;
    if (digitCount < 2) {
        isValid = false;
        messages.push("Password must have at least 2 digits");
    }

    // Print the result
    if (isValid) {
        console.log("Password is valid");
    } else {
        for (const message of messages) {
            console.log(message);
        }
    }
}

// Examples
validatePassword('MyPass123');

