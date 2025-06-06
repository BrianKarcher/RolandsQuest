using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using tk2d;

namespace tk2dEditor2.SpriteCollectionEditor
{
	public class SettingsView	
	{
		public bool show = false;
		Vector2 settingsScrollbar = Vector2.zero;
		int[] padAmountValues = null;
		string[] padAmountLabels = null;
		
		IEditorHost host;
		public SettingsView(IEditorHost host)
		{
			this.host = host;
		}
		
		SpriteCollectionProxy SpriteCollection { get { return host.SpriteCollection; } }
		
		Material DuplicateMaterial(Material source)
		{
			string sourcePath = AssetDatabase.GetAssetPath(source);
			string targetPath = AssetDatabase.GenerateUniqueAssetPath(sourcePath);
			AssetDatabase.CopyAsset(sourcePath, targetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			return AssetDatabase.LoadAssetAtPath(targetPath, typeof(Material)) as Material;
		}

		void DrawMaterialEditor()
		{
			// Upgrade
			int numAltMaterials = 0;
			foreach (var v in SpriteCollection.altMaterials)
				if (v != null) numAltMaterials++;

			if ((SpriteCollection.altMaterials.Length == 0 || numAltMaterials == 0) && SpriteCollection.atlasMaterials.Length != 0)
				SpriteCollection.altMaterials = new Material[1] { SpriteCollection.atlasMaterials[0] };
			
			if (SpriteCollection.altMaterials.Length > 0)
			{
				GUILayout.BeginHorizontal();
				DrawHeaderLabel("Materials");
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("+", EditorStyles.miniButton))
				{
					int sourceIndex = -1;
					int i;
					for (i = 0; i < SpriteCollection.altMaterials.Length; ++i)
					{
						if (SpriteCollection.altMaterials[i] != null)
						{
							sourceIndex = i;
							break;
						}
					}
					for (i = 0; i < SpriteCollection.altMaterials.Length; ++i)
					{
						if (SpriteCollection.altMaterials[i] == null)
							break;
					}
					if (i == SpriteCollection.altMaterials.Length)
						System.Array.Resize(ref SpriteCollection.altMaterials, SpriteCollection.altMaterials.Length + 1);
					
					Material mtl = null;
					if (sourceIndex == -1)
					{
						Debug.LogError("Sprite collection has null materials. Fix this in the debug inspector.");
					}
					else
					{
						mtl = DuplicateMaterial(SpriteCollection.altMaterials[sourceIndex]);
					}
					
					SpriteCollection.altMaterials[i] = mtl;
					SpriteCollection.Trim();

					if (SpriteCollection.platforms.Count > 1)
					{
						SpriteCollection.platforms[0].spriteCollection.altMaterials = SpriteCollection.altMaterials;
						EditorUtility.SetDirty(SpriteCollection.platforms[0].spriteCollection);

						for (int j = 1; j < SpriteCollection.platforms.Count; ++j)
						{
							if (!SpriteCollection.platforms[j].Valid) continue;
							tk2dSpriteCollection data = SpriteCollection.platforms[j].spriteCollection;
							System.Array.Resize(ref data.altMaterials, SpriteCollection.altMaterials.Length);
							data.altMaterials[i] = DuplicateMaterial(data.altMaterials[sourceIndex]);
							EditorUtility.SetDirty(data);
						}
					}

					host.Commit();
				}
				GUILayout.EndHorizontal();

				if (SpriteCollection.altMaterials != null)
				{
					EditorGUI.indentLevel++;
					
					for (int i = 0; i < SpriteCollection.altMaterials.Length; ++i)
					{
						if (SpriteCollection.altMaterials[i] == null)
							continue;
						
						bool deleteMaterial = false;
						
						Material newMaterial = EditorGUILayout.ObjectField(SpriteCollection.altMaterials[i], typeof(Material), false) as Material;
						if (newMaterial == null)
						{
							// Can't delete the last one
							if (numAltMaterials > 1)
							{
								bool inUse = false;
								foreach (var v in SpriteCollection.textureParams)
								{
									if (v.materialId == i)
									{
										inUse = true;
										break;
									}
								}
								foreach (var v in SpriteCollection.fonts)
								{
									if (v.materialId == i)
									{
										inUse = true;
										break;
									}
								}
								
								if (inUse)
								{
									if (EditorUtility.DisplayDialog("Delete material", 
										"This material is in use. Deleting it will reset materials on " +
										"sprites that use this material.\n" +
										"Do you wish to proceed?", "Yes", "Cancel"))
									{
										deleteMaterial = true;
									}
								}
								else
								{
									deleteMaterial = true;
								}
							}
						}
						else
						{
							SpriteCollection.altMaterials[i] = newMaterial;
						}
						
						if (deleteMaterial)
						{
							SpriteCollection.altMaterials[i] = null;

							// fix up all existing materials
							int targetMaterialId;
							for (targetMaterialId = 0; targetMaterialId < SpriteCollection.altMaterials.Length; ++targetMaterialId)
								if (SpriteCollection.altMaterials[targetMaterialId] != null)
									break;
							foreach (var sprite in SpriteCollection.textureParams)
							{
								if (sprite.materialId == i)
									sprite.materialId = targetMaterialId;
							}
							foreach (var font in SpriteCollection.fonts)
							{
								if (font.materialId == i)
									font.materialId = targetMaterialId;
							}
							SpriteCollection.Trim();
							
							// Do the same on inherited sprite collections
							for (int j = 0; j < SpriteCollection.platforms.Count; ++j)
							{
								if (!SpriteCollection.platforms[j].Valid) continue;
								tk2dSpriteCollection data = SpriteCollection.platforms[j].spriteCollection;
								data.altMaterials[i] = null;
								
								for (int lastIndex = data.altMaterials.Length - 1; lastIndex >= 0; --lastIndex)
								{
									if (data.altMaterials[lastIndex] != null)
									{
										int count = data.altMaterials.Length - 1 - lastIndex;
										if (count > 0)
											System.Array.Resize(ref data.altMaterials, lastIndex + 1);
										break;
									}
								}
								
								EditorUtility.SetDirty(data);
							}
							
							host.Commit();
						}
					}
										
					EditorGUI.indentLevel--;
				}
			}			
		}

