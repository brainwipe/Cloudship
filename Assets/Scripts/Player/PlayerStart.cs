using System;
using UnityEngine;

public static class PlayerStart
{
	public static void SetupCloudship(Cloudship player, BuildMenu buildMenu)
	{
		player.Stores.AddFlotsam(270);
		AddBuildings(player, buildMenu);
	}

    private static void AddBuildings(Cloudship player, BuildMenu buildMenu)
    {
        var buildSurface = player.GetComponentInChildren<BuildSurface>();
		var menuChimney = buildMenu.FindBuilding("boiler-medium");
		var menuBridge = buildMenu.FindBuilding("bridge-standard");

		var chimney = menuChimney.Clone(buildSurface.transform);
		var bridge = menuBridge.Clone(buildSurface.transform);
		bridge.transform.localPosition = new Vector3(
			bridge.transform.localPosition.x, 
			bridge.transform.localPosition.y, 
			bridge.transform.localPosition.z + 29f);

		chimney.UnSelected();
		bridge.UnSelected();
		player.UpdateAbilities();
    }
}
