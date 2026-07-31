# Chronicles of the Lost Dungeon

A third-person dungeon crawler built in Unity 6. You fight your way through five areas of a dungeon, each one gated behind an enemy you have to beat. Kill the right enemy and a staircase unlocks, or a health orb appears, or the screen fades and drops you into the next area. Beat the boss in area 5 and the run is over.

Progress saves to disk, so the levels you've cleared stay cleared and the level select screen remembers them.

## Controls

| Action | Keyboard | Mobile |
|---|---|---|
| Move | WASD or arrow keys | on-screen joystick |
| Jump | Space | — |
| Melee attack | J | — |
| Fireball | K | — |
| Lightning | L | — |
| Interact | F | — |
| Menu / pause | Esc | — |

Movement is camera-relative, so the character turns to face wherever you push. Interact works on whatever interactable is closest inside a 2.5 unit radius — staircases, health orbs, and so on.

## Running it

You need Unity 6 (`6000.4.5f1`). Anything older won't open the project.

1. Clone the repo:
   ```
   git clone https://github.com/Remy-cloud/Lost-Dungeon.git
   ```
2. Add the folder in Unity Hub and open it. First open takes a while — Unity has to rebuild the Library folder, which isn't committed.
3. Open `Assets/Scenes/SampleScene.unity` and hit Play.

All five levels live in that one scene as separate areas. Moving between them is a teleport to a spawn point, not a scene load, so there's no loading screen anywhere in the game.

## Builds

The project targets Windows, WebGL and Android from the same codebase.

- **Windows / Mac** — File > Build Settings, pick the platform, build.
- **WebGL** — same, but expect a long first build.
- **Android** — before you build, set a bundle identifier under Player Settings > Other Settings (it's blank in the repo, and Android won't build without one), and switch the scripting backend to IL2CPP if you're targeting ARM64. The game is laid out for landscape.

Build output goes to `Builds/`, which is gitignored on purpose. Committing 50MB+ of binaries into a source repo makes it slow to clone forever, so builds belong on a GitHub Release instead.

## How the code is put together

Everything gameplay-related sits in `Assets/Scripts`, with tests in `Assets/Scripts/Tests` behind their own assembly definition so they don't ship in a player build.

### Design patterns

**Singleton** — `GameManager`, `SaveManager`, `AudioManager`, `ObjectPool` and `ScreenFader` each keep a static `Instance` and survive scene changes with `DontDestroyOnLoad`. Anything can reach them without a serialized reference.

**Observer** — the game runs on static C# events instead of direct calls. `Health` fires `OnHealthChanged` and `OnDeath`; `SaveManager` fires `OnLevelCompleted` and `OnLevelUnlocked`; `GameManager` fires state, enemy-defeated, item-collected and ability events. `AudioManager` and `GameOverHandler` just subscribe. Nothing in the combat code knows the UI or the audio system exists.

**Strategy** — `IEnemyBehaviour` defines `Chase`, `Attack` and an `AttackRange`. Four classes implement it differently:

- `MeleeBehaviour` walks at you and swings on a cooldown
- `RangedFireBehaviour` holds position and fires pooled projectiles
- `IceGuardianBehaviour` cycles between guarding (damage reduced to 10%) and a short vulnerable window
- `TowerPatrolBehaviour` strafes left and right while shooting spikes

`EnemyStateMachine` grabs whichever one is attached with `GetComponent<IEnemyBehaviour>()` and never has to care which it got. Swapping an enemy's whole fighting style means swapping one component.

**State** — `EnemyStateMachine` runs Idle → Chase → Attack → Dead, driven by distance to the player and the behaviour's own attack range. `GameManager` tracks a separate `GameState` enum for MainMenu, Playing, Paused and GameOver.

**Object pooling** — `ObjectPool` builds a queue of inactive instances per tag at startup. Every projectile in the game (fireballs, ice shards, spikes) is dequeued, repositioned, and pushed back to the end of the queue instead of being instantiated and destroyed mid-fight.

### Interfaces

| Interface | Used for |
|---|---|
| `IDamageable` | anything that can take damage — player and enemies share the same `Health` component |
| `IEnemyBehaviour` | the four enemy fighting styles |
| `IAbility` | player abilities with an activation and a cooldown |
| `IInteractable` | staircases and collectibles that respond to the F key |
| `ISaveable` | contract for pushing and pulling state from `PlayerSaveData` |

### Saving

`PlayerSaveData` is a plain serializable class holding the highest unlocked level, the list of completed levels, health, enemy and death counts, inventory, volume settings and unlocked abilities. `SaveManager` writes it as formatted JSON to `Application.persistentDataPath/savegame.json` with `JsonUtility`, and reads it back on `Awake`. If no file exists, it starts fresh.

Saves happen when a level is completed and when you back out of the settings screen.

### REST API

`ApiService` calls `https://catfact.ninja/fact` with `UnityWebRequest` in a coroutine on startup and logs the response. Failures are caught and logged rather than thrown, so no network means no crash — the game just carries on.

### Algorithms

- **Sorting** — completed levels are kept in ascending order with `List.Sort()` so the level select screen reads correctly no matter what order you finish things in.
- **Critical hits** — `Health.TakeDamage` rolls a random value against a 20% threshold and doubles damage when it lands. The result then gets multiplied by an incoming-damage modifier, which is what lets the Ice Guardian shrug off hits while guarding.
- **Closest-target search** — `PlayerInteraction` collects every collider in range, filters to the ones that are interactable and available, and does a linear pass tracking the minimum distance. You always grab the nearest thing, never a random one.

### Platform-specific code

`#if` blocks handle the differences instead of separate builds:

- `PlatformSettings` caps mobile at 30fps on low quality, desktop and web at 60 on higher settings
- `PlayerController` reads the on-screen joystick on Android and iOS, and the keyboard everywhere else
- `MobileControlsVisibility` switches the touch UI off entirely outside mobile
- The quit button in `MainMenuController` stops play mode in the editor instead of calling `Application.Quit`

## Tests

Nine NUnit tests in `Assets/Scripts/Tests`, run through Window > General > Test Runner in Edit Mode.

`HealthTests` covers damage reduction, death at zero, healing clamped at max, and that a dead thing ignores further damage. `AlgorithmTests` covers the sort order, the crit multiplier and the level unlock rule. `SaveDataAndPoolTests` covers a JSON round trip and pooled object activation.

## Project layout

```
Assets/
├── Scenes/SampleScene.unity     the whole game
├── Scripts/
│   ├── Tests/                   edit mode tests
│   └── *.cs                     gameplay, UI, systems
├── prefabs/                     enemies, projectiles, pickups
├── player/                      player model and animations
├── material/
├── Audio/
├── Resources/
└── Settings/                    URP render pipeline assets
```

## Built with

Unity 6000.4.5f1, Universal Render Pipeline, TextMesh Pro, Unity Test Framework,C# throughout.