		void DrawHeaderLabel(string name)
		{
			GUILayout.Label(name, EditorStyles.boldLabel);
		}

		void BeginHeader(string name)
		{
			DrawHeaderLabel(name);
			GUILayout.Space(2);
			EditorGUI.indentLevel++;
		}

		void EndHeader()
		{
			EditorGUI.indentLevel--;
			GUILayout.Space(8);
		}

		void DrawSystemSettings()
		{
			BeginHeader("System");

			// Loadable
			bool allowSwitch = SpriteCollection.spriteCollection != null;
			bool loadable = SpriteCollection.spriteCollection?SpriteCollection.loadable:false;
			bool newLoadable = EditorGUILayout.Toggle("Loadable asset", loadable);
			if (newLoadable != loadable)
			{
				if (!allowSwitch)
				{
					EditorUtility.DisplayDialog("Please commit the sprite collection before attempting to make it loadable.", "Make loadable.", "Ok");
				}
				else
				{
					if (newLoadable) 
					{
						if (SpriteCollection.assetName.Length == 0)
							SpriteCollection.assetName = SpriteCollection.spriteCollection.spriteCollectionName; // guess something
						tk2dSystemUtility.MakeLoadableAsset(SpriteCollection.spriteCollection, SpriteCollection.assetName);
					}
					else 
					{
						if (tk2dSystemUtility.IsLoadableAsset(SpriteCollection.spriteCollection))
							tk2dSystemUtility.UnmakeLoadableAsset(SpriteCollection.spriteCollection);
					}
					loadable = newLoadable;
					SpriteCollection.loadable = loadable;
				}
			}
			if (loadable)
			{
				SpriteCollection.assetName = EditorGUILayout.TextField("Asset Name/Path", SpriteCollection.assetName);
			}

			// Clear data
			GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("References");
			if (GUILayout.Button("Clear References", EditorStyles.miniButton))
			{
				if (EditorUtility.DisplayDialog("Clear references",
												"Clearing references will clear references to data (atlases, materials) owned by this sprite collection. " +
												"This will only remove references, and will not delete the data or textures. " + 
												"Use after duplicating a sprite collection to sever links with the original.\n\n" +
												"Are you sure you want to do this?"
												, "Yes", "No"))
				{
					SpriteCollection.ClearReferences();

					foreach (tk2dSpriteCollectionPlatform plat in SpriteCollection.platforms)
						plat.spriteCollection = null;
				}
			}
			GUILayout.EndHorizontal();
			
			EndHeader();
		}

