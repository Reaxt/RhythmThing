const fs = require("fs");

const INPUT = "ChartInfo1.json";
const OUTPUT = "ChartInfo.json";

let difficulty = JSON.parse(fs.readFileSync(INPUT));

difficulty.notes.forEach(note => {
  if (note.time > 84 && note.time < 100) {
    difficulty.events.push(
      {
        time: note.time - 0.05,
        name: "freezeAllCollumnArrows",
        data: 0
      },
      {
        time: note.time,
        name: "freezeAllCollumnArrows",
        data: 1
      }
    );
  }
});
difficulty.events.push({
  time: 84,
  name: "setModPercent",
  data: "bumpy 7"
});
difficulty.events.push({
  time: 91.5,
  name: "freezeAllCollumnArrows",
  data: 0
});

{
  let tog1 = false;
  let tog2 = false;
  let lastTime = 0;
  difficulty.notes
    .filter(x => x.time >= 200 && x.time <= 240)
    .forEach(note => {

      if (difficulty.notes.filter(x => x.time == note.time).length > 1) {
        //swap that shit homie
        if (lastTime != note.time) {
          lastTime = note.time;

          if (tog1) {
            difficulty.events.push(
              {
                time: note.time,
                name: "moveCollumnEase",
                data: "3 -43 0 0 0 easeOutExpo 0.25"
              },
              {
                time: note.time,
                name: "moveCollumnEase",
                data: "0 43 0 -0 0 easeOutExpo 0.25"
              }
            );
          } else {
            difficulty.events.push(
              {
                time: note.time,
                name: "moveCollumnEase",
                data: "3 0 0 -43 0 easeOutExpo 0.25"
              },
              {
                time: note.time,
                name: "moveCollumnEase",
                data: "0 0 0 43 0 easeOutExpo 0.25"
              }
            );
          }
          tog1 = !tog1;
        }

      }
    });
    difficulty.notes.filter(x => x.time >= 259 && x.time < 260).forEach(note => {
        console.log(note.time)
        difficulty.events.push(
            {
              time: note.time - 0.05,
              name: "freezeAllCollumnArrows",
              data: 0
            },
            {
              time: note.time,
              name: "freezeAllCollumnArrows",
              data: 1
            }
          );
    })
    difficulty.events.push({
        time: 260,
        name: "freezeAllCollumnArrows",
        data: 0
      });
}
fs.writeFileSync(OUTPUT, JSON.stringify(difficulty, null, 2));
