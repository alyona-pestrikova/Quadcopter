Drone Explorer

Project:
This project enables the exploration of 3D space by way of a quad-copter drone. 
The intent is to provide a quick drop in package to allow a user to explore a 
scene in the Unity Play engine. The code is written to be modular and can incorporate 
other gameplay elements with the addition of more scripts.

Author: Wilson Sauders 
Email:  HamsterUnity@gmail.com
Date: 03/14/2022 (Happy Pie day)

Versions
1.0.0 - 03/14/2022 
* Initial Release

1.0.1 - 03/20/2022 
* Added colors to sample scene
* Replaced spheres and cube with a drone mesh
* Added bullet and bullet Pooling
* Added gun object and link to CmnInterrface
* Added Popup targets and controller scripts to pop them up in groups
* Created combat and noncombat prefabs and demo scenes

1.0.2 - 05/09/2022
* Added CourseCtrl for time attack game play mode.
* Added particle effects to TargetGroup to act as fly through gate marker
* Added GhostLine which traces the previous flight path.
* Added Main Menu class and prefab for navigating between courses
* Added PlayerPrefs interaction to store user settings and high scores.


Importing:
The package should unpack to folder:
Assets\Packages\DroneExplorer\

To get a feel for what this package can do the sample scenes are loaded to 
this folder:
Assets\Packages\DroneExplorer\Scene\

DemoExplore - The basic implementation of the DroneContainer object with 
some simple lighting and objects for  reference.

DemoCombat - The basic implementation of the CombatContainer object. There 
are a scattering of targets to interact with and a target group controller that 
brings targets up and down.

DemoRace - The basic implementation of GateRaceContainer. It contains a CourseCtrl with all the attendant object links. There is also a UI canvas and a series of gates to fly through.

DemoBiathlon - Similar to DemoRace only with demo combat elements and targets to shoot midway.

DemoObsticle - A more complex implementation of DemoRace.

MainMenu - A central scene that allows the user to navigate to DemoRace, DemoBiathlon, and DemoObsticle. 

To setup the scene swapping, the use will have to open the File->Build Settings. Then 
drag and drop the following scenes into the Scenes In Build box in this order:
* MainMenu
* DemoRace
* DemoBiathlon
* DemoObsticle

The Demo Scenes should be playable with the mouse keyboard interface with the 
standard unity project. If the user wants to import the code into a Demo Scene 
they can drag the DroneContainer prefab from the prefabs folder into the scene. 
The user must then link the Scene's Main Camera into the DroneLook Object's "CamObj" 
variable to bind the camera to the interface.

If the user wants to import the Combat system into a custom scene import the 
DemoCombat prefab then link the Scene's Main Camera into the DroneLook Object's "CamObj" 
variable to bind the camera to the interface.

Controls: 
With the Default Mouse Keyboard interface the controls are as follows:
Forward / Back:				W / S
Left / Right: 				A / D
Go Up: 					Space
Go Down: 				Left Shift
Pan Camera Up / Down / Left / Right: 	Mouse Movement

With Gamepad:
The user must add the following joystick axes to the 
Project Setting -> Input Manager Axes List:

Thrust:  				Joystick Axis 3rd Axis
LookX:  				Joystick Axis 4th Axis
LookY:  				Joystick Axis 5th Axis

Once these parameters are set the user must disable the 
DroneContainer->CommonInterface->PlayerMouseKeyboard object
and enable the 
DroneContainer->CommonInterface->PlayerXbox object.

With the XBox Gamepad:

Forward / Back / Left / Right:  	Left Stick
Pan Camera Up / Down / Left / Right: 	Right Stick
Go Up:  				Right Trigger
Go Down: 				Left Trigger

Prefabs:
Bullet - This contains a single bullet with a trail and particle system for impact explosion. THe 
prfab should not be placed in the scene but rather referenced by BulletPool.

BulletPool - This instantiates multiple bullet objects and will create a static Instance reference 
to itself which all other objects in the game can reference to spawn bullets. Import this prefab into 
scene if you want to shoot bullets in it.

How to make your own Scene:
1) Create a scene with meshes and colliders (or import one from another package).
2) Drop the prefab Assets\Packages\DroneExplorer\Prefab\GateRaceContainer into the scene.
3) Bind Main Camera into GateRaceContainer->DroneContainer->DroneLook->CamObj
4) Right click GateRaceContainer -> Prefab -> Unpack
5) Expand GateRaceContainer -> Course: Move the gates about to what makes sense to your scene. Duplicate or remove gates as needed. Different gates can be added from the Assets\Packages\DroneExplorer\Prefab\Gates\
6) Playtest your course, get a feel for what is a good time and what is an acceptable time. Move gates as needed so the course feels good.
7) Save your scene
8) Add your scene to File->BuildSettings->Scenes in Build
9) Open MainMenu Scene
10) Expand Canvas->MenuCtrl->LevSelect->Scroll View List->Viewport->Content
11) Duplicate LevelMarker (0)
12) In LevelMarker Level Marker Component change Following
12a) LevelName to the file name of the scene you are adding
12b) Title to the text you want to see
12c) Description to the text you want to see in the description area when the level is picked. Use "|" to denote new lines in your description.
12d) Star Times set element 0  to the time the user must beat to get 1 star, element 1 to get 2 stars, and element 2 to get 3 starts.
13) Move the LevelMarker object to the bottom of the list -30 - 50*index
14) Congrats you are done. Play test your new creation.



Licencing:
This package is provided as is. The original developer (Wilson Saunders) is under no obligation to support or maintain it. The code associated prefab files are free to include in Games and Unity Asset Packs with or without attribution to the original developer.