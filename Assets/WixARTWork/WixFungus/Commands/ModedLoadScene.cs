using UnityEngine;
using System;
using System.Collections;

namespace Fungus
{
	[CommandInfo("Flow", 
	             "讀取場景", 
	             "切換場景")]
	[AddComponentMenu("")]
	public class ModedLoadScene : LoadScene {

		public override void OnEnter()
		{
            print(Application.loadedLevelName);
            Game.lastScene = Application.loadedLevelName;
			SceneLoader.LoadScene(sceneName, loadingImage);
		}

		public override string GetSummary()
		{
			if (sceneName.Length == 0)
			{
				return "Error: 未輸入場景名稱";
			}

			return "切換場景至: " + sceneName;
		}
	}

}