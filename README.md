# Board Defence - Tower Defense Game

A grid-based tower defense game developed in Unity as a technical case study, showcasing clean architecture, scalable
systems, and performance-focused design patterns.

## 🎮 Project Overview

Board Defence is a classic tower defense game where players strategically place towers on a grid-based board to defend
against waves of incoming enemies.

---

## 🛠️ Technical Stack

- **Unity 6000.2.f2 LTS** - This is the recommended version for best compatibility and after the security vulnerability that happened with Unity, they released a new version of the 6000.2.f1 stable version. So I used this one as my project.
- **Zenject** - Dependency injection framework that I used in the past before. It is very stable and mature and has solid documentation. I used it to manage dependencies and lifecycles of game systems and entities. I decided to avoid its Memory Pool and Signal Bus features in favour of writing my own implementations.
- **UniTask** - Outlasts Unity's built-in async/await support with better performance and features. I used it for asynchronous operations like enemy spawning and animation playback. It offers zero heap allocation which is crucial for performance as the game scales significantly.
- **PrimeTween** - I recently started to discover Prime Tween and I found it very useful for simple tweening needs. I used it for smooth transitions and animations, such as tower attack animations and enemy death effects. It is a strong alternative to DOTween and I will definitely use it more. Same as UniTask, it offers zero heap allocation which is crucial for performance as the game scales significantly.
- **Addressables** - One of the best features that Unity can offer. I used Addressables for efficient asset management and loading, especially for pooled entities like towers, enemies, and projectiles. It helps reduce memory usage and load times by loading assets on demand. It might be an overkill for this small project, but I wanted to demonstrate a small portion of my knowledge with the Addressable System.

---

## 🎨 Visual Polish

- I made some visual polish, although it is not much. I added simple animations to towers and enemies using PrimeTween for smooth transitions.
- I created simple UI elements to display wave information and player feedback during gameplay.

---

## 🚀 Running the Project

1. I highly advise you to play with Unity 6000.2.06f2 or later versions. Compatibility issues may arise with earlier versions and I can't guarantee that it will run as smoothly as expected.
2. If you ever make changes to the Addressables assets, please make sure to rebuild the Addressables by navigating to `Window > Asset Management > Addressables > Groups` and clicking on `Build > New Build > Default Build Script`. Because you will face the problem of your prefabs not being updated in the addressable groups because you haven't build them.

---

## 📊 Performance Considerations

- **Object Pooling**: I leveraged the usage of object pooling to minimize the allocation costs in the memory, reusing the returned objects again to manage a list of objects.
- **Math Based Detection**: I deliberitely avoided Unity's physics system for enemy detection. I followd a math-based approach using distance calculations to identify enemies within tower range or within a projectile explosion range.
- **Event Subscription Cleanup**: I tracked all of my subscriptions and unsubscribed from them in `OnDestroy()`/`Dispose()` methods to prevent memory leaks. Monobehaviours benefit from their cleanup from their cycle but IDisposable systems also clean up their objects as well.

---

## 🎯 Future Enhancements

If given more time, I would add:

1. **Tower Upgrades**: Multi-tier upgrade paths for towers, which is very popular with dominant tower defense games on the market. It is definitely a fun feature to implement.
2. **Special Abilities**: We can offer a variety of player-activated abilities like airstrikes or temporary buffs. This is, again, popular in tower defense games and would add an extra layer of strategy.
3. **Enemy Pathfinding**: The pathfinding was not the priority for this project, so I implemented a simple straight-line movement for enemies. However, implementing A* pathfinding would allow for more complex enemy movement and dynamic obstacle avoidance. As well as, adding smarter AI logic to the enemies....
4. **Resource Economy**: We are currently not offering a resource economy system. Implementing a currency system where players earn resources for defeating enemies and can spend them on towers and upgrades would add depth to the gameplay.
5. **Save System**: Implementing a save/load system to allow players to save their progress and continue later. Again, this is also a fundamental need if this project ever grows.
6. **Audio System**: A couple of sound effects would mean a lot for the game experience. Background music, tower attack sounds, and enemy death sounds would enhance immersion.
7. **Particle Effects**: I am not an expert in visual effects, but adding particle effects for tower attacks, enemy deaths, and explosions would improve the visual appeal of the game. This is on my to-do list if I ever plan to enhance this project further.
8. **Unit Tests**: Comprehensive test coverage for core systems. Considering that this project leverages Zenject for
   dependency injection, I would implement unit tests for key systems such as the WaveSystem, LevelSystem, and State
   Machines. This would ensure that the game logic behaves as expected and would point out some ideas about future refactoring and
   feature additions.

---
