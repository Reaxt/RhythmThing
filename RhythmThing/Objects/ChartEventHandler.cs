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
            public string modName;
        }
        private enum easeType
        {
            window,
            collumn,
            rotation,
            mod,
            movementAmount,
            screenX
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
                            //collumn, offsetX, offsetY (both based off ~~current loc~~ starting loc)
                            collumn = float.Parse((eventInfo).data.Split(' ')[0]);
                            targetx = float.Parse((eventInfo).data.Split(' ')[1]);
                            targety = float.Parse((eventInfo).data.Split(' ')[2]);
                            receivers[(int)collumn].xOffset = (int)targetx;
                            receivers[(int)collumn].yOffset = (int)targety;
                            break;
                        case "moveAllCollumn":
                            //offsetX, offsetY
                            targetx = float.Parse((eventInfo.data.Split(' ')[0]));
                            targety = float.Parse((eventInfo).data.Split(' ')[1]);
                            for (int i = 0; i < 4; i++)
                            {
                                receivers[i].xOffset = (int)targetx;
                                receivers[i].yOffset = (int)targety;
                            }
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
                            //x y targetx targety ease time
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
                            tempEaseing.startValueX = float.Parse(eventInfo.data.Split(' ')[1]) / 360;
                            tempEaseing.endValueX = float.Parse(eventInfo.data.Split(' ')[2]) / 360; ;
                            tempEaseing.easingType = eventInfo.data.Split(' ')[3];
                            tempEaseing.endTime = float.Parse(eventInfo.data.Split(' ')[4]) + eventInfo.time ;
                            tempEaseing.startTime = eventInfo.time;
                            easings.Add(tempEaseing);
                            break;
                        case "setAllCollumnAngle":
                            //angle
                            for (int i = 0; i < 4; i++)
                            {
                                receivers[i].rot = float.Parse(eventInfo.data)/360;
                            }
                            break;
                        case "setAllCollumnAngleEase":
                            //startangle, endangle, ease, time
                            for (int i = 0; i < 4; i++)
                            {
                                tempEaseing.objectType = easeType.rotation;
                                tempEaseing.receiver = i;
                                tempEaseing.startValueX = float.Parse(eventInfo.data.Split(' ')[0]) / 360;
                                tempEaseing.endValueX = float.Parse(eventInfo.data.Split(' ')[1]) / 360;
                                tempEaseing.easingType = eventInfo.data.Split(' ')[2];
                                tempEaseing.endTime = float.Parse(eventInfo.data.Split(' ')[3]) + eventInfo.time;
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
                                tempEaseing.endValueX = targetx + receivers[i].xOffset;
                                tempEaseing.startValueY = receivers[i].yOffset;
                                tempEaseing.endValueY = targety + receivers[i].yOffset;
                                tempEaseing.endTime = eventInfo.time + float.Parse(eventInfo.data.Split(' ')[3]);
                                tempEaseing.receiver = i;
                                tempEaseing.objectType = easeType.collumn;
                                easings.Add(tempEaseing);

                            }
                            break;
                        case "relativeAllCollumnMove":
                            targetx = float.Parse(eventInfo.data.Split(' ')[0]);
                            targety = float.Parse(eventInfo.data.Split(' ')[1]);
                            for (int i = 0; i < 4; i++)
                            {
                                receivers[i].xOffset = (int)Math.Round(targetx + receivers[i].xOffset);
                                receivers[i].yOffset = (int)Math.Round(targety + receivers[i].yOffset);


                            }
                            break;
                        case "freezeCollumnArrows":
                            //collumn, 0/1
                            //0= unfrozen
                            //1 = frozen
                            int col = int.Parse(eventInfo.data.Split(' ')[0]);
                            int frozena = int.Parse(eventInfo.data.Split(' ')[1]);
                            if(frozena == 0)
                            {
                                receivers[col].frozen = false;
                            } else
                            {
                                receivers[col].frozen = true;
                            }
                            break;
                        case "freezeAllCollumnArrows":
                            int frozenb = int.Parse(eventInfo.data);
                            for (int i = 0; i < 4; i++)
                            {
                                if(frozenb == 0)
                                {
                                    receivers[i].frozen = false;
                                } else
                                {
                                    receivers[i].frozen = true;
                                }
                            }
                            break;
                        case "relativeCollumnEase":
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
                            tempEaseing.endValueX = targetx + receivers[int.Parse(eventInfo.data.Split(' ')[0])].xOffset;
                            tempEaseing.startValueY = receivers[int.Parse(eventInfo.data.Split(' ')[0])].yOffset;
                            tempEaseing.endValueY = targety + receivers[int.Parse(eventInfo.data.Split(' ')[0])].yOffset;
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
                            tempEaseing.endValueX = receivers[int.Parse(eventInfo.data.Split(' ')[0])].rot+(float.Parse(eventInfo.data.Split(' ')[1]) / 360);
                            tempEaseing.easingType = eventInfo.data.Split(' ')[2];
                            tempEaseing.endTime = float.Parse(eventInfo.data.Split(' ')[3]) + eventInfo.time;
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
                                tempEaseing.endValueX = (float.Parse(eventInfo.data.Split(' ')[0]) / 360) + receivers[i].rot;
                                tempEaseing.easingType = eventInfo.data.Split(' ')[1];
                                tempEaseing.endTime = float.Parse(eventInfo.data.Split(' ')[2]) + eventInfo.time;
                                tempEaseing.startTime = eventInfo.time;
                                easings.Add(tempEaseing);
                            }
                            break;
                        case "setMovementAmount":
                            Arrow.movementAmount = float.Parse(eventInfo.data.Split(' ')[0]);
                            break;
                        case "setCollumnModPercent":
                            //col, mod, percent
                            receivers[int.Parse(eventInfo.data.Split(' ')[0])].mods[eventInfo.data.Split(' ')[1]] = float.Parse(eventInfo.data.Split(' ')[2]);
                            break;
                        case "setModPercent":
                            //mod, percent
                            for (int i = 0; i < 4; i++)
                            {
                                receivers[i].mods[eventInfo.data.Split(' ')[0]] = float.Parse(eventInfo.data.Split(' ')[1]);
                            }
                            break;
                        case "setModPercentEase":
                            //mod, startpercent, endpercent, ease, time
                            for (int i = 0; i < 4; i++)
                            {
                                tempEaseing.receiver = i;
                                tempEaseing.modName = eventInfo.data.Split(' ')[0];
                                tempEaseing.startValueX = float.Parse(eventInfo.data.Split(' ')[1]);
                                tempEaseing.endValueX = float.Parse(eventInfo.data.Split(' ')[2]);
                                tempEaseing.easingType = eventInfo.data.Split(' ')[3];
                                tempEaseing.endTime = float.Parse(eventInfo.data.Split(' ')[4]) + eventInfo.time;
                                tempEaseing.startTime = eventInfo.time;
                                tempEaseing.objectType = easeType.mod;
                                easings.Add(tempEaseing);

                            }
                            break;

                        case "setCollumnModPercentEase":
                            //col mod, startpercent, endpercent, ease, time
                            
                            tempEaseing.receiver = int.Parse(eventInfo.data.Split(' ')[0]);
                            tempEaseing.modName = eventInfo.data.Split(' ')[1];
                            tempEaseing.startValueX = float.Parse(eventInfo.data.Split(' ')[2]);
                            tempEaseing.endValueX = float.Parse(eventInfo.data.Split(' ')[3]);
                            tempEaseing.easingType = eventInfo.data.Split(' ')[4];
                            tempEaseing.endTime = float.Parse(eventInfo.data.Split(' ')[5]) + eventInfo.time;
                            tempEaseing.startTime = eventInfo.time;
                            tempEaseing.objectType = easeType.mod;
                            easings.Add(tempEaseing);
                                break;
                        case "setMovementAmountEase":
                            //start end ease time
                            tempEaseing.startValueX = float.Parse(eventInfo.data.Split(' ')[0]);
                            tempEaseing.endValueX = float.Parse(eventInfo.data.Split(' ')[1]);
                            tempEaseing.easingType = eventInfo.data.Split(' ')[2];
                            tempEaseing.startTime = eventInfo.time;
                            tempEaseing.endTime = eventInfo.time + float.Parse(eventInfo.data.Split(' ')[3]);
                            tempEaseing.objectType = easeType.movementAmount;
                            
                            easings.Add(tempEaseing);
                            break;
                        case "BPMChange":
                            chart.changeBPM(float.Parse(eventInfo.data), eventInfo.time);
                            break;
                        //THIS EVENT IS VOLITALE. IT IS MEANT TO **BREAK THE GAME**
                        case "changeScreenX":
                            windowManager.wwidth1 = int.Parse(eventInfo.data);
                            break;
                        case "changeScreenXEase":
                            //old, new, easing, time
                            tempEaseing.startValueX = int.Parse(eventInfo.data.Split(' ')[0]);
                            tempEaseing.endValueX = int.Parse(eventInfo.data.Split(' ')[1]);
                            tempEaseing.easingType = eventInfo.data.Split(' ')[2];
                            tempEaseing.startTime = eventInfo.time;
                            tempEaseing.endTime = eventInfo.time + float.Parse(eventInfo.data.Split(' ')[3]);
                            tempEaseing.objectType = easeType.screenX;

                            easings.Add(tempEaseing);
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

                        //ease basically takes a percent
                        receivers[item.receiver].xOffset = (int)Math.Round(Ease.Lerp(item.startValueX, item.endValueX, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime))));
                        receivers[item.receiver].yOffset = (int)Math.Round(Ease.Lerp(item.startValueY, item.endValueY, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime))));

                    //windowManager.moveWindow(Ease.Lerp(item.startValueX, item.endValueX, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime))), Ease.Lerp(item.startValueY, item.endValueY, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime))));
                    //is having this after a good idea? maybe? should I have it like this on the other easings?? how do I even test for fuckups?!?!?!? 
                    //FOUND THE FUCKUP! Make sure this sets us to the end
                    if (chart.beat >= item.endTime)
                    {
                        receivers[item.receiver].xOffset = (int)item.endValueX;
                        receivers[item.receiver].yOffset = (int)item.endValueY;
                        toDie.Add(item);
                    }

                } else if(item.objectType == easeType.rotation)
                {
                    if (chart.beat >= item.endTime)
                    {

                        //receivers[item.receiver].rot = item.endValueX;
                        toDie.Add(item);
                    }
                    else
                    {
                        //ease basically takes a percent
                        receivers[item.receiver].rot = Ease.Lerp(item.startValueX, item.endValueX, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime)));
                        
                        //windowManager.moveWindow(Ease.Lerp(item.startValueX, item.endValueX, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime))), Ease.Lerp(item.startValueY, item.endValueY, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime))));
                    }
                } else if(item.objectType == easeType.mod)
                {
                    if(chart.beat >= item.endTime)
                    {
                        toDie.Add(item);
                    } else
                    {
                        receivers[item.receiver].mods[item.modName] = Ease.Lerp(item.startValueX, item.endValueX, Ease.byName[item.easingType]((chart.beat - item.startTime) / (item.endTime - item.startTime)));
                    }
                } else if(item.objectType == easeType.movementAmount)
                {
                    if(chart.beat >= item.endTime)
                    {
                        toDie.Add(item);
                    } else
                    {
                        Arrow.movementAmount = Ease.Lerp(item.startValueX, item.endValueX, Ease.byName[item.easingType](chart.beat - item.startTime) / (item.endTime - item.startTime));
                    }
                } else if(item.objectType == easeType.screenX)
                {
                    if(chart.beat >= item.endTime)
                    {
                        toDie.Add(item);
                        windowManager.wwidth1 = (int)item.endValueX;
                    } else
                    {
                        windowManager.wwidth1 = (int)Math.Round(Ease.Lerp(item.startValueX, item.endValueX, Ease.byName[item.easingType](chart.beat - item.startTime) / (item.endTime - item.startTime)));

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
