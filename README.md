# Phasmophobia MelonLoader Mod

Just an private project to learn some basic programming of Unreal Engine Mods with the help of MelonLoader.  
Place the generated DLL into the MelonLoader Mods folder.   


**Features**  
\- Simple Box ESP  
\- Fullbright Mode  
\- Basic Ghost Informations (Ghost Name/Type/State, responds to)  
\- Revealed Evidence  
\- Own Insanity  
\- Active and completed Missions  
\- Console Window for Logging (MelonLoader)


**Hotkeys**  
Up Arrow = ESP  
Down Arrow = Fullbright  
Left Arrow = Information Box  
Right Arrow = Trolling UI  
Insert = Cheat UI  
Delete = Enable debug output


**Screenshots**  
\- [Mod v.1](Images/v1.png)  
\- [Mod v.2](Images/v2.png)  
\- [Mod v.3](Images/v3.png)  
\- [Mod v.4](Images/v4.png)


# How to build
1. [Install MelonLoader](https://melonwiki.xyz/#/README)
2. Start the game without Mod, only with MelonLoader. ML will download stuff do handle the IL2CPP from Phasmophobia
3. Modify `PhasmoMelonMod.csproj` and edit all  
```<Reference Include="Assembly-CSharp"><HintPath>**.dll</HintPath></Reference>```  
to link to your Steams Phasmophobia directory
4. Compile (Release) and move the `obj\Release\PhasmoMelonMod.dll` to the Mod folder inside your Phasmophobia directory



# Credits
**Fullbright:** *ShieldSupporterfor* sharing the code from *Plagues*  
**2D Box ESP:** *EBro912*
