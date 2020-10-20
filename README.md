# Drift-Learner
<img src="https://github.com/Manjunatha-b/Drift-Learner/blob/master/Still.jpeg" width="800">

## Introduction & Details :
Project was aimed at making a reinforcement learning algorithm capable of driving a drifty car.<br/><br/>
**AI Based details:**
1. A game environment was developed with car physics, drifting, skidmarks and anti roll mechanics.
1. Reset mechanics were introduced to speedup the process 
    * Resets if the car flips
    * Resets if the car crashes
    * Resets if it doesn't reach the next checkpoint in time.
1. Added penalty for 
    * mean square distance
    * angular difference 
    * collision with mountain.
1. Added waypoints along the road for the AI to reach and obtain reward. waypoint colliders were created dynamically during execution calculating the front facing vector using the perpendicular vector formula to the guiding spline<br/>
1. Observations for input: 
      * the next 3 waypoints
      * velocity of the car 
      * depth raycasts from the car's roof slightly pointed towards the ground
1. Created two types ofbased on wheel physics settings:
      * highly gripping car, with very responsive control
      * highly drifty car, with low responsive control
1. Since neither of the two cars were learning a **shuffle algorithm was added which spawned the car at random points along the track**
1. Both were left to train overnight ~12 hours with 4x instances of the game running.
1. Experimented with imitation learning
1. **Only the car with high grip coefficient on the tires was able to learn anything substantial**

**Graphics based details:**
1. Introduced occlusion culling which sent framerate up to about ~45
1. Added postprocess, ambient occlusion, color grading, depth of field and fog at some point along the development cycle but later removed while training AI
1. the mountain and track models were made from blender and the polycount was kept as low as possible.( Very Low poly road made the car bounce ).
1. skidmarks script was taken from here: <ahref>https://github.com/Nition/UnitySkidmarks</ahref>


<br/>
The project was slightly *ambitious* and went through many changes with one hard reset<br/>


## Example of its working 
Here is a gif of me driving the drifty car with arrow keys as input.<br/>
<img src="https://github.com/Manjunatha-b/Drift-Learner/blob/master/simple car driving.gif" width="800">
<br/><br/>
This is the AI driving the grippy car. Very wonky controls.<br/>
<img src="https://github.com/Manjunatha-b/Drift-Learner/blob/master/Aidriving.gif" width="800">
<br/>
Will get back to this project after learning some more about reinforcement learning.

### Todo:
1. Increase decision interval.
1. Curriculum Learning from grip to drift coefficients.
1. Curriculum Learning for brake strength.
1. Action smoothing, reward if similar to previous state.
1. Learn RAINBOW.
1. CNN input.
