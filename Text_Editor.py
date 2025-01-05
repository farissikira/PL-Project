import os
import subprocess
from colorama import init, Fore,  Style, Back

class ConsoleBasedTextEditor:
    

    def __init__(self):
        self.filepath=""
        self.text=[]

 
    def open_file(self, path):

        try:
            with open(path, 'r') as file:
                self.text=file.readlines()
                self.filepath=path
                print("File is opened successfully.")
 
        except FileNotFoundError:
            print("Error reading the file.")

    
    def save_file(self):
        
        try:
            with open(self.filepath, "w") as file:
                file.writelines(self.text)

            print("\nFile saved successfully.")
            self.open_in_notepad()

        except Exception:
            print("\nError saving the file.")


    def edit_file(self, line_num, new_text):
        
            while len(self.text)<=line_num:
                self.text.append("\n")

            self.text[line_num]=self.text[line_num].strip()+" "+new_text+"\n"
            


    def delete_line(self, line_num):
        
        try:
            self.text.pop(line_num)
            print("Line",line_num+1,"is deleted successfully.")

        except IndexError:
            print("Invalid line number")


    def delete_word(self, word):
        
        found=self.find_in_file(word)

        if not found:
            return 
        
        if len(word)==1 and word.isalpha():
            positions_info=[(i+1,[pos+1 for pos, char in enumerate(line) if char==word]) for i, line in enumerate(self.text) if word in line]

            if not positions_info:
                print(word + " not found as a single character in any line.")
                return


            try:

                line_choice = int(input("Choose a line number to delete the letter from: ")) - 1
                valid_lines = [line_num - 1 for line_num, _ in positions_info]

                if line_choice not in valid_lines:
                    print("Invalid line number.")
                    return

                selected_line_positions = next(positions for line_num, positions in positions_info if line_num == line_choice + 1)
                position_choice = int(input("Choose a position to delete: ")) - 1

                if (position_choice + 1) in selected_line_positions:
                    
                    line = self.text[line_choice]
                    self.text[line_choice] = line[:position_choice] + line[position_choice + 1:]
                    print("\nDeleted " + word + " from line", line_choice + 1, "at position", position_choice + 1,".")
                
                else:
                    print("Invalid position choice.")
            
            except ValueError:
                print("Invalid input. Please enter a valid number.")
        
        else:

            for i, line in enumerate(self.text):

                if word in line:  
                    print("\nFound " + word + " in line", i + 1, ": " + line.strip())

                    if word in line.split():  

                        self.text[i] = " ".join(w for w in line.split() if w != word) + "\n"
                        print("Deleted exact match " + word + " from line", i + 1)

                    else:  

                        modified_line = line.replace(word, "")
                        self.text[i] = modified_line
                        print("Deleted part of the word " + word + " in line", i + 1)
                    return

            print("\n'" + word + " not found.")
            

    def find_in_file(self, text_to_find):

        found=False

        if len(text_to_find)==1 and text_to_find.isalpha():

                for i, line in enumerate(self.text):
                    position=[pos+1 for pos, char in enumerate(line) if char==text_to_find]
                    
                    if position:
                        print("Found ",text_to_find, "in line",i+1,"at position", position)
                        found=True
        
        elif text_to_find.strip() and len(text_to_find.split())==1:
            
            for i, line in enumerate(self.text):
                words=line.split()
                position=[pos+1 for pos, word in enumerate(words) if word==text_to_find]

                if position:
                    print("Found "+text_to_find,"in line",i+1,"at position",position)
                    found=True

                else: 
                    partial_matches = [word for word in words if text_to_find in word]
                    
                    if partial_matches:
                        print(text_to_find+" is part of the word(s)",partial_matches,"in line",i + 1)
                        found = True

        else: 

            for i, line in enumerate(self.text):
                
                if text_to_find in line:
                    print("Found "+text_to_find,"in line",i+1)
                    found=True

        if not found:
            print("Content "+text_to_find,"not found.")
        
        return found


    def replace_word(self, word, new_word):
        
        found=self.find_in_file(word)

        if not found: 
            return 

        if len(word) == 1 and word.isalpha():

            positions_info = [ (i + 1, [pos + 1 for pos, char in enumerate(line) if char == word])
                for i, line in enumerate(self.text) if word in line]

            if not positions_info:
                print(word,"not found as a single character in any line.")
                return

            try:
                line_choice = int(input("Choose a line to replace the letter: ")) - 1
                valid_lines = [line_num - 1 for line_num, _ in positions_info]

                if line_choice not in valid_lines:
                    print("Invalid line number.")
                    return

                selected_line_positions = next(positions for line_num, positions in positions_info if line_num == line_choice + 1)
                position_choice = int(input("Choose a position to replace: ")) - 1

                if (position_choice + 1) in selected_line_positions:

                    line = self.text[line_choice]
                    self.text[line_choice] = line[:position_choice] + new_word + line[position_choice + 1:]
                    print("The letter",word,"has been replaced with",new_word,"in line",line_choice + 1)

                else:
                    print("Invalid position choice.")

            except ValueError:
                print("Invalid input. Please enter a valid number.")


        else: 
            multi_line_word=[i for i, line in enumerate(self.text) if word in line]

            if len(multi_line_word)>1:

                line_num=int(input("Enter the line of the word you want to replace: "))-1

                if line_num in multi_line_word:
                    self.text[line_num]=self.text[line_num].replace(word,new_word)
                    print("The word",word,"is replaced with the word:",new_word)
                    return 

                else:
                    print("The word",word,"is not found in the line!")
                    return

            else:
                for i, line in enumerate(self.text):
                    self.text[i]=line.replace(word,new_word)

            print("The word",word,"is replaced with the word:",new_word)


    def display_text(self):
        
        for i, line in enumerate(self.text, start=1):

            print(line.strip())


    def open_in_notepad(self):
        
        if os.name == 'nt' and self.filepath:
            subprocess.Popen(['notepad.exe', self.filepath])
        else:
            print("\nNotepad is not available!")


    def start_program(self):
        
        init()
        
        print(Fore.RED+"\033[1m**Text Editor**"+Style.RESET_ALL)
        print(Fore.RED+"1. "+Style.RESET_ALL+"Open File")
        print(Fore.RED+"2. "+Style.RESET_ALL+"Save File")
        print(Fore.RED+"3. "+Style.RESET_ALL+"Edit File")
        print(Fore.RED+"4. "+Style.RESET_ALL+"Find")
        print(Fore.RED+"5. "+Style.RESET_ALL+"Replace")
        print(Fore.RED+"6. "+Style.RESET_ALL+"Delete Line")
        print(Fore.RED+"7. "+Style.RESET_ALL+"Delete Word")
        print(Fore.RED+"8. "+Style.RESET_ALL+"Display File")
        print(Fore.RED+"9. "+Style.RESET_ALL+"Exit")
        
        while True:
            
            commands= int(input(Fore.RED+'\nChoose an option: '+Style.RESET_ALL))

            if commands==1:

                path=input("\nEnter the path of the file you want to open: ").strip()
                self.open_file(path)


            elif commands==2:

                if not self.text:
                    print("\nNo content to save. Please open or edit a file first.")

                else:
                    self.filepath
                    self.save_file()
            

            elif commands==3:

                if not self.text:
                    print("\nThe file is not opened or it has no content.")

                else:
                    
                    print("\nEditing opened")

                    while(True):

                        line_num=input("Enter the line number to edit (or ':x' to exit): ").strip()
                            
                        if line_num==':x':

                            print("Edit exited.")
                            break
                        
                        if not line_num.isdigit():
                            print("Invalid input. Please enter a valid line number.")
                            continue

                        line_num=int(line_num)-1
                        new_text=input("Enter the content to add: ").strip()
                        self.edit_file(line_num, new_text)
                        print("Content added successfully.")
                            

            elif commands==4:

                if not self.text:
                    print("\nThe file is not opened or it has no content.")

                else:
                     words=input("\nEnter the content you want to find: ").strip()
                     self.find_in_file(words)
                     
                
            elif commands==5:

                if not self.text:
                    print("\nThe file is not opened or it has no content.")

                else:

                     word=input("\nEnter the word to replace: ").strip()
                     new_word=input("Enter the new word: ").strip()
                     self.replace_word(word,new_word)



            elif commands==6:

                if not self.text:
                    print("\nThe file is not opened or it has no content.")

                else:
                    line_num=int(input("\nEnter the line number to delete: "))
                    self.delete_line(line_num-1)


            elif commands==7:
                
                if not self.text:
                    print("\nThe file is not opened or it has no content.")

                else:
                     word=input("\nEnter the word to delete: ").strip()
                     self.delete_word(word)


            elif commands==8:

                if not self.text:
                    print("\nThe file is not opened or it has no content.") 
                else:
                    print("Current content:")
                    print("\n")
                    self.display_text()


            elif commands==9:
                print(Fore.RED+"\nThank you for using our text editor :)\n"+Style.RESET_ALL)
                break


            else:
                print("Invalid option. Please try again.")


e=ConsoleBasedTextEditor()
e.start_program()