/* ==================== Required Functions ==================== */
// This is required to get the initial background-color of an element.
// The element might have it's bg-color already set before the transition.
// Transition should continue/start from this color.
// This will be used only once.
function getElementBG(elm) {
	var bg	= getComputedStyle(elm).backgroundColor;
		bg	= bg.match(/\((.*)\)/)[1];
		bg	= bg.split(",");
	for (var i = 0; i < bg.length; i++) {
		bg[i] = parseInt(bg[i], 10);
	}
	if (bg.length > 3) { bg.pop(); }
	return bg;
}

// A function to generate random numbers.
// Will be needed to generate random RGB value between 0-255.
function random() {
	if (arguments.length > 2) {
		return 0;
	}
	switch (arguments.length) {
		case 0:
			return Math.random();
		case 1:
			return Math.round(Math.random() * arguments[0]);
		case 2:
			var min = arguments[0];
			var max = arguments[1];
			return Math.round(Math.random() * (max - min) + min);
	}
}

// Generates a random RGB value.
function generateRGB(min, max) {
	var min		= min || 0;
	var max		= min || 255;
	var color	= [];
	for (var i = 0; i < 3; i++) {
		var num = random(min, max);
		color.push(num);
	}
	return color;
}

// Calculates the distance between the RGB values.
// We need to know the distance between two colors
// so that we can calculate the increment values for R, G, and B.
function calculateDistance(colorArray1, colorArray2) {
	var distance = [];
	for (var i = 0; i < colorArray1.length; i++) {
		distance.push(Math.abs(colorArray1[i] - colorArray2[i]));
	}
	return distance;
}

// Calculates the increment values for R, G, and B using distance, fps, and duration.
// This calculation can be made in many different ways.
function calculateIncrement(distanceArray, fps, duration) {
	var fps			= fps || 30;
	var duration	= duration || 1;
	var increment	= [];
	for (var i = 0; i < distanceArray.length; i++) {
		var incr = Math.abs(Math.floor(distanceArray[i] / (fps * duration)));
		if (incr == 0) {
			incr = 1;
		}
		increment.push(incr);
	}
	return increment;
}

// Converts RGB array [32,64,128] to HEX string #204080
// It's easier to apply HEX color than RGB color.
function rgb2hex(colorArray) {
	var color = [];
	for (var i = 0; i < colorArray.length; i++) {
		var hex = colorArray[i].toString(16);
		if (hex.length < 2) { hex = "0" + hex; }
		color.push(hex);
	}
	return "#" + color.join("");
}

/* ==================== Setup ==================== */
// Duration is not what it says. It's a multiplier in the calculateIncrement() function.
// duration = 1-4, fast-to-slow
var fps				= 30;
var duration		= 3;
var transElement	= document.body;
var currentColor	= getElementBG(transElement);
var transHandler	= null;

startTransition();

/* ==================== Transition Initiator ==================== */
function startTransition() {
	clearInterval(transHandler);
	
	targetColor	= generateRGB();
	distance	= calculateDistance(currentColor, targetColor);
	increment	= calculateIncrement(distance, fps, duration);
	
	transHandler = setInterval(function() {
		transition();
	}, 1000/fps);
}

/* ==================== Transition Calculator ==================== */
function transition() {
	// checking R
	if (currentColor[0] > targetColor[0]) {
		currentColor[0] -= increment[0];
		if (currentColor[0] <= targetColor[0]) {
			increment[0] = 0;
		}
	} else {
		currentColor[0] += increment[0];
		if (currentColor[0] >= targetColor[0]) {
			increment[0] = 0;
		}
	}
	
	// checking G
	if (currentColor[1] > targetColor[1]) {
		currentColor[1] -= increment[1];
		if (currentColor[1] <= targetColor[1]) {
			increment[1] = 0;
		}
	} else {
		currentColor[1] += increment[1];
		if (currentColor[1] >= targetColor[1]) {
			increment[1] = 0;
		}
	}
	
	// checking B
	if (currentColor[2] > targetColor[2]) {
		currentColor[2] -= increment[2];
		if (currentColor[2] <= targetColor[2]) {
			increment[2] = 0;
		}
	} else {
		currentColor[2] += increment[2];
		if (currentColor[2] >= targetColor[2]) {
			increment[2] = 0;
		}
	}
	
	// applying the new modified color
	transElement.style.backgroundColor = rgb2hex(currentColor);
	
	// transition ended. start a new one
	if (increment[0] == 0 && increment[1] == 0 && increment[2] == 0) {
		startTransition();
	}
}