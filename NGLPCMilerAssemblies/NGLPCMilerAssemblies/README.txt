PFM 4/2/2015
Had to Reference the NGL.Service.PCMiler64.dll as a file because it wont let me reference the project due to different frameworks 3.5 and 4.5.
The CMDLine server Apps were referencing the wrong file and need to reference the same file in the PCMiler Service Install package which is 
 NGL.Service.PCMiler64.dll.  This file never copied to local while having different frameworks.