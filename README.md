# PexeFramework
 Python Executable format framework

 Allows users to package both the requirements.txt and main.py in a zip file that can then be executed by the famework, making packaging and sending Python applications easy.

## How to get started
 1.) Download the latest release and extract the .zip
 2.) Open a terminal in the location of the Framework.exe (you can optinally add it to path as well)
 3.) Run 'PexeFramework.exe -new {CoolName}' and replace {CoolName} with what you want to call the project
 4.) Go into the new folder and edit main.py, and also put your pip requirements in requirements.txt
 Any other files you need such as image files, other .py files, etc can also be added into the folder
 5.) When you have completed your project run 'PexeFramework.exe -pack {FolderName} {PexeName(No .exe)}' to build your .pexe file
 
 To run your new .pexe file do 'PexeFramework.exe {.pexe location}' and the framework will automatically setup python, pip and your requirements in the background
