# Sphere Generator
Sphere Generator is a free Unity tool for generating sphere meshes procedurally.

![A display of examples of 12 different shapes representing 3 different sphere types (icosphere, cube sphere, UV sphere), each one with 4 examples with different vertex count.](https://blog.matheusamazonas.net/assets/images/post22/sphere_display.png)

## Contents
- [Features](#features)
- [Importing](#importing)
	- [Import using a git URL](#import-using-a-git-url)
	- [Import with OpenUPM](#import-with-openupm)
	- [After importing](#after-importing)
- [Usage](#usage)
  - [Example](#example)
- [Samples](#samples)
- [Compatibility and dependencies](#compatibility-and-dependencies)
- [Contributing](#contributing)
- [Getting help](#getting-help)
- [License](#license)

## Features
- Support for different basic shapes:
  - Icosphere (based on a regular icosahedron).
  - Cube sphere.
  - UV sphere.
- Customizable radius and level of detail (a.k.a. fragmentation depth). 

## Importing
The first step to get started with Sphere Generator is to import the library into your Unity project. There are two ways to do so: via the Package Manager using a git URL, and via OpenUPM.

### Import using a git URL
This approach uses Unity's Package Manager to add Sphere Generator to your project using the repo's git URL. To do so, navigate to `Window > Package Manager` in Unity. Then click on the `+` and select "Add package from git URL":

![](https://ttg.matheusamazonas.net/assets/images/upm_adding.png)

Next, enter the following in the "URL" input field to install the latest version of Sphere Generator:
```
https://github.com/matheusamazonas/sphere_generator.git?path=Assets/Libraries/SphereGenerator
```
Finally, click on the "Add" button. The importing process should start automatically. Once it's done, Sphere Generator is ready to be used in the project. 

### Import with OpenUPM
Sphere Generator is available as a package on [OpenUPM](https://openupm.com/packages/com.lazysquirrellabs.spheregenerator/). To import Sphere Generator into your project via the command line, run the following command:
```
openupm add com.lazysquirrellabs.spheregenerator
```
Once the importing process is complete, Sphere Generator is ready to be used in the project. 

### After importing
After importing , check the [Usage](#usage) section on how to use it and the [Samples](#samples) section on how to import and use the package samples.

## Usage
Using Sphere Generator is straightforward. First, create a `SphereGenerator` (base class) instance by calling one of the supported sphere type constructors:
```csharp
public CubeSphereGenerator(float radius, ushort depth);
public IcosphereGenerator(float radius, ushort depth);
public UVSphereGenerator(float radius, ushort depth);
```
Where `radius` is the sphere radius and `depth` is the fragmentation depth (a.k.a. level of detail) of the to-be generated spheres. Check the documentation for valid parameter value ranges.

Once an instance of the `SphereGenerator` class is created, its `Generate` method can be called to generate a sphere mesh:
```csharp
public Mesh Generate()
```
The method returns a `Mesh` instance that represents the generated sphere. It is up to the user to control the lifetime of the `Mesh` instance.

### Example
Let's say we would like to create an icosphere with radius of 20 units, a fragmentation depth of 3, and use the generated mesh on a mesh filter that is used by a mesh renderer to display the sphere in the scene. The code below implements that inside an `Awake` method:
```csharp
[SerializeField] private MeshFilter _meshFilter;

private void Awake()
{
    SphereGenerator generator = new IcosphereGenerator(20, 3);  
    Mesh mesh = generator.Generate();
    _meshFilter.mesh = mesh;
}
```

## Samples
The package contains one sample named "Display". It displays the generational capabilities of the tool, displaying all supported sphere types with different depths. This sample is better displayed in the Scene view, with "Shading Mode" set to "Shared wireframe", so the vertices and triangles are visible. This sample was also used to generate the image at the top of this page.

To import the samples, open the Package Manager and select Sphere Generator in the packages list. Then find the Samples section on the right panel, and click on the "Import" button right next to the sample you would like to import. Once importing is finished, navigate to the `Assets/Samples/SphereGenerator` folder. Finally, open and play the scene from the sample you would like to test.

## Compatibility and dependencies
Sphere Generator requires Unity 2022.3.X or above, its target API compatibility level is .NET Standard 2.1, and depends on the following packages:
- `com.unity.collections`

## Contributing
If you would like to report e bug, please create an [issue](https://github.com/matheusamazonas/sphere_generator/issues). If you would like to contribute with bug fixing or small improvements, please open a Pull Request. If you would like to contribute with a new feature (regardless if it's in the roadmap or not), [contact the developer](https://matheusamazonas.net/contact.html).  

## Getting help
Use the [issues page](https://github.com/matheusamazonas/sphere_generator/issues) if there's a problem with your Sphere Generator setup, if something isn't working as expected, or if you would like to ask questions about the tool and its usage.

## License
Sphere Generator is distributed under the terms of the MIT license. For more information, check the [LICENSE](LICENSE) file in this repository.
