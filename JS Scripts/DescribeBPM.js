//Takes input of a BPM string from a sm chart, outputs a "BPMEvents.json" file that contains the array of bpm change events. An important part of the conversion process
const fs = require('fs');

let string = fs.readFileSync('input', 'utf8');
console.log(string);

let array = string.split(',')
let events = []

array.forEach((item, i) => {
var time = parseInt(item.split('=')[0]);
var bpm =item.split('=')[1];
events.push({time: time, name:"BPMChange",data:bpm})
//JSON.stringify(jokelist, null, 2))
});
fs.writeFileSync("BPMEvents.json", JSON.stringify(events, null, 2))
