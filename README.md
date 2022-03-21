
# Asteroids 🛸
###### for a TSB test

---

#### Controls 
- Movement: WASD/Arrow-Keys

- Fire: Spacebar

- HyperSpace: Shift

---
#### Being designers/artist-friendly
In my limited experience with DOTS my projects have quickly become designer unfriendly. Few editor interfaces exist which can allow behavioiurs to be tuned in the editor, and when they do, are often either limited in capability or intuitive.

User-friendly editors and workflows are a **hugely** important aspect of development. Otherwise everyone is required to be completely code-fluent experts of all systems in the game to be able to contribute productivily.

With this in mind, I wanted to make sure that authoring components were reasonably intuitive and powerful, ensuring that a lot of behaviour could be tweaked and adjusted in the scene view itself, sans recompile. I also wanted to experiment with different strategies to get larger config data into the DOTS world, (FixedList, AssetBlobs.. etc).

----

#### Architecture in a DOTS game
I wanted to explore different patterns which allowed interdependent behaviours be driven by decoupled systems. What does this look like in DOTS? Because of this I tried to use schedule() and ScheduleParralel() wherever possible, and avoid creating any unnecessary new sync points. This wasn't so much for performance reasons, but instead it was to uncover some of the challenges involved with writing code in multi-threaded, Burst compiled C# (HPC#).

- I experimented with flags to signal data-oriented "events" with my overlap and destruction system. 

- I experimented with decoupling the concept of "drivers" --> "launchers" --> "projectiles". 

- I experimented with allowing systems to clean up resources that "belonged" to them (from a memory ownership POV), but were nevertheless attached to entities, using system state components. 

---

#### Data Orientation and Performance
In many ways my prototype is  **not** data oriented, for instance, my overlap system is far more generic than it has to be for an asteroids game, and it doesn't make use of chunk-based filtering that ECS lends itself to and that already comes for free  with some of the components that the renderer uses to cull objects. 

---
#### Spawning System and Bugs
Originally I started creating a more procedural system which would spawn entities based of weights which would change over time.

I instead decided to try something more hand-authored and designer friendly, with pacing baked in. This lead the creation of a wave-based spawning method where designers would be able to hand pick what objects would be spawned in each wave. And it what order.

I tried  converting the serialized MonoBehaviours and ScriptableObjects to  both assetblobs, DynamicBuffers and FixedLists. Unfortunately the conversion process of these configs remained broken throughout development. 
