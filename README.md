# Games Engines 1 Project
Assignment 1 of Games Engines 1<br>
Damian Wojtowicz<br>
C17413722<br>
DT228/4<br>

## Project Idea
The idea is to create an audio responsive procedurally generated tunnel that the camera will move through.<br>
I have several ideas for how the project will work and these are not entirely set in stone and will probably change as the project progresses.<br>
<br>

**Note:** The colours in the images are not representative of the final program.

The inital idea (Prototype 1) was to have an audio responsive ring that generates more rings at intervals and moves these rings futher away from the camera, creating a tunnel effect. The opacity of the ring will change as it moves further away. Additionally dots/shperes will spawn and copies of these dots will be created at intervals in similar fashion and move away from the original, creating a trailing effect. Although I couldn't capture it in the image due to my inexperience with drawing, the rings are supposed to look different from one another.
<br><br>
However, I wanted to explore the possibility of going through the tunnel itself. This could be the second stage of the music visualization when enough rings are generated to create a tunnel. The rings will can then switch to a state like in the second image (Prototype 2) where they are no longer opaque and ununiform. I plan to make the camera move through the tunnel at a speed relative to the beat. I also intend to scale the cubes within the tunnel and the rectangles that make up the rings to the beat. Dynamic colour changing will be experimented with as well to create an interesting visual experience.
<br><br>
I would also like to experiment with bending the tunnel if I have enough time.
<br><br>

### Prototype 1 (Drawing)
<img src="https://i.imgur.com/ph76RcP.png" width="50%" height="50%">

### Prototype 2 (Drawing)
<img src="https://i.imgur.com/3jsoB7Z.png" width="50%" height="50%">

## Research and Inspirations
#### Audio Visualizer Inspiration
<img src="https://i.imgur.com/gHSZeQ0.png" width="50%" height="50%"><br>
https://www.youtube.com/watch?v=gHpxRv4MBBA&t=3s&ab_channel=KnukN<br><br>
The video features an audio responsive visualizer and the points of interest for me were the movement/speed of the camera in response to the music. I found that very interesting and seeing the effect it has on the experience makes me want to implement something similar. I also like the randomness of the cubes and their movements as it gives the visuals that random aspect which I personally enjoy. However, the visualizer was implemented in Processing 3, meaning I will have to figure out how to code it in Unity.
<br><br><br>

#### Audio Visualizer Tutorial #1
<img src="https://i.imgur.com/gh7wGBn.png" width="50%" height="50%"><br>
https://www.youtube.com/watch?v=wtXirrO-iNA&ab_channel=N3KEN<br><br>
Approximately knowing what I would like to create I began with looking at online tutorials that could help me get a better idea on the implementation. This video has taught me how to visual an audio spectrum and the basics for how it works.
<br><br><br>

#### Audio Visualizer Tutorial #2
<img src="https://i.imgur.com/5lv2taD.png" width="50%" height="50%"><br>
https://www.youtube.com/watch?v=PzVbaaxgPco&ab_channel=RenaissanceCoders<br><br>
Although the last example was very helpful I wanted to get an even better idea of the implementation of an audio visualizer in Unity. This tutorial explains the various components and values that can be extracted from an audio source and how these can be put to use. This tutorial works with UI objects as opposed to 3D game objects like I am planing to, however, it was still very useful in understand how audio visualizing works and the concepts behind it.

#### Audio Visualizer Tutorial #3
https://forum.unity.com/threads/audio-visualization-tutorial-unity-c-q-a.432461/
This is a forum on unity with several videos explaining how audio visualization works in unity and in general. I found it helpful as well in better understanding how audio visualizing works.
