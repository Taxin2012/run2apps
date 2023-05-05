# run2apps
Program that runs two apps. Was used for Discord RPC for applications that don't have it.

How to use:
1) Put all files to the root of the needed folder;
2) Edit the config file (run2apps/config.cfg)

Config file must have two paths to .exe files:
First line - main proccess.
Second line - slave proccess, will be immediately closed if the main proccess was closed.

For hiding the proccess window - just add anything after the needed path. (ex.: myApp/app.exe itsAnything)
