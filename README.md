# resharper-preview-tab

An extension to make ReSharper 9.1 open files in Visual Studio's preview tab when navigating.

## What does it do? ##

Visual Studio 2012 introduced the preview tab - a reusable editor tab. Single-clicking a file in solution explorer opens the file in the preview tab, replacing what was already there. Editing the file automatically and seamlessly promotes it to a normal editor tab, keeping the file open.

This is a very useful feature for browsing code, since you can navigate through lots of files without cluttering your currently open set, allowing you to concentrate on the code you're editing, rather than getting lost in a sea of unrelated files, before finally declaring editor tab bankruptcy.

This plugin changes ReSharper's navigation (ctrl-click, go to type/symbol/file, find usages, etc.) to open files in the preview tab. Since ReSharper makes it so easy to navigate to lots of files, this greatly reduces the amount of unnecessary open windows in your Visual Studio.

Since this reduces the number of open windows in your Visual Studio session, it does make it harder to ctrl-tab to a recently opened file. Instead, you need to use Visual Studio's "navigate back" and "navigate forward" shortcuts (ctrl+'-' and shift+ctrl+'-') or ReSharper's "recent files" feature (ctrl+'E' for IntelliJ users and ctrl+',') to navigate to files previously opened in the preview tab. It's also useful to remember the ctrl+alt+home keyboard shortcut - this promotes the preview tab to a full editor tab without editing the file. You can also click the little tab icon in the preview tab to do the same. 

## How do I get it? ##

Simply install from the Extension Manager, available from the ReSharper menu.

### Building ###

The source contains projects for ReSharper 9.1. The SDKs are referenced as NuGet packages, and will automatically restore on first build.

### Version ###

The current version is 1.2.0. It works with Visual Studio 2012, 2013 and 2015, and ReSharper 9.1. Previous versions are available for ReSharper 9.0 and 8.x in the Extension Manager.

### Contributing ###

Feel free to [raise issues on GitHub](https://github.com/citizenmatt/resharper-preview-tab/issues), or [fork the project](http://help.github.com/fork-a-repo/) and [send a pull request](http://help.github.com/send-pull-requests/).
