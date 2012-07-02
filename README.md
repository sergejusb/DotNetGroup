# DotNetGroup.lt

DotNetGroup.lt is a social aggregation platform. Our moto is - all development news, links and posts on the single page.

# Dependencies

Before anything else, you should get these dependencies installed:

[ASP.NET MVC 3](http://www.asp.net/mvc/mvc3)
[NuGet](http://docs.nuget.org/docs/start-here/installing-nuget)
[MongoDB](http://www.mongodb.org/display/DOCS/Quickstart+Windows)

Next, to get the project running, go through these steps:

1.  Run IISSetup.ps1. If you're getting errors, try:

    * Running PowerShell as Administrator.

    * Running `get-module -listAvailable | import-module` in PowerShell.

    * Running `Set-ExecutionPolicy 'Unrestricted'` in PowerShell.

    * Using 64bit PowerShell (C:\Windows\system32\WindowsPowerShell\v1.0\powershell.exe) on 64bit machines.

2.  Add read security permissions for IUSR and IIS_IUSRS users on DotNetGroup folder.

3.  Run Host application from solution. It populates MongoDB with latest data. You don't have to keep it running all the time, once for the initial data dump will do.

Now you should be able to navigate to dotnetgroup.dev. If you can't, [contact us](https://github.com/sergejusb/DotNetGroup/issues/new), we'll try to help!

# How to Contribute

You can find roadmap [here](https://gist.github.com/1330485).

# Contributors

DotNetGroup.lt is not beeing built by [single code ninja](https://github.com/sergejusb). People that helped:

[Giedrius Banaitis](https://github.com/dziedrius)

[Mindaugas Mozūras](https://github.com/mmozuras)

## Copyright

Copyright © 2012 Sergejus Barinovas and contributors

## License

DotNetGroup is licensed under [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form"). Refer to [license.txt](https://github.com/sergejusb/DotNetGroup/blob/master/license.txt) for more information.
