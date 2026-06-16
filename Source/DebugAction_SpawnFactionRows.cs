using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using RimWorld;
using Verse;

namespace DebugSpawner
{
    public static class DebugAction_SpawnFactionRows
    {
        private const int SpawnCount = 10;
        private const int RowGap = 2;

        [DebugAction("Spawning", allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 500)]
        private static List<DebugActionNode> SpawnFactionRows()
        {
            List<DebugActionNode> list = new List<DebugActionNode>();

            foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
            {
                if (faction.IsPlayer)
                {
                    continue;
                }

                if (faction.defeated)
                {
                    continue;
                }

                List<PawnKindDef> allKinds = DefDatabase<PawnKindDef>.AllDefs
                    .Where(kd => kd.defaultFactionDef == faction.def)
                    .OrderBy(kd => kd.defName)
                    .ToList();

                if (allKinds.Count == 0)
                {
                    continue;
                }

                Faction localFac = faction;
                List<PawnKindDef> localAllKinds = allKinds;
                List<PawnKindDef> localFighterKinds = allKinds.Where(kd => kd.isFighter).ToList();

                DebugActionNode factionNode = new DebugActionNode(localFac.Name + " (" + localFac.def.defName + ")", DebugActionType.Action);
                factionNode.childGetter = () =>
                {
                    List<DebugActionNode> children = new List<DebugActionNode>();

                    children.Add(new DebugActionNode("All (" + localAllKinds.Count + " kinds)", DebugActionType.ToolMap)
                    {
                        action = delegate
                        {
                            SpawnFactionGrid(localFac, localAllKinds, UI.MouseCell());
                        }
                    });

                    if (localFighterKinds.Count > 0)
                    {
                        children.Add(new DebugActionNode("Fighters only (" + localFighterKinds.Count + " kinds)", DebugActionType.ToolMap)
                        {
                            action = delegate
                            {
                                SpawnFactionGrid(localFac, localFighterKinds, UI.MouseCell());
                            }
                        });
                    }

                    return children;
                };

                list.Add(factionNode);
            }

            if (list.Count == 0)
            {
                list.Add(new DebugActionNode("(no factions with pawn kinds available)", DebugActionType.Action));
            }

            return list;
        }

        private static void SpawnFactionGrid(Faction faction, List<PawnKindDef> kinds, IntVec3 origin)
        {
            Map map = Find.CurrentMap;

            for (int row = 0; row < kinds.Count; row++)
            {
                PawnKindDef kind = kinds[row];

                for (int col = 0; col < SpawnCount; col++)
                {
                    IntVec3 cell = origin + new IntVec3(col, 0, row * RowGap);

                    if (!cell.InBounds(map))
                    {
                        continue;
                    }

                    if (!cell.Standable(map))
                    {
                        continue;
                    }

                    Pawn pawn = PawnGenerator.GeneratePawn(kind, faction, -1);
                    GenSpawn.Spawn(pawn, cell, map);
                    pawn.Rotation = Rot4.South;
                }
            }
        }
    }
}
