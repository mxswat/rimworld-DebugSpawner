# Debug Spawner

Adds a **Spawn faction rows** submenu under the **Spawning** debug action category (`Ctrl+F9` / Debug Actions menu). For each non-player, non-defeated faction, lists all `PawnKindDef`s that map to it via `defaultFactionDef`.

## Usage

1. Open debug actions and go to **Spawning > Spawn faction rows**.
2. Select a faction by name.
3. Choose **All (N kinds)** or **Fighters only (N kinds)**.
4. Click a cell on the map -- a grid of pawns is spawned at that position.

Each pawn kind gets one row (10 pawns wide, 2-cell row gap). Pawns are generated with `PawnGenerator.GeneratePawn` using that faction and face South.

```
        N                 origin (mouse click)
        ^                    v
        |    ┌──┬──┬──┬──┬──┬──┬──┬──┬──┬──┐
        |    │P1│P2│P3│P4│P5│P6│P7│P8│P9│P10│  ← row 0: pawn kind A
   W <──┼──> E    (empty row)
        |    
        |    ┌──┬──┬──┬──┬──┬──┬──┬──┬──┬──┐
        |    │P1│P2│P3│P4│P5│P6│P7│P8│P9│P10│  ← row 2: pawn kind B
        |         (empty row)
        S    ┌──┬──┬──┬──┬──┬──┬──┬──┬──┬──┐
             │P1│P2│P3│P4│P5│P6│P7│P8│P9│P10│  ← row 4: pawn kind C
              ...all facing ↓ (South)
```

## Build

```powershell
dotnet build "Source\DebugSpawner.csproj"
```
