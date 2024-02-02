function sortAlternate(arr) {
    // Сортирайте масива във възходящ ред
    arr.sort((a, b) => a - b);

    // Инициализирайте празен масив за резултат
    let result = [];

    // Итерирайте през сортирания масив и пренареждайте елементите
    while (arr.length > 0) {
        result.push(arr.shift());
        if (arr.length > 0) {
            result.push(arr.pop());
        }
    }

    return result;
}

// Пример за използване

console.log(sortAlternate([1, 65, 3, 52, 48, 63, 31, -3, 18, 56]));
