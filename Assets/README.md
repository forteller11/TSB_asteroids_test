


#### Controls 
- Movement: WASD/Arrow-Keys

- Fire: Spacebar

- HyperSpace: Shift

### Goals
##### designers/artist-friendly
In my limited experience with DOTS my projects have quickly become designer unfriendly where very little can be configured or changed in the Editor, and if it can be changed, the authoring is often unintuitive.

User-friendly editors and workflows are a **huge** part of game development within a team where not everybody is code-fluent, and not everybody is an expert in every part of the codebase. 
With this in mind, I wanted to make sure that authoring components were reasonably intuitive and powerful, ensuring that a lot of configuration work take place in the scene view itself. I also wanted to experiment with different strategies to get larger configuration data into the DOTS world, (FixedList, AssetBlobs.. etc).

----

#### architecture
I wanted to explore what decoupled, yet interrelated systems looks like in DOTS. Looks like with multithreaded Jobs, scheduled within systems, acting of compoenents. Because of this I tried to use schedule() and Scheduleparrlel() wherever possible, not for performance reasons,  but to the challenges involved writing code in multithreading, Burst compiled C# 

- I experimented with flags to signal data-oriented "events" with my overlap and destruction system. 

- I experimented with decoupling the concept of "drivers" --> "launchers" --> "projectiles". 

- I experimented with allowing systems to clean up resources that "belonged" to them, but where attached to entities, with system state components. 

---

#### Data Orientation and Performance
In many ways my prototype is  **not** data oriented, for instance, my overlap system is far more generic than it has to be for an asteroids game, and it doesn't make use of chunk-based filtering that ECS lends itself to and that already comes with some of the components that the renderer uses to cull objects.


### Woes
Spawning system
