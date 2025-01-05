using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Reflection.Metadata;


class TextEditor
{

    private string file;
    private StringBuilder text;
    private int condForFind = 0;
    private int condForOpen = 0;



    public TextEditor()
    {
        text = new StringBuilder();
    }

    public void Open_File(string path)
    {
        file = path;
        if (File.Exists(path))
        {
            text.Clear();
            text.Append(File.ReadAllText(path));
            Console.WriteLine($"File is opened successfully.");
            condForOpen = 0;
        }
        else
        {
            Console.WriteLine($"File is not created.");
            condForOpen = 1;


        }
    }

    public void Save_File()
    {
        while (string.IsNullOrEmpty(file))
        {
            Console.WriteLine("File is not created. Create the file:");
            file = Console.ReadLine();
        }

        File.WriteAllText(file, text.ToString());
        Console.WriteLine($"File saved successfully.");
        try
        {
            Process.Start("notepad.exe", file);
        }
        catch (Exception)
        {
            Console.WriteLine("Error.");
        }
    }
    public void Edit_Text()
    {
        Console.WriteLine("Editing opened.");
        while (true)
        {
            Console.WriteLine("Enter the line number where you want to add text:");
            int lineNum = Convert.ToInt32(Console.ReadLine());

            if (lineNum >= 1)
            {
                string[] lines = text.ToString().Split(Environment.NewLine);

                if (lineNum > lines.Length)
                {
                    Array.Resize(ref lines, lineNum);
                }

                Console.WriteLine("Enter the content you want to add to this line:");
                string content = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(lines[lineNum - 1]))
                {
                    lines[lineNum - 1] = (lines[lineNum - 1] ?? "") + content;
                }
                else
                {
                    lines[lineNum - 1] = (lines[lineNum - 1] ?? "") + " " + content;
                }

                text.Clear();
                text.Append(string.Join(Environment.NewLine, lines));
                Console.WriteLine("Text added successfully.");
            }
            else
                Console.WriteLine("Please enter number greater than 0.");

            Console.WriteLine("Type \":s\" to save if you are done with editing or enter if you want to continue with editing:");

            string line = Console.ReadLine();
            if (line == ":s")
            {
                Save_File();
                break;
            }
            else if (line == ":x")
            {
                Console.WriteLine("Edit exited.");
                break;
            }


            else
            {
                text.AppendLine(line);
            }
        }
    }


    public void Find()
    {

        Console.WriteLine("Enter a content to find:");
        string contentToFind = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(contentToFind))
        {
            Console.WriteLine("Please provide the word to search for.");
            contentToFind = Console.ReadLine();

        }

        string[] lines = text.ToString().Split(Environment.NewLine);

        bool found = false;

        while (!found && contentToFind != ":x")
        {

            for (int lineNum = 0; lineNum < lines.Length; lineNum++)
            {
                int i = 0;
                while (i < lines[lineNum].Length)
                {
                    int index = lines[lineNum].IndexOf(contentToFind, i);
                    if (index != -1)
                    {
                        Console.WriteLine($"Content '{contentToFind}' found in line {lineNum + 1}. Position {index + 1}.");
                        i = index + contentToFind.Length;
                        found = true;

                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine($"Content '{contentToFind}' not found. Enter again:");
                contentToFind = Console.ReadLine();

            }
        }
    }



    public void Replace(string oldContent, string newContent)
    {
        Console.WriteLine("Enter the line from where you want to replace the word:");
        int lineNum = Convert.ToInt32(Console.ReadLine());


        string[] lines = text.ToString().Split(Environment.NewLine);

        while (lineNum < 1 || lineNum > lines.Length || !lines[lineNum - 1].Contains(oldContent))
        {

            Console.WriteLine("Invalid line number. Enter again:");
            lineNum = Convert.ToInt32(Console.ReadLine());

        }

        string line = lines[lineNum - 1];

        lines[lineNum - 1] = line.Replace(oldContent, newContent);

        text.Clear();
        text.Append(string.Join(Environment.NewLine, lines));
        Console.WriteLine($"Replaced '{oldContent}' with '{newContent}'");


    }

    private void FindForReplace(string contentToFind)
    {


        string[] lines = text.ToString().Split(Environment.NewLine);

        bool found = false;

        for (int lineNum = 0; lineNum < lines.Length; lineNum++)
        {
            int i = 0;
            while (i < lines[lineNum].Length)
            {
                int index = lines[lineNum].IndexOf(contentToFind, i);
                if (index != -1)
                {
                    Console.WriteLine($"Text '{contentToFind}' found in line {lineNum + 1}. Position {index + 1}.");
                    i = index + contentToFind.Length;
                    found = true;
                    condForFind = 1;
                }
                else
                {
                    break;
                }
            }
        }

        if (!found)
        {
            Console.WriteLine($"Content '{contentToFind}' not found.");

        }
    }



    public void DeleteLine()
    {

        Console.WriteLine("Enter the line you want to delete:");
        int lineNum = Convert.ToInt32(Console.ReadLine());
        string[] lines = text.ToString().Split(Environment.NewLine);

        while (lineNum < 1 || lineNum > lines.Length)
        {
            Console.WriteLine("Invalid line number. Enter again:");
            lineNum = Convert.ToInt32(Console.ReadLine());

        }
        lines = lines.Where((line, i) => i != lineNum - 1).ToArray();

        text.Clear();
        text.Append(string.Join(Environment.NewLine, lines));

        Console.WriteLine($"Line {lineNum} deleted.");

    }

    public void DeleteWord()
    {

        if (string.IsNullOrWhiteSpace(text.ToString()))
        {
            Console.WriteLine("No content in the file");
        }
        else
        {
            Console.WriteLine("Enter the line you want the content to be deleted from:");
            int lineNum;
            string[] lines = text.ToString().Split(Environment.NewLine);

            lineNum = Convert.ToInt32(Console.ReadLine());


            while (lineNum < 1 || lineNum > text.ToString().Split(Environment.NewLine).Length)
            {

                Console.WriteLine("Invalid line number. Enter again:");
                lineNum = Convert.ToInt32(Console.ReadLine());

            }

            Console.WriteLine("Enter the content you want to delete:");
            string contentToDelete = Console.ReadLine();

            string line = lines[lineNum - 1];
            while (!line.Contains(contentToDelete))
            {
                Console.WriteLine($"Content not found in {lineNum}. line. Enter again:");
                contentToDelete = Console.ReadLine();

            }
            lines[lineNum - 1] = line.Replace(contentToDelete, "");
            Console.WriteLine("Content deleted.");


            text.Clear();
            text.Append(string.Join(Environment.NewLine, lines));
        }



    }

    public void Display()
    {

        if (string.IsNullOrWhiteSpace(text.ToString()))
        {
            Console.WriteLine("No content in the file");
        }
        else
        {
            Console.WriteLine("Current content:");
            Console.WriteLine(text.ToString());
        }
    }

    static void Main(string[] args)
    {
        TextEditor editor = new TextEditor();
        Boolean condition = true;
        Boolean isFileOpened = false;
        




        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("**Text Editor**\n");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("1. ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Open File");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("2. ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Save File");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("3. ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Edit File");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("4. ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Find");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("5. ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Replace");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("6. ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Delete Line");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("7. ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Delete Word");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("8. ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Display");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("9.");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(" Exit");


        Console.ResetColor();

        

        while (condition)
        {

            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("\nChoose an option:");
            Console.ResetColor();





            int input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {

                case 1:
                    Console.WriteLine("Enter the path of the file you want to open: ");
                    string fileName = Console.ReadLine();

                    editor.Open_File(fileName);
                    while (editor.condForOpen == 1)
                    {
                        Console.WriteLine("Enter the path of the file you want to open: ");
                        string file_Name = Console.ReadLine();
                        editor.Open_File(file_Name);



                    }


                    isFileOpened = true;



                    break;

                case 2:
                    if (isFileOpened)
                    {
                        editor.Save_File();
                    }
                    else
                    {
                        Console.WriteLine("You must open the file.");
                    }
                    break;

                case 3:
                    if (isFileOpened)
                    {
                        editor.Edit_Text();
                    }
                    else
                    {
                        Console.WriteLine("You must open the file.");
                    }
                    break;

                case 4:
                    if (isFileOpened)
                    {

                        editor.Find();

                    }
                    else
                    {
                        Console.WriteLine("You must open the file.");
                    }
                    break;


                case 5:
                    if (isFileOpened)
                    {
                        string oldWord = "";
                        string newWord;
                        editor.condForFind = 0;
                        Console.WriteLine("Enter the word you want to replace:");
                        while (editor.condForFind == 0)
                        {
                            oldWord = Console.ReadLine();
                            if (oldWord == ":x")
                                break;
                            editor.FindForReplace(oldWord);
                            if (editor.condForFind == 1)
                            {
                                break;
                            }

                            Console.WriteLine("Enter again:");


                        }
                        if (oldWord == ":x")
                            break;
                        Console.WriteLine("Enter the new word:");
                        newWord = Console.ReadLine();
                        if (newWord == ":x")
                            break;

                        editor.Replace(oldWord, newWord);
                    }
                    else
                    {
                        Console.WriteLine("You must open the file.");


                    }

                    break;


                case 6:
                    if (isFileOpened)
                    {
                        editor.DeleteLine();
                    }
                    else
                    {
                        Console.WriteLine("You must open the file.");
                    }
                    break;

                case 7:
                    if (isFileOpened)
                    {
                        editor.DeleteWord();
                    }
                    else
                    {
                        Console.WriteLine("You must open the file.");
                    }
                    break;

                case 8:
                    if (isFileOpened)
                    {
                        editor.Display();
                    }
                    else
                    {
                        Console.WriteLine("You must open the file.");
                    }
                    break;

                case 9:
                    condition = false;
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine("Thank you for using the program :)");
                    Console.ResetColor();

                    break;

                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }
}