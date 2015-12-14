using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class MapNodeJSON
    {
        public static String mapNodeJSON = @"

{ 
Name: 'Main Lobby',
xPos: 926,
yPos: 869,
iconPosX: 78,
iconPosY: 66,
width: 194,
height: 436,
parentName: 'Water Falls High School'
}
{ 
Name: 'The Quad',
xPos: 662,
yPos: 1019,
iconPosX: 47,
iconPosY: 25,
width: 194,
height: 120,
parentName: 'Water Falls High School'
}
{ 
Name: 'The Far Side',
xPos: 315,
yPos: 986,
iconPosX: 26,
iconPosY: 26,
width: 271,
height: 120,
parentName: 'Water Falls High School'
}
{ 
Name: 'South Hall',
xPos: 71,
yPos: 1221,
iconPosX: 97,
iconPosY: 32,
width: 650,
height: 122,
locker: true,
parentName: 'Water Falls High School'
}
{ 
Name: 'Dwarves & Druids Club',
xPos: 120,
yPos: 1414,
iconPosX: 26,
iconPosY: 40,
width: 194,
height: 120,
parentName: 'Water Falls High School'
}
{ 
Name: 'Gym Lobby',
xPos: 29,
yPos: 587,
iconPosX: 25,
iconPosY: 100,
width: 194,
height: 436,
bathroom: true,
parentName: 'Water Falls High School'
}
{ 
Name: 'Janitor\'s Closet',
xPos: 659,
yPos: 694,
iconPosX: 26,
iconPosY: 25,
width: 287,
height: 120,
parentName: 'Water Falls High School'
}
{ 
Name: 'North Hall',
xPos: 164,
yPos: 425,
iconPosX: 130,
iconPosY: 93,
width: 650,
height: 122,
parentName: 'Water Falls High School',
bathroom: true,
locker: true,
locks: [ { type: 'Gold', x:387, y: 448, destinationMapName: 'Intro to Science'} ]
}
{ 
Name: 'East Hall',
xPos: 1013,
yPos: 509,
iconPosX: 37,
iconPosY: 118,
width: 194,
height: 190,
parentName: 'Water Falls High School',
locks: [ { type: 'Gold', x:982, y: 732, destinationMapName: 'Janitor\'s Closet'} ]
}
{ 
Name: 'Kitchen',
xPos: 1278,
yPos: 625,
width: 194,
height: 120,
iconPosX: 39,
iconPosY: 27,
parentName: 'Water Falls High School'
}
{ 
Name: 'Upstairs',
xPos: 253,
yPos: 168,
iconPosX: 40,
iconPosY: 48,
width: 650,
height: 122,
parentName: 'Water Falls High School',
locker: true,
bathroom: true,
trenchcoat: true
}
{ 
Name: 'Paranormal Club',
xPos: 605,
yPos: 0,
iconPosX: 26,
iconPosY: 26,
width: 243,
height: 120,
parentName: 'Water Falls High School'
}
{ 
Name: 'Intro to Science',
xPos: 0,
yPos: 58,
iconPosX: 24,
iconPosY: 81,
width: 277,
height: 113,
parentName: 'Science'
}
{ 
Name: 'Science 101',
xPos: 331,
yPos: 54,
iconPosX: 362,
iconPosY: 81,
width: 277,
height: 113,
parentName: 'Science'
}
{ 
Name: 'Science 102',
xPos: 669,
yPos: 58,
iconPosX: 701,
iconPosY: 81,
width: 277,
height: 113,
parentName: 'Science'
}
{ 
Name: 'Science 103',
xPos: 1008,
yPos: 58,
iconPosX: 33,
iconPosY: 23,
width: 277,
height: 113,
parentName: 'Science',
bathroom: true
}
{ 
Name: 'Science 104',
xPos: 561,
yPos: 0,
iconPosX: 1377,
iconPosY: 81,
width: 277,
height: 113,
parentName: 'Science'
}
{ 
Name: 'Science 105',
xPos: 1684,
yPos: 58,
iconPosX: 34,
iconPosY: 24,
width: 277,
height: 113,
parentName: 'Science',
trenchcoat: true
}
{ 
Name: 'Stone Fort Gate',
xPos: 0,
yPos: 117,
iconPosX: 25,
iconPosY: 24,
width: 277,
height: 113,
parentName: 'History',
bathroom: true
}
{ 
Name: 'Stone Fort - Central',
xPos: 350,
yPos: 5,
iconPosX: 49,
iconPosY: 24,
width: 145,
height: 327,
parentName: 'History',
}
{ 
Name: 'Stone Fort - East',
xPos: 581,
yPos: 0,
iconPosX: 41,
iconPosY: 32,
width: 327,
height: 145,
parentName: 'History',
}
{
Name: 'Stone Fort - West',
xPos: 581,
yPos: 197,
iconPosX: 41,
iconPosY: 32,
width: 327,
height: 145,
parentName: 'History',
}
{
Name: 'Stone Fort Wasteland',
xPos: 669,
yPos: 377,
iconPosX: 24,
iconPosY: 58,
width: 169,
height: 176,
parentName: 'History',
}
";
    }
}