		string linkedCollectionName = "";

		void DrawLinked() {
			BeginHeader("Linked Collections");

			if (!SpriteCollection.disableTrimming) {
				EditorGUILayout.HelpBox("Linked collections are only supported when trimming is disabled.", SpriteCollection.linkedSpriteCollections.Count == 0 ? MessageType.Info : MessageType.Error);
				return;
			}

			GUILayout.BeginHorizontal();
			linkedCollectionName = EditorGUILayout.TextField(linkedCollectionName, GUILayout.ExpandWidth(true));
			GUI.enabled = linkedCollectionName.Trim().Length > 0;
			if (GUILayout.Button("Add")) {
				bool allowAdd = true;
				foreach (var v in SpriteCollection.linkedSpriteCollections) {
					if (v.name == linkedCollectionName) {
						allowAdd = false;
						break;
					}
				}
				if (!allowAdd) {
					EditorUtility.DisplayDialog("Add Linked Collection", "Unable to add linked collection", "Ok");
				}
				else {
					tk2dLinkedSpriteCollection s = new tk2dLinkedSpriteCollection();
					s.name = linkedCollectionName;
					linkedCollectionName = "";
					SpriteCollection.linkedSpriteCollections.Add(s);
				}
			}
			GUI.enabled = true;
			GUILayout.EndHorizontal();

			int toDelete = -1;
			for (int i = 0; i < SpriteCollection.linkedSpriteCollections.Count; ++i) {
				tk2dLinkedSpriteCollection lsc = SpriteCollection.linkedSpriteCollections[i];
				GUILayout.BeginHorizontal();
				GUILayout.Label(lsc.name);
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("X", EditorStyles.miniButton)) {
					toDelete = i;
				}
				GUILayout.EndHorizontal();
			}

			if (toDelete != -1) {
				SpriteCollection.linkedSpriteCollections.RemoveAt(toDelete);
			}
		}		

		void DrawPlatforms()
		{
			// Asset Platform
			BeginHeader("Platforms");
			tk2dSystem system = tk2dSystem.inst_NoCreate;

			if (system == null && GUILayout.Button("Add Platform Support"))
				system = tk2dSystem.inst; // force creation

			if (system)
			{
				int toDelete = -1;
				for (int i = 0; i < SpriteCollection.platforms.Count; ++i)
				{
					tk2dSpriteCollectionPlatform currentPlatform = SpriteCollection.platforms[i];

					GUILayout.BeginHorizontal();
					string label = (i==0)?"Current platform":"Platform";
					currentPlatform.name = tk2dGuiUtility.PlatformPopup(system, label, currentPlatform.name);
					bool displayDelete = ((SpriteCollection.platforms.Count == 1 && SpriteCollection.platforms[0].name.Length > 0) || 
										  (SpriteCollection.platforms.Count > 1 && i > 0));
					if (displayDelete && GUILayout.Button("Delete", EditorStyles.miniButton, GUILayout.MaxWidth(50))) toDelete = i;
					GUILayout.EndHorizontal();
				}

				if (toDelete != -1)
				{
					tk2dSpriteCollection deletedSpriteCollection = null;
					if (SpriteCollection.platforms.Count == 1)
					{
						if (SpriteCollection.platforms[0].spriteCollection != null && SpriteCollection.platforms[0].spriteCollection.spriteCollection != null)
							deletedSpriteCollection = SpriteCollection.platforms[0].spriteCollection;
						SpriteCollection.platforms[0].name = "";
						SpriteCollection.platforms[0].spriteCollection = null;
					}
					else
					{
						if (SpriteCollection.platforms[toDelete].spriteCollection != null && SpriteCollection.platforms[toDelete].spriteCollection.spriteCollection != null)
							deletedSpriteCollection = SpriteCollection.platforms[toDelete].spriteCollection;
						SpriteCollection.platforms.RemoveAt(toDelete);
					}
					if (deletedSpriteCollection != null)
					{
						foreach (tk2dSpriteCollectionFont f in deletedSpriteCollection.fonts)
							tk2dSystemUtility.UnmakeLoadableAsset(f.data);	
						tk2dSystemUtility.UnmakeLoadableAsset(deletedSpriteCollection.spriteCollection);
					}
				}

				if (SpriteCollection.platforms.Count > 1 ||
					(SpriteCollection.platforms.Count == 1 && SpriteCollection.platforms[0].name.Length > 0))
				{
					GUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel(" ");
					if (GUILayout.Button("Add new platform", EditorStyles.miniButton))
						SpriteCollection.platforms.Add(new tk2dSpriteCollectionPlatform());
					GUILayout.EndHorizontal();
				}
			}

			EndHeader();
		}

