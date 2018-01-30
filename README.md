# Handyman Project

This is a Unity project for the competition of Handyman task of the Partner Robot Challenge (Virtual Space) in the World Robot Competition.

Please prepare a common unitypackage in advance, and import the unitypackage to this project.  
For details of using the common unitypackage, please see an introduction in the following repository.
https://github.com/PartnerRobotChallengeVirtual/common-unity.git

## Prerequisites

- OS: Windows 10
- Unity version: 2017.3

## How to Build

### Import the common Unitypackage

1. Open this project with Unity.
2. Click [Assets]-[Import Package]-[Custom Package...].
3. Select a common unitypackage (e.g. wrs-virtual-common.unitypackage) and open the file.
4. Click [Import] button.
5. Please confirm that no error occurred in Console window.

### Build
1. Create a "Build" folder under this project folder.
2. Open this project with Unity.
3. Click [File]-[Build Settings].
4. Click [Build]
5. Select the "Build" folder , and type a file name (e.g. Handyman) and save the file.

## How to Set Up

### Modify Configuration

1. Open this project with Unity.
2. Click [SIGVerse]-[SIGVerse Settings].  
SIGVerse window will be opened.
3. Type the IP address of ROS to "Rosbridge IP" in SIGVerse window.

## How to Execute Handyman Program

Please start the ROS side application beforehand.

<font color="Red"><b>
Note: Currently, task message supports only the following formats.  
"Go to the AAA,  grasp the BBB and come back here".
</b></font>

### Execute On Unity Editor
1. Double click "Assets/Competition/Handyman/Handyman(.unity)" in Project window.
2. Click the Play button at the top of the Unity editor.

### Execute the Executable file
1. Copy the "SIGVerseConfig" folder into the "Build" folder.
2. Double Click the "Handyman.exe" in the "Build" folder.

## License

This project is licensed under the SIGVerse License - see the LICENSE.txt file for details.
