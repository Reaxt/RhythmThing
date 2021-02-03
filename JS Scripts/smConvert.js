console.log(process.argv);
const SMParse = require("./parser.js");
const fs = require("fs");
const { notes } = require("./parser.js");
let ChartInfo = {};
const readline = require("readline").createInterface({
  input: process.stdin,
  output: process.stdout
});
let input = fs.readFileSync(process.argv[2]).toString();
//console.log(input)

let chart = SMParse(input);
ChartInfo.songPath = "fix me!"
ChartInfo.songPath = chart.files.music;
ChartInfo.songName = chart.meta.title;
ChartInfo.songAuthor = chart.meta.artist;
ChartInfo.chartAuthor = "";
ChartInfo.offset = parseFloat(chart.times.offset);
ChartInfo.bpm = 0;
ChartInfo.preview = parseFloat(chart.times.sample.start);
ChartInfo.previewLength = parseFloat(chart.times.sample.length);
ChartInfo.events = [];
ChartInfo.notes = [];
//gen bpm chanes
let bpmList = chart.raw.BPMs.split(",");
ChartInfo.bpm = parseFloat(bpmList[0].split("=")[1]);
if (bpmList.length > 1) {
  for (let i = 0; i < bpmList.length; i++) {
    let event = {
      time: parseFloat(bpmList[i].split("=")[0]),
      type: "BPMChange",
      data: bpmList[i].split("=")[1]
    };
    ChartInfo.events.push(event);
  }
}

singles = chart.charts.filter(o => o.type == "dance-single");
console.log("=== ENTER THE DIFF YOU WANT TO CONVERT ===");
for (let i = 0; i < singles.length; i++) {
  console.log(`${i}: ${singles[i].name}, diff: ${singles[i].diff}`);
}

readline.question("Please select a number:", selectedChart => {
  readline.close();

  selectedChart = chart.charts[parseInt(selectedChart)];
  ChartInfo.chartAuthor = selectedChart.credit;
  //yes this is a lazy way. it works. brain doesnt wanna do math rn
  let beatCount = 0;
  for (let i = 0; i < selectedChart.notes.length; i++) {
    let currentMeasure = selectedChart.notes[i][1];
    console.log(currentMeasure);
    let currentDivision = 4 / currentMeasure.length;

    for (let x = 0; x < currentMeasure.length; x++) {
      for (let c = 0; c < 4; c++) {
        if (currentMeasure[x][c] == "1" || currentMeasure[x][c] == "2") {
          console.log(x / currentDivision);
          let time = beatCount + x * currentDivision;
          ChartInfo.notes.push({
            time: time,
            collumn: c
          });
        }
      }
    }
    beatCount += 4;
  }
  fs.writeFileSync("./ChartInfo.json", JSON.stringify(ChartInfo, NaN, 2));
  console.log(`Converted ${ChartInfo.songAuthor}-${ChartInfo.songName}, charted by ${ChartInfo.chartAuthor} into RhythmThing format! Dont forget to set up your audio file!`);
});