		void DrawTextureSettings()
		{
			BeginHeader("Texture Settings");

			SpriteCollection.atlasFormat = (tk2dSpriteCollection.AtlasFormat)EditorGUILayout.EnumPopup("Atlas Format", SpriteCollection.atlasFormat);
			if (SpriteCollection.atlasFormat == tk2dSpriteCollection.AtlasFormat.UnityTexture) {
				SpriteCollection.filterMode = (FilterMode)EditorGUILayout.EnumPopup("Filter Mode", SpriteCollection.filterMode);
				SpriteCollection.textureCompression = (tk2dSpriteCollection.TextureCompression)EditorGUILayout.EnumPopup("Compression", SpriteCollection.textureCompression);
				SpriteCollection.userDefinedTextureSettings = EditorGUILayout.Toggle("User Defined", SpriteCollection.userDefinedTextureSettings);
				if (SpriteCollection.userDefinedTextureSettings) GUI.enabled = false;
				EditorGUI.indentLevel++;
				SpriteCollection.wrapMode = (TextureWrapMode)EditorGUILayout.EnumPopup("Wrap Mode", SpriteCollection.wrapMode);
				SpriteCollection.anisoLevel = (int)EditorGUILayout.IntSlider("Aniso Level", SpriteCollection.anisoLevel, 0, 9);
				SpriteCollection.mipmapEnabled = EditorGUILayout.Toggle("Mip Maps", SpriteCollection.mipmapEnabled);
				EditorGUI.indentLevel--;
				GUI.enabled = true;
			}
			else if (SpriteCollection.atlasFormat == tk2dSpriteCollection.AtlasFormat.Png) {
				tk2dGuiUtility.InfoBox("Png atlases will decrease on disk game asset sizes, at the expense of increased load times.",
					tk2dGuiUtility.WarningLevel.Warning);
				SpriteCollection.textureCompression = (tk2dSpriteCollection.TextureCompression)EditorGUILayout.EnumPopup("Compression", SpriteCollection.textureCompression);
				SpriteCollection.filterMode = (FilterMode)EditorGUILayout.EnumPopup("Filter Mode", SpriteCollection.filterMode);
				SpriteCollection.mipmapEnabled = EditorGUILayout.Toggle("Mip Maps", SpriteCollection.mipmapEnabled);
			}

			int curRescaleSelection = 0;
			if (SpriteCollection.globalTextureRescale > 0.4 && SpriteCollection.globalTextureRescale < 0.6)
				curRescaleSelection = 1;
			if (SpriteCollection.globalTextureRescale > 0.2 && SpriteCollection.globalTextureRescale < 0.3)
				curRescaleSelection = 2;
			int newRescaleSelection = EditorGUILayout.Popup("Rescale", curRescaleSelection, new string[] {"1", "0.5", "0.25"});
			switch (newRescaleSelection) {
				case 0: SpriteCollection.globalTextureRescale = 1.0f; break;
				case 1: SpriteCollection.globalTextureRescale = 0.5f; break;
				case 2: SpriteCollection.globalTextureRescale = 0.25f; break;
			}

			EndHeader();
		}

