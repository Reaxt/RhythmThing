using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using RhythmThing.Utils;
using System.Reflection;

namespace RhythmThing.Objects
{
    public class ChartEventHandler : GameObject
    {
        private struct easing
        {
            public string easingType;
            public float startTime;
            public float endTime; //or length?
            public float startValueX;
            public float startValueY;
            public float endValueX;
            public float endValueY;
            public int receiver;
            public easeType objectType;
        }
        private enum easeType
        {
            window,
            collumn,
            rotation
        }
        private Chart chart;
        private Receiver[] receivers;
        private WindowManager windowManager;
        private List<Chart.EventInfo> events;
        private List<easing> easings;
        private Dictionary<string, float> mods;



        public ChartEventHandler(Chart chart, Receiver[] receivers, List<Chart.EventInfo> events)
        {
            this.chart = chart;
            this.receivers = receivers;
            this.events = events;
            this.mods = new Dictionary<string, float> { };
        }
        public override void End()
        {
            //throw new NotImplementedException();
        }

        public override void Start(Game game)
        {
            type = objType.nonvisual;
            components = new List<Component>();
            windowManager = game.display.windowManager;
            easings = new List<easing>();
        }

        public override void Update(double time, Game game)
        {
            List<Chart.EventInfo> deadEvents = new List<Chart.EventInfo>();
            foreach (Chart.EventInfo eventInfo in events)
            {
                if(eventInfo.time <= chart.beat)
                {
                    float x = 0;
                    float y = 0;
                    float collumn;
                    float targetx = 0;
                    float targety = 0;
                    //run the event n shit
                            easing tempEaseing = new easing();
                    switch (eventInfo.name)
                    {
                        case "moveWindow":
                            //data="50 50"
                            x = float.Parse(eventInfo.data.Split(' ')[0]);
                            y = float.Parse(eventInfo.data.Split(' ')[1]);
                            x = (float)(x / 100) * 0.5f + 0.75f;
                            y = (y / 100) * 0.5f + 0.75f;
  

                            windowManager.CenterWindow();
                            windowManager.moveWindow(x, y);
                            break;
                        case "moveWindowEase":
                            //firstx, firsty, targetx, targety, easing, time
                            x = float.Parse(eventInfo.data.Split(' ')[0]);
                            y = float.Parse(eventInfo.data.Split(' ')[1]);
                            x = (float)(x / 100) * 0.5f + 0.75f;
                            y = (y / 100) * 0.5f + 0.75f;

                            targetx = float.Parse(eventInfo.data.Split(' ')[2]);
                            targety = float.Parse(eventInfo.data.Split(' ')[3]);
                            targetx = (float)(targetx / 100) * 0.5f + 0.75f;
                            targety = (targety / 100) * 0.5f + 0.75f;
                            //NOTE:THIS IS A MASSIVE SECURITY FLAW. IF THIS WAS NOT JUST A GAME FOR SCHOOL WHERE IM GIVING ALL THE CHARTS N STUFF I WOULD NEVER DO THIS
                            //MethodInfo method = typeof(float).GetMethod(eventInfo.data.Split(' ')[4]);
                            //I really should replace with a switch case...
                            //fuck it dictionary
                            tempEaseing.easingType = eventInfo.data.Split(' ')[4];
                            tempEaseing.startTime = eventInfo.time;
                            tempEaseing.startValueX = x;
                            tempEaseing.endValueX = targetx;
                            tempEaseing.startValueY = y;
                            tempEaseing.endValueY = targety;
                            tempEaseing.endTime = eventInfo.time + float.Parse(eventInfo.data.Split(' ')[5]);
                            tempEaseing.objectType = easeType.window;
                            easings.Add(tempEaseing);
                            break;
                        case "moveCollumn":
                            //collumn, offsetX, offsetY (both based off current loc)
                            collumn = float.Parse((eventInfo).data.Split(' ')[0]);
                            targetx = float.Parse((eventInfo).data.Split(' ')[1]);
                            targety = float.Parse((eventInfo).data.Split(' ')[2]);
                            receivers[(int)collumn].xOffset = (int)targetx;
                            receivers[(int)collumn].yOffset = (int)targety;
                            break;
                        case "moveCollumnEase":
                            //collumn, startX, startY, endX, endY, type, duration
                            
                            x = float.Parse(eventInfo.data.Split(' ')[1]);
                            y = float.Parse(eventInfo.data.Split(' ')[2]);


                            targetx = float.Parse(eventInfo.data.Split(' ')[3]);
                            targety = float.Parse(eventInfo.data.Split(' ')[4]);

                            //NOTE:THIS IS A MASSIVE SECURITY FLAW. IF THIS WAS NOT JUST A GAME FOR SCHOOL WHERE IM GIVING ALL THE CHARTS N STUFF I WOULD NEVER DO THIS
                            //MethodInfo method = typeof(float).GetMethod(eventInfo.data.Split(' ')[4]);
                            //I really should replace with a switch case...
                            //fuck it dictionary
                            tempEaseing.easingType = eventInfo.data.Split(' ')[5];
                            tempEaseing.startTime = eventInfo.time;
                            tempEaseing.startValueX = x;
                            tempEaseing.endValueX = targetx;
                            tempEaseing.startValueY = y;
                            tempEaseing.endValueY = targety;
                            tempEaseing.endTime = eventInfo.time + float.Parse(eventInfo.data.Split(' ')[6]);
                            tempEaseing.receiver = int.Parse(eventInfo.data.Split(' ')[0]);
                            tempEaseing.objectType = easeType.collumn;
                            easings.Add(tempEaseing);
                            break;
                        case "moveAllCollumnEase":
                            x = float.Parse(eventInfo.data.Split(' ')[0]);
                            y = float.Parse(eventInfo.data.Split(' ')[1]);


                            targetx = float.Parse(eventInfo.data.Split(' ')[2]);
                            targety = float.Parse(eventInfo.data.Split(' ')[3]);

                            //NOTE:THIS IS A MASSIVE SECURITY FLAW. IF THIS WAS NOT JUST A GAME FOR SCHOOL WHERE IM GIVING ALL THE CHARTS N STUFF I WOULD NEVER DO THIS
                            //MethodInfo method = typeof(float).GetMethod(eventInfo.data.Split(' ')[4]);
                            //I really should replace with a switch case...
                            //fuck it dictionary
                            for (int i = 0; i < 4; i++)
                            {
                                tempEaseing.easingType = eventInfo.data.Split(' ')[4];
                                tempEaseing.startTime = eventInfo.time;
                                tempEaseing.startValueX = x;
                                tempEaseing.endValueX = targetx;
                                tempEaseing.startValueY = y;
                                tempEaseing.endValueY = targety;
                                tempEaseing.endTime = eventInfo.time + float.Parse(eventInfo.data.Split(' ')[5]);
                                tempEaseing.receiver = i;
                                tempEaseing.objectType = easeType.collumn;
                                easings.Add(tempEaseing);

                            }
                            break;
                        case "changeCollumnDir":
                            //collumn, dir
                            receivers[int.Parse(eventInfo.data.Split(' ')[0])].direction = (Arrow.direction)int.Parse(eventInfo.data.Split(' ')[1]);
                            break;
                        case "setCollumnAngle":
                            //collumn, angle (0-360)
                            receivers[int.Parse(eventInfo.data.Split(' ')[0])].rot = float.Parse(eventInfo.data.Split(' ')[1]) / 360;
                            break;
                        case "setCollumnAngleEase":
                            //collumn, startangle, endangle, ease, time
                            tempEaseing.objectType = easeType.rotation;
                            tempEaseing.receiver = int.Parse(eventInfo.data.Split(' ')[0]);
                            tempEaseing.startValueX = float.Parse(eventInfo.data.Split(' ')[1]);
                            tempEaseing.endValueX = float.Parse(eventInfo.data.Split(' ')[2]);
                            tempEaseing.easingType = eventInfo.data.Split(' ')[3];
                            tempEaseing.endTime = float.Parse(eventInfo.data.Split(' ')[4]);
                            tempEaseing.startTime = eventInfo.time;
                            easings.Add(tempEaseing);
                            break;
                        case "setAllCollumnAngle":
                            //angle
                            for (int i = 0; i < 4; i++)
                            {
                                receivers[i].rot = float.Parse(eventInfo.data);
                            }
                            break;
                        case "setAllCollumnAngleEase":
                            //startangle, endangle, ease, time
                            for (int i = 0; i < 4; i++)
                            {
                                tempEaseing.objectType = easeType.rotation;
                                tempEaseing.receiver = i;
                                tempEaseing.startValueX = float.Parse(eventInfo.data.Split(' ')[0]);
                                tempEaseing.endValueX = float.Parse(eventInfo.data.Split(' ')[1]);
                                tempEaseing.easingType = eventInfo.data.Split(' ')[2];
                                tempEaseing.endTime = float.Parse(eventInfo.data.Split(' ')[3]);
                                tempEaseing.startTime = eventInfo.time;
                                easings.Add(tempEaseing);
                            }
                            break;
                        case "relativeAllCollumnEase":
                            //xoffset, yoffset, ease, time
                            //x = float.Parse(eventInfo.data.Split(' ')[0]);
                            //y = float.Parse(eventInfo.data.Split(' ')[1]);


                            targetx = float.Parse(eventInfo.data.Split(' ')[0]);
                            targety = float.Parse(eventInfo.data.Split(' ')[1]);

                            //NOTE:THIS IS A MASSIVE SECURITY FLAW. IF THIS WAS NOT JUST A GAME FOR SCHOOL WHERE IM GIVING ALL THE CHARTS N STUFF I WOULD NEVER DO THIS
                            //MethodInfo method = typeof(float).GetMethod(eventInfo.data.Split(' ')[4]);
                            //I really should replace with a switch case...
                            //fuck it dictionary
                            for (int i = 0; i < 4; i++)
                            {
                                tempEaseing.easingType = eventInfo.data.Split(' ')[2];
                                tempEaseing.startTime = eventInfo.time;
                                tempEaseing.startValueX = receivers[i].xOffset;
                                tempEaseing.endValueX = targetx;
                                tempEaseing.startValueY = receivers[i].yOffset;
                                tempEaseing.endValueY = targety;
                                tempEaseing.endTime = eventInfo.time + float.Parse(eventInfo.data.Split(' ')[3]);
                                tempEaseing.receiver = i;
                                tempEaseing.objectType = easeType.collumn;
                                easings.Add(tempEaseing);

                            }
                            break;
                        case "realtiveCollumnEase":
                            //col, targetx, targety, ease, time
                            //x = float.Parse(eventInfo.data.Split(' ')[1]);
                            //y = float.Parse(eventInfo.data.Split(' ')[2]);


                            targetx = float.Parse(eventInfo.data.Split(' ')[1]);
                            targety = float.Parse(eventInfo.data.Split(' ')[2]);

                            //NOTE:THIS IS A MASSIVE SECURITY FLAW. IF THIS WAS NOT JUST A GAME FOR SCHOOL WHERE IM GIVING ALL THE CHARTS N STUFF I WOULD NEVER DO THIS
                            //MethodInfo method = typeof(float).GetMethod(eventInfo.data.Split(' ')[4]);
                            //I really should replace with a switch case...
                            //fuck it dictionary
                            tempEaseing.easingType = eventInfo.data.Split(' ')[3];
                            tempEaseing.startTime = eventInfo.time;
                            tempEaseing.startValueX = receivers[int.Parse(eventInfo.data.Split(' ')[0])].xOffset;
                            tempEaseing.endValueX = targetx;
                            tempEaseing.startValueY = receivers[int.Parse(eventInfo.data.Split(' ')[0])].yOffset;
                            tempEaseing.endValueY = targety;
                            tempEaseing.endTime = eventInfo.time + float.Parse(eventInfo.data.Split(' ')[4]);
                            tempEaseing.receiver = int.Parse(eventInfo.data.Split(' ')[0]);
                            tempEaseing.objectType = easeType.collumn;
                            easings.Add(tempEaseing);
                            break;
                        case "relativeCollumnAngleEase":
                            //collumn, targetX, ease, time
                            tempEaseing.objectType = easeType.rotation;
                            tempEaseing.receiver = int.Parse(eventInfo.data.Split(' ')[0]);
                            tempEaseing.startValueX = receivers[int.Parse(eventInfo.data.Split(' ')[0])].rot;
                            tempEaseing.endValueX = float.Parse(eventInfo.data.Split(' ')[1]);
                            tempEaseing.easingType = eventInfo.data.Split(' ')[2];
                            tempEaseing.endTime = float.Parse(eventInfo.data.Split(' ')[3]);
                            tempEaseing.startTime = eventInfo.time;
                            easings.Add(tempEaseing);
                            break;
                        case "relativeAllAngleEase":
                            // endangle, ease, time
                            for (int i = 0; i < 4; i++)
                            {
                                tempEaseing.objectType = easeType.rotation;
                                tempEaseing.receiver = i;
                                tempEaseing.startValueX = receivers[i].rot;
                                tempEaseing.endValueX = float.Parse(eventInfo.data.Split(' ')[0]);
                                tempEaseing.easingType = eventInfo.data.Split(' ')[1];
                                tempEaseing.endTime = float.Parse(eventInfo.data.Split(' ')[2]);
                                tempEaseing.startTime = eventInfo.time;
                                easings.Add(tempEaseing);
                            }
                            break;
                        case "setMovementAmount":
                            Arrow.movementAmount = float.Parse(eventInfo.data.Split(' ')[0]);
                            break;
                        default:
                            break;
                    }
                    deadEvents.Add(eventInfo);
                }
            }
            foreach (var item in deadEvents)
            {
                events.Remove(item);
            }
            List<easing> toDie = new List<easing>();
            //run through easings
            foreach (var item in easings)
            {
                if(item.objectType == easeType.window)
                {
                    if(chart.beat >= item.endTime)
                    {
                        windowManager.moveWindow(item.endValueX, item.endValueY);
                        //float hm = Ease.byName[item.easingType](chart.beat - item.startTime, item.endTime - item.startTime, item.startValueX, item.endValueX)/1.6f;
                        toDie.Add(item);
                    } else
                    {
                        //ease basically takes a percent
                        float temptest = (chart.beat - item.startTime) / (item.endTime - item.startTime);
                        windowManager.moveWindow(Ease.Lerp(item.startValueX, item.endValueX, Ease.byName[item.easingType]((chart.beat- item.startTime) /(item.endTime - item.startTime))), Ease.Lerp(item.startValueY, item.endValueY, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime))));
                    }

                } else if(item.objectType == easeType.collumn)
                {
                    if (chart.beat >= item.endTime)
                    {
                        
                        //float hm = Ease.byName[item.easingType](chart.beat - item.startTime, item.endTime - item.startTime, item.startValueX, item.endValueX)/1.6f;
                        toDie.Add(item);
                    }
                    else
                    {
                        //ease basically takes a percent
                        receivers[item.receiver].xOffset = (int)Ease.Lerp(item.startValueX, item.endValueX, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime)));
                        receivers[item.receiver].yOffset = (int)Ease.Lerp(item.startValueY, item.endValueY, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime)));

                        //windowManager.moveWindow(Ease.Lerp(item.startValueX, item.endValueX, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime))), Ease.Lerp(item.startValueY, item.endValueY, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime))));
                    }
                } else if(item.objectType == easeType.rotation)
                {
                    if (chart.beat >= item.endTime)
                    {

                        //float hm = Ease.byName[item.easingType](chart.beat - item.startTime, item.endTime - item.startTime, item.startValueX, item.endValueX)/1.6f;
                        toDie.Add(item);
                    }
                    else
                    {
                        //ease basically takes a percent
                        receivers[item.receiver].rot = (int)Ease.Lerp(item.startValueX, item.endValueX, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime)));
                        
                        //windowManager.moveWindow(Ease.Lerp(item.startValueX, item.endValueX, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime))), Ease.Lerp(item.startValueY, item.endValueY, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime))));
                    }
                }
            }
            foreach (var item in toDie)
            {
                easings.Remove(item);
            }




        }
    }
}
