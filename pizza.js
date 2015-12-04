var fs = require('fs');
var pizzas = JSON.parse(fs.readFileSync('pizzas.json', 'utf-8'));

var map = {};

for (var index = 0; index < pizzas.length; index++) {
	var order = pizzas[index];
	var toppings = order.toppings;

	toppings.sort(function (a, b) {
		return a.localeCompare(b);
	});

	var hash = JSON.stringify(toppings);
	if (hash in map) { 
		map[hash] += 1;
	} else {
		map[hash] = 0;
	}
}

var orders = Object.keys(map).map(function (key) {
	return [key, map[key]];
});

orders.sort(function (a, b) {
	return b[1] - a[1];
});

console.log(orders.slice(0, 20));
