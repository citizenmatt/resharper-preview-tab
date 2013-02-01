# resharper-preview-tab

ReSharper 7.0 added support for Visual Studio 2012's preview tab. ReSharper 7.1 took it away. This plugin brings it all back.

## What does it do? ##

Visual Studio 2012's preview tab is a reusable editor tab. Single-clicking files in solution explorer open in the preview tab, replacing what was already there. Editing the file automatically promotes it to a normal editor tab, keeping the file open.

This is a very useful feature for browsing code, since you can navigate through lots of files without cluttering your currently open set, allowing you to concentrate on the code you're editing, rather than getting lost in a sea of unrelated files.

ReSharper 7.0 supported the preview tab. All navigation (ctrl-click, go to type/file/symbol, find usages, etc) would open the file in the preview tab. Perfect.

ReSharper 7.1 removed the support. For various reasons (see below if you're interested). The upshot is that ReSharper 7.1 no longer uses the preview tab. Some (i.e. me) view this as a backwards step.

Fortunately, this plugin adds the support back in. Simply install it and now, whenever ReSharper needs to open a file, it will open it in the preview tab. So all navigation again goes straight to the preview tab. Happy days.

## How do I get it? ##

If you wish to just install a copy of the plugins without building yourself:

- Download the latest zip file: [resharper-preview-tab.1.0.zip](http://download.jetbrains.com/resharper/plugins/resharper-preview-tab.1.0.zip)
- Extract everything
- Run Install-PreviewTab.7.1.bat file

## Why was support removed? ##

Good question. Not everyone liked the preview tab. There were two main arugments.

First, it was inconsistent with Visual Studio, and therefore confusing. A single click from solution explorer opened the file in the preview tab, and a double click opened the file normally. In the ReShaprer tool windows, a single click displayed a preview in the preview pane of the tool window, and a double click opened the file in the preview tab. 

I think whether this is an issue depends on if you have the ReSharper tool window docked, or floating as a document window. If it's docked, like solution explorer, then opening in the preview tab when single-clicking makes sense. As a document window, it doesn't, because single-clicking would either a) open the preview tab in the background, which isn't much help, or b) immediately swap focus to the preview tab, which would be massively annoying. Single-clicking can display the code in a preview pane, but that has a different purpose to the preview tab - it displays just the code under focus, and doesn't allow editing, refactoring or navigation.

The second argument was that navigating to a type or symbol implied you were going to edit it, so it should open in a normal editor tab. I don't buy this at all - if you're going to edit a file in the preview tab, just start typing, and the file is seamlessly moved to a normal tab. It doesn't matter whether it was originally open in the preview tab or a normal tab.

What I don't like about this change is that it affects everybody. In 7.0, if you didn't like the preview tab behaviour, you could disable the preview tab in Visual Studio's settings, and ReSharper would open everything in normal editor tabs, and you get the behaviour you want. But in 7.1, I can't change the behaviour, and the preview tab feature is effectively removed.

## Building ##

To build the source, you need the [ReSharper 7.1 SDK](http://www.jetbrains.com/resharper/download/index.html) installed. Then just open the src\resharper-preview-tab.sln file and build.

## Version

The current version is 1.0. It only works with Visual Studio 2012 and ReSharper 7.1.1. See the [ReleaseNotes wiki page](https://github.com/citizenmatt/resharper-nuget/wiki/Release-Notes) for more details.

## Contributing ##

Feel free to [raise issues on GitHub](https://github.com/citizenmatt/resharper-preview-tab/issues), or [fork the project](http://help.github.com/fork-a-repo/) and [send a pull request](http://help.github.com/send-pull-requests/).

## Roadmap

Please see the [Issues tab on the GitHub page](https://github.com/citizenmatt/resharper-preview-tab/issues).