		void DrawSpriteCollectionSettings()
		{
			BeginHeader("Sprite Collection Settings");

			tk2dGuiUtility.SpriteCollectionSize( SpriteCollection.sizeDef );

			GUILayout.Space(4);

			SpriteCollection.padAmount = EditorGUILayout.IntPopup("Pad Amount", SpriteCollection.padAmount, padAmountLabels, padAmountValues);
			if (SpriteCollection.padAmount == 0 && SpriteCollection.filterMode != FilterMode.Point)
			{
				tk2dGuiUtility.InfoBox("Filter mode is not set to Point." + 
					" Some bleeding will occur at sprite edges.", 
					tk2dGuiUtility.WarningLevel.Info);
			}

			SpriteCollection.premultipliedAlpha = EditorGUILayout.Toggle("Premultiplied Alpha", SpriteCollection.premultipliedAlpha);
			SpriteCollection.disableTrimming = EditorGUILayout.Toggle("Disable Trimming", SpriteCollection.disableTrimming);
			GUIContent gc = new GUIContent("Disable rotation", "Disable rotation of sprites in atlas. Use this if you need consistent UV direction for shader special effects.");
			SpriteCollection.disableRotation = EditorGUILayout.Toggle(gc, SpriteCollection.disableRotation);
			SpriteCollection.normalGenerationMode = (tk2dSpriteCollection.NormalGenerationMode)EditorGUILayout.EnumPopup("Normal Generation", SpriteCollection.normalGenerationMode);

			EndHeader();
		}
		
		void DrawPhysicsSettings()
		{
			BeginHeader("Physics Settings");

#if (UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
			GUI.enabled = false;
			SpriteCollection.physicsEngine = tk2dSpriteDefinition.PhysicsEngine.Physics3D;
#endif
			SpriteCollection.physicsEngine = (tk2dSpriteDefinition.PhysicsEngine)EditorGUILayout.EnumPopup("Physics Engine", SpriteCollection.physicsEngine);
#if (UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
			GUI.enabled = false;
#endif
			GUI.enabled = SpriteCollection.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics3D;
			SpriteCollection.physicsDepth = EditorGUILayout.FloatField("Collider depth", SpriteCollection.physicsDepth);
			GUI.enabled = true;

			EndHeader();
		}
		
		void DrawAtlasSettings()
		{
			BeginHeader("Atlas Settings");

			int[] allowedAtlasSizes = { 64, 128, 256, 512, 1024, 2048, 4096 };
			string[] allowedAtlasSizesString = new string[allowedAtlasSizes.Length];
			for (int i = 0; i < allowedAtlasSizes.Length; ++i)
				allowedAtlasSizesString[i] = allowedAtlasSizes[i].ToString();

			SpriteCollection.forceTextureSize = EditorGUILayout.Toggle("Force Atlas Size", SpriteCollection.forceTextureSize);
			EditorGUI.indentLevel++;
			if (SpriteCollection.forceTextureSize)
			{
				SpriteCollection.forcedTextureWidth = EditorGUILayout.IntPopup("Width", SpriteCollection.forcedTextureWidth, allowedAtlasSizesString, allowedAtlasSizes);
				SpriteCollection.forcedTextureHeight = EditorGUILayout.IntPopup("Height", SpriteCollection.forcedTextureHeight, allowedAtlasSizesString, allowedAtlasSizes);
			}
			else
			{
				SpriteCollection.maxTextureSize = EditorGUILayout.IntPopup("Max Size", SpriteCollection.maxTextureSize, allowedAtlasSizesString, allowedAtlasSizes);
				SpriteCollection.forceSquareAtlas = EditorGUILayout.Toggle("Force Square", SpriteCollection.forceSquareAtlas);
			}
			EditorGUI.indentLevel--;
			
			bool allowMultipleAtlases = EditorGUILayout.Toggle("Multiple Atlases", SpriteCollection.allowMultipleAtlases);
			if (allowMultipleAtlases != SpriteCollection.allowMultipleAtlases)
			{
				// Disallow switching if using unsupported features
				if (allowMultipleAtlases == true)
				{
					bool hasDicing = false;
					for (int i = 0; i < SpriteCollection.textureParams.Count; ++i)
					{
						if (SpriteCollection.textureParams[i].texture != null &
							SpriteCollection.textureParams[i].dice)
						{
							hasDicing = true;
							break;
						}
					}
					
					if (SpriteCollection.fonts.Count > 0 || hasDicing)
					{
						EditorUtility.DisplayDialog("Multiple atlases", 
									"Multiple atlases not allowed. This sprite collection contains fonts and/or " +
									"contains diced sprites.", "Ok");
						allowMultipleAtlases = false;
					}
				}
				
				SpriteCollection.allowMultipleAtlases = allowMultipleAtlases;
			}

			if (SpriteCollection.allowMultipleAtlases)
			{
				tk2dGuiUtility.InfoBox("Sprite collections with multiple atlas spanning enabled cannot be used with the Static Sprite" +
					" Batcher, Fonts, the TileMap Editor and doesn't support Sprite Dicing and material level optimizations.\n\n" +
					"Avoid using it unless you are simply importing a" +
					" large sequence of sprites for an animation.", tk2dGuiUtility.WarningLevel.Info);
			}
			
			if (SpriteCollection.allowMultipleAtlases)
			{
				EditorGUILayout.LabelField("Num Atlases", SpriteCollection.atlasTextures.Length.ToString());
			}
			else
			{
				EditorGUILayout.LabelField("Atlas Width", SpriteCollection.atlasWidth.ToString());
				EditorGUILayout.LabelField("Atlas Height", SpriteCollection.atlasHeight.ToString());
				EditorGUILayout.LabelField("Atlas Wastage", SpriteCollection.atlasWastage.ToString("0.00") + "%");
			}

			if (SpriteCollection.atlasFormat == tk2dSpriteCollection.AtlasFormat.Png) {
				int totalAtlasSize = 0;
				foreach (TextAsset ta in SpriteCollection.atlasTextureFiles) {
					if (ta != null) {
						totalAtlasSize += ta.bytes.Length;
					}
				}
				EditorGUILayout.LabelField("Atlas File Size", EditorUtility.FormatBytes(totalAtlasSize));
			}

			GUIContent remDuplicates = new GUIContent("Remove Duplicates", "Remove duplicate textures after trimming and other processing.");
			SpriteCollection.removeDuplicates = EditorGUILayout.Toggle(remDuplicates, SpriteCollection.removeDuplicates);

			EndHeader();
		}
		
