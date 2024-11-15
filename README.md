# PL-Project 
# Console-Based Text Editor

__Project Explanation__

Console-Based Text Editor Project behaves like a regular Notepad on Microsoft.
Opening, saving, editing, finding and replacing a word and displaying the text are the options that our program will provide.
Program should be simple to maintain and it will be run in the terminal where a user will have the
provided options on how to use Text Editor without the usage of GUI.
It will be written in C#, JavaScript and Python in Visual Studio and Visual Studio Code IDE.

__Goals__

The goals of this project are:

- learning and exploring new programming languages (C#, JavaScript and Python)
- using appropriate sources for coding
- learning how to work with files and their manipulation through built-in functions in C#, JavaScript and Python
- handling mistakes and errors throught project development


__Features__

Code will be connected by a text file with the Notepad so changes made in the terminal
will be visible in the Notepad since this is not a GUI application.


Main features for the project:
- **Open_File**

   Takes a path argument (the location of the file) which will allow the file to be accessed and modified directly in command line. When the file is opened its contents are 
   loaded into the editor.
  
- **Save_File**

  Overwrites the existing file with the current version in the editor. Each time a user edits the file, user will save the changes by using Save_File function.
   
- **Edit_File**

  Allows users to modify the text within the Edit_File function. This includes adding and deleting of words in the text.
  
- **Find_File**

  A search feature that allows users to find specific words within the file. Function has one parameter which is the word that the users are searching for.
  
- **Replace_File**

  Allows users to search for a particular word and replace it with a new one throught the file. The function will have two parameters (old word, new word).

    
- **Display_File**

  Displays the full content of the file directly on the command line.
