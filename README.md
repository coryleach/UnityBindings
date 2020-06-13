<p align="center">
<img align="center" src="https://raw.githubusercontent.com/coryleach/UnityPackages/master/Documentation/GameframeFace.gif" />
</p>
<h1 align="center">Gameframe.Bindings 👋</h1>

<!-- BADGE-START -->
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/0bb60e11b3c9422d94c523c43bce4713)](https://www.codacy.com/manual/coryleach/UnityBindings?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=coryleach/UnityBindings&amp;utm_campaign=Badge_Grade)
![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/coryleach/UnityBindings?include_prereleases)
[![openupm](https://img.shields.io/npm/v/com.gameframe.bindings?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.gameframe.bindings/)
[![license](https://img.shields.io/github/license/coryleach/UnityBindings)](https://github.com/coryleach/UnityBindings/blob/master/LICENSE)

[![twitter](https://img.shields.io/twitter/follow/coryleach.svg?style=social)](https://twitter.com/coryleach)
<!-- BADGE-END -->

This is a library of binding components that allow you to quickly wire data sources to target properties via the inspector.    
    
Binding changes propagate via the System.ComponentModel.INotifyPropertyChanged interface but the included binding components will also refresh their target properties in OnEnable.    
A general purpose ComponentBinding monobehaviour is included to wire any two UnityEngine.Objects together as well as a TextBinding for quick and simple binding to text fields.  


## Quick Package Install

#### Using UnityPackageManager (for Unity 2019.3 or later)
Open the package manager window (menu: Window > Package Manager)<br/>
Select "Add package from git URL...", fill in the pop-up with the following link:<br/>
https://github.com/coryleach/UnityBindings.git#1.0.3<br/>

#### Using UnityPackageManager (for Unity 2019.1 or later)

Find the manifest.json file in the Packages folder of your project and edit it to look like this:
```js
{
  "dependencies": {
    "com.gameframe.bindings": "https://github.com/coryleach/UnityBindings.git#1.0.3",
    ...
  },
}
```

<!-- DOC-START -->
<!-- 
Changes between 'DOC START' and 'DOC END' will not be modified by readme update scripts
-->

## Usage

For an example of how the components are used import the package included in the Demo folder after installing this package.

```C#
//Bindings can be created in code as follows
//Currently all bindings are one way from source to target
var binding = new Binding();

//If your source implements INotifyPropertyChanged correctly changes will automatically propagate
binding.SetSource(myData,"SourcePropertyName");
binding.SetTarget(myView,"TargetPropertyName");

//You can also force a refresh (unnecessary if source implements INotifyPropertyChange)
binding.Refresh();

//Temporarily disable bindings
binding.Enabled = false;

//Set a custom converter to transform source values before they are passed to the target
binding.Converter = (x) => x.ToString();

//Destroy the Binding
binding.Dispose(); 
```

<!-- DOC-END -->

## Author

👤 **Cory Leach**

* Twitter: [@coryleach](https://twitter.com/coryleach)
* Github: [@coryleach](https://github.com/coryleach)


## Show your support

Give a ⭐️ if this project helped you!

***
_This README was generated with ❤️ by [Gameframe.Packages](https://github.com/coryleach/unitypackages)_
