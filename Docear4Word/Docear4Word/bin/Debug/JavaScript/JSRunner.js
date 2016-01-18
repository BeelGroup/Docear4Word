function createJSArray() {
	var args = [];

	for (var i = 0; i < arguments.length; i++) {
		args.push(arguments[i]);
	}

	return args;
}

createEmptyJSArray = function() {
	return new Array();
};

createJSObjectFromJSON = function(jsonString) {
	return JSON.parse(jsonString);
};

createJSONFromJSObject = function(jsObject, space) {
		return JSON.stringify(jsObject, null, space);
};

createJSObject = function() {
	return {};
};

var dumpJSObject = function (obj, name, indent, depth) {
	//alert("dump");
	if (depth > 10) {
		return indent + name + ": <Maximum Depth Reached>\n";
	}

	if (typeof obj != "object") return obj;
	var child;
	var output = indent + name + "\n";
	indent += "\t";

	for (var item in obj) {
		try {
			child = obj[item];
		} catch (e) {
			child = "<Unable to Evaluate>";
		}

		if (typeof child == "object") {
			output += this.dumpObj(child, item, indent, depth + 1);
		} else {
			output += indent + item + ": " + child + "\n";
		}
	}

	return output;
};

