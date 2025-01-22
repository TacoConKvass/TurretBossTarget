using CalamityMod.TileEntities;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Terraria.ModLoader;

namespace TurretBossTarget;

public class TurretBossTarget : Mod {
	ILHook[] hooks = [
		TurretsTargetBosses_HookFactory<TEPlayerFireTurret>(),
		TurretsTargetBosses_HookFactory<TEPlayerIceTurret>(),
		TurretsTargetBosses_HookFactory<TEPlayerLabTurret>(),
		TurretsTargetBosses_HookFactory<TEPlayerLaserTurret>(),
		TurretsTargetBosses_HookFactory<TEPlayerOnyxTurret>(),
		TurretsTargetBosses_HookFactory<TEPlayerPlagueTurret>(),
		TurretsTargetBosses_HookFactory<TEPlayerWaterTurret>()
	];

	public override void Load() {
		foreach (ILHook hook in hooks) hook.Apply();
	}

	public override void Unload() {
		foreach (ILHook hook in hooks) hook.Undo();
	}

	static ILHook TurretsTargetBosses_HookFactory<T>() {
		return new ILHook(
			typeof(T).GetMethod("ChooseTarget", BindingFlags.NonPublic | BindingFlags.Instance),
			static (ILContext il) => {
				ILCursor cursor = new ILCursor(il);
				cursor.Index = 0;
				cursor.RemoveRange(8);
			},
			false
		);
	}
}
