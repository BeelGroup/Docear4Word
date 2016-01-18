Add-in Express (TM) 2010 for Microsoft Office and .NET
-------------------------------------------------------------------------------------

CONTENT OF THE REDISTRIBUTABLES FOLDER
-------------------------------------------------------------------------------------
1.  adxloader.dll and adxloader64.dll:
        The adxloader.dll (adxloader64.dll) file is a compiled shim not bound to any
        certain Add-in Express project. It must always be located in the Loader 
        subdirectory of the main Add-in Express project directory. When a project is 
        being built, the loader files are copied to the project's output directory. 
        You can sign the loader with a digital signature and, in this way, create 
        trusted extensions for the target application. For more information see the 
        product documentation.

2.  adxlauncher.exe and adxlauncher2010.exe:
        These files are the heart of the Add-in Express ClickOnce Solution. The 
        Launcher is the application that will be installed on the end-user PC. It 
        is listed in the Start menu and Add Remove Programs.  It provides a dialog 
        form for  the user to register, unregister and update your add-in. It also
        allows the user to switch between two latest versions of your add-in. In 
        general, the Launcher communicates with the ClickOnce API.

3.  adxregistrator.exe:
        The adxregistrator.exe file is the Add-in Registrator for the setup project.
        It works as a custom action to provide ready-to-use Install, Uninstall and 
        Rollback custom actions. For more information see the product documentation.

4.  adxregaddin.exe (deprecated in the current version):
        The adxregaddin.exe file is the Add-in Registrator for the setup project. It
        works as a custom action to provide ready-to-use Install, Uninstall and 
        Rollback custom actions.
5.  Interop Assemblies:
        The directory contains interops assemblies for Microsoft Office applications.

-------------------------------------------------------------------------------------
Copyright (C) Add-in Express Ltd. All rights reserved.
