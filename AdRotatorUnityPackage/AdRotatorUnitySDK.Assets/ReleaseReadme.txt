
1) Update the version number in the SDK documentation:
	- Windows 8 Ad Rotator for Unity SDK.docx
	- Any script modified and packaged in the Unity Package.
2) Build the current solution
3) Start command prompt and go to "AdRotatorUnitySDK.Assets"
4) Execute the CreateRelease.cmd batch file. Argument to provide is the path to the Unity.exe of Unity:
	CreateRelease.cmd "C:\Program Files (x86)\Unity\Editor\Unity.exe"
5) Get the generated Unity Packages (AdRotatorUnitySDK.unitypackage and AdRotatorUnitySDKDemo.unitypackage) from following location:
	AdRotatorUnitySDK.Assets\bin\Debug\AdRotatorUnitySDK\Demo
	or
	AdRotatorUnitySDK.Assets\bin\Release\AdRotatorUnitySDK\Demo

Notes:
- The binary output of the compilation shall not be used. Project compilation is only used to validate basic script compilation.
- AdRotatorBridge.cs.ignore will not be compiled with the project and needs to be tested from an actual Unity Windows Store App (e.g. the Demo) 
- To distribute the SDK as CS file only, without the Unity Packages, refer to "AdRotatorUnitySDK.Assets\PackageRelease.cs" to see which files are required to be provided and put in which folder.
- A new CreateRelease.cmd is generated each time the build on the current solution is done.