		public void Draw()
		{
			if (SpriteCollection == null)
				return;
			
			// initialize internal stuff
			if (padAmountValues == null || padAmountValues.Length == 0)
			{
				int MAX_PAD_AMOUNT = 18;
				padAmountValues = new int[MAX_PAD_AMOUNT];
				padAmountLabels = new string[MAX_PAD_AMOUNT];
				for (int i = 0; i < MAX_PAD_AMOUNT; ++i)
				{
					padAmountValues[i] = -1 + i;
					padAmountLabels[i] = (i==0)?"Default":((i-1).ToString());
				}
			}
	
			GUILayout.BeginHorizontal();
			
			GUILayout.BeginVertical(tk2dEditorSkin.SC_BodyBackground, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
			GUILayout.EndVertical();

			Rect rect = GUILayoutUtility.GetLastRect();
			tk2dGrid.Draw(rect);
			
			
			int inspectorWidth = host.InspectorWidth;
			tk2dGuiUtility.LookLikeControls(130.0f, 40.0f);
			
			settingsScrollbar = GUILayout.BeginScrollView(settingsScrollbar, GUILayout.ExpandHeight(true), GUILayout.Width(inspectorWidth));
	
			GUILayout.BeginVertical(tk2dEditorSkin.SC_InspectorHeaderBG, GUILayout.ExpandWidth(true));
			GUILayout.Label("Settings", EditorStyles.largeLabel);
			SpriteCollection.spriteCollection = EditorGUILayout.ObjectField("Data object", SpriteCollection.spriteCollection, typeof(tk2dSpriteCollectionData), false) as tk2dSpriteCollectionData;
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical(tk2dEditorSkin.SC_InspectorBG, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

			DrawSpriteCollectionSettings();

			DrawPhysicsSettings();

			DrawTextureSettings();

			DrawAtlasSettings();

			DrawSystemSettings();

			DrawLinked();

			DrawPlatforms();
			
			// Materials
			if (!SpriteCollection.allowMultipleAtlases)
			{
				DrawMaterialEditor();
			}

			GUILayout.Space(8);
			
			GUILayout.EndVertical();
			GUILayout.EndScrollView();

			// make dragable
			tk2dPreferences.inst.spriteCollectionInspectorWidth -= (int)tk2dGuiUtility.DragableHandle(4819284, GUILayoutUtility.GetLastRect(), 0, tk2dGuiUtility.DragDirection.Horizontal);

			GUILayout.EndHorizontal();
		}		
	}
}
