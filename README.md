# Remotely Controlling a NAO Robot using Virtual Reality
## Who am I right NAO?
Sorry for the pun. Let me introduce myself.

My name is Daniel Handojo and I am a former Master's student from the University of California, Riverside. This was the project I completed under the guidance of [Dr. Jiasi Chen](http://www.cs.ucr.edu/~jiasi/) in 2017-18. Special thanks to [Dr. Craig Schroeder](http://www.cs.ucr.edu/~craigs/) and [Dr. Tamar Shinar](http://www.cs.ucr.edu/~shinar/) for advice during the planning stage, loaning the actual NAO robot itself, and attending my project presentation. A paper was written for this report, but is not included in this repository in case I decide to submit it to a workshop one day. It is available upon request, or you can email me at [daniel_handojo@yahoo.com](mailto:daniel_handojo@yahoo.com).

## General Summary
This application can be broken down into two main subcomponents: an application developed with the Unity3D game engine that interfaces with the Oculus Rift and a Python script that handles message passing between the Unity3D application and the NAO robot. The Python script continuously requests new images from the NAO robot and sends the images to the Unity3D application. The Unity3D application sends the Rift headset's yaw and pitch rotation, and the touch controller's positions and button presses, to the NAO robot. 

All of this condenses into 3 main features:
1. You can *see* what the NAO robot sees in VR. (Sadly, the NAO robot does not have stereoscopic vision, so no 3D. This is still pretty awesome though)
2. You can control where NAO looks using the Oculus Rift headset's orientation (*where you look, NAO looks*). 
3. You can make NAO walk around using the joysticks on the Oculus Rift's Touch controllers.
4. You can control how NAO's arms move with the Oculus Rift's Touch controllers!!!

## Relevant Files
The Oculus1 folder contains the Unity3D project that interfaces with the Oculus Rift. The relevant scene file is MainNaoController. Any other scenes or files not relevant to that controller are leftover from old experiments and tutorials I went through while developing this application.

PythonNao/Merged/unityserver.py is the Python script that handles message passing between the Unity3D application and the NAO robot. Run this using Python 2.7.14 (since this is what the [NAO robot's API](http://doc.aldebaran.com/2-1/dev/python/install_guide.html#python-install-guide) supports). All of the other Python scripts are debugging scripts that helped me learn how NAO's Python APIs worked. Pardon the mess.

## How to Run
For this project, I used [Unity3D 2017.3.1f1](https://unity3d.com/get-unity/download/archive), [Python 2.7](https://www.python.org/downloads/release/python-2715/), and a [NAO H21 model, V3.3 model](http://doc.aldebaran.com/2-1/family/body_type.html) running the 1.14.5 geode version of NAOqi OS. You can (hopefully) find the image for the OS in the archives of the software page of the developer portal [here](https://community.ald.softbankrobotics.com/en/resources/software/robot/nao-2). I also use the Oculus Rift with Touch Controllers.

1. Turn on the NAO robot and (preferably) connect it to a wifi network. The directions for that can be found in NAO's documentation (albeit with some struggle and an ethernet cable) [here](http://doc.aldebaran.com/2-1/index.html). Retrieve its IP address.
2. Open the Python script and modify the IP address for the `PythonToNAO` class. 
3. Run the Python script. The script will connect to NAO first and have it stand up. Then it will attempt to connect to the Unity3D application.
4. Open the Unity3D application *on the same machine as the Python script*. If you don't want to do that, then modify the fields for the `PythonToUnity` class in the Python script and the `Connector` class in Oculus1/Assets/Plugins.
5. Open the MainNaoController scene in the \_Scenes folder. 
6. Run the debug mode for Unity3D (or build an executable of the project if you'd like!). The Unity3D application will connect to the waiting Python script when it starts (but if it doesn't, press Y on the Touch controllers).
7. Follow the on-screen (in VR) instructions for calibrating your Touch controller's positions. Then you'll be able to control NAO's arms. The rest of the previously listed features work immediately.

## Disclaimer/Final Notes
This project has a lot of flaws, and I may/may not come back to this project. But I had a HUGE BLAST working on this and I'm proud of my little robot for what it was able to do. I definitely could optimize and make the application generally better, but for now, enjoy the project and even improve it yourself. If you do, I'd love to know/see it, so could you fork this project and/or credit me for the original work? Thanks and have fun with VR and Robotics. 
