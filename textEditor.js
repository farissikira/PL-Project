//Console-based Text Editor

//import neccessary modules (first one for working with files, second one for user input, and the third one for color)
import fs from 'fs';
import readline from 'readline';
import chalk from 'chalk';

//create an interface that allows reading user input from the terminal
const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

//menu options
function menu() {
    console.log(chalk.bold.red("\n** Text Editor **"));
    console.log(chalk.red("1.") + " Open File");
    console.log(chalk.red("2.") + " Save File");
    console.log(chalk.red("3.") + " Edit File");
    console.log(chalk.red("4.") + " Find");
    console.log(chalk.red("5.") + " Replace");
    console.log(chalk.red("6.") + " Delete Line");
    console.log(chalk.red("7.") + " Delete Word");
    console.log(chalk.red("8.") + " Display File");
    console.log(chalk.red("9.") + " Exit");
    rl.question(chalk.red("\nChoose an option: "), menuOptions);
}

//global variable that will store and modify the content of our file
let fileContent = '';

//global variable that checks if the file is opened
let fileOpened = false;

//global variable that stors the path of the opened file
let openedFilePath = ''

//handling possible menu options
function menuOptions(option) {
    if (!fileOpened && (option === '2' || option === '3' || option === '4' || option === '5' || option === '6' || option === '7' || option === '8')) {
        console.log("Please open a file first.");
        return rl.question(chalk.red("\nChoose an option: "), menuOptions);
    }

    switch (option) {
        case '1':
            openFile();
            break;
        case '2':
            saveFile();
            break;
        case '3':
            editFile();
            break;
        case '4':
            findWord();
            break;
        case '5':
            replaceWord();
            break;
        case '6':
            deleteLine();
            break;
        case '7':
            deleteWord();
            break;
        case '8':
            displayFile();
            break;
        case '9':
            console.log(chalk.bold.red("Thank you for using our text editor :)\n"));
            rl.close();
            break;
        default:
            console.log("Invalid option. Please try again.");
            rl.question(chalk.red("\nChoose an option: "), menuOptions);
    }
}

//opening a file
function openFile() {
    rl.question("Enter the path of the file you want to open: ", (path) => {
        try {
            if (!fs.existsSync(path)) {
                console.error("The path is invalid or the file does not exist. Please try again.");
                rl.question(chalk.red("\nChoose an option: "), menuOptions);
                return;
            }

            fileContent = fs.readFileSync(path, 'utf8');
            openedFilePath = path;
            fileOpened = true;
            console.log("File opened successfully.");
        }
        catch (error) {
            console.error("Error reading the file. Please ensure the file path is correct.");
        }
        finally {
            rl.question(chalk.red("\nChoose an option: "), menuOptions);
        }
    });
}

//saving a file
function saveFile() {
    if (!fileOpened) {
        console.log("No file has been opened or edited yet. Please open a file first.");
        return rl.question(chalk.red("\nChoose an option: "), menuOptions);
    }

    rl.question("Enter the path to save the file: ", (path) => {
        try {
            if (!fs.existsSync(path)) {
                console.error("The path is invalid or the file does not exist.");
                return rl.question(chalk.red("\nChoose an option: "), menuOptions);
            }

            fs.writeFileSync(path, fileContent, 'utf8');
            console.log("File saved successfully.");
        }
        catch (error) {
            console.error("Error saving the file. Please ensure the file path is correct.");
        }
        finally {
            rl.question(chalk.red("\nChoose an option: "), menuOptions);
        }
    });
}

//editing in the editor
function editFile() {
    console.log("Editing opened.");
    const lines = fileContent.split("\n");

    function editingLoop() {
        rl.question("Enter the line number to edit (':x' to exit editing): ", (lineNumber) => {
            try {
                if (lineNumber === ":x") {
                    console.log("Editing exited.");
                    return rl.question(chalk.red("\nChoose an option: "), menuOptions);
                }

                lineNumber = parseInt(lineNumber);
                if (isNaN(lineNumber) || lineNumber < 1) {
                    console.log("Invalid line number.");
                    editingLoop();
                    return;
                }

                rl.question("Enter the content to add: ", (content) => {
                    while (lines.length < lineNumber) {
                        lines.push("");
                    }
                    lines[lineNumber - 1] += (lines[lineNumber - 1] ? " " : "") + content;
                    fileContent = lines.join("\n");
                    console.log("Content added successfully.");
                    editingLoop();
                });
            }
            catch (error) {
                console.error("An error occurred while editing. Please try again.");
                editingLoop();
            }
        });
    }
    editingLoop();
}

//finding a specific word
function findWord() {
    try {
        if (!fileContent) {
            console.log("No content in the file.");
        }
        else {
            rl.question("Enter the word to find: ", (word) => {
                const lines = fileContent.split("\n");
                let found = false;
                lines.forEach((line, index) => {
                    let position = line.indexOf(word);
                    while (position !== -1) {
                        console.log(`Found '${word}' in line ${index + 1} at position ${position + 1}`);
                        position = line.indexOf(word, position + 1);
                        found = true;
                    }
                });
                if (!found) {
                    console.log(`Word '${word}' not found.`);
                }
                rl.question(chalk.red("\nChoose an option: "), menuOptions);
            });
        }
    }
    catch (error) {
        console.error("An error occurred while searching for the word.");
        rl.question(chalk.red("\nChoose an option: "), menuOptions);
    }
}

//replacing a word
function replaceWord() {
    if (!fileContent) {
        console.log("No content in the file.");
        rl.question(chalk.red("\nChoose an option: "), menuOptions);
        return;
    }

    rl.question("Enter the word to replace: ", (oldWord) => {
        const lines = fileContent.split("\n");
        const occurrences = [];

        lines.forEach((line, index) => {
            let startPosition = 0;
            while ((startPosition = line.indexOf(oldWord, startPosition)) !== -1) {
                occurrences.push({ line: index + 1, position: startPosition + 1 });
                console.log(`Found '${oldWord}' in line ${index + 1} at position ${startPosition + 1}`);
                startPosition += oldWord.length;
            }
        });

        if (occurrences.length === 0) {
            console.log(`Word '${oldWord}' not found in the file.`);
            rl.question(chalk.red("\nChoose an option: "), menuOptions);
            return;
        }

        rl.question("Enter the line number where you want to replace the word: ", (lineNumberInput) => {
            const lineNumber = parseInt(lineNumberInput);
            if (isNaN(lineNumber) || lineNumber < 1 || lineNumber > lines.length) {
                console.log("Invalid line number. Returning to menu.");
                rl.question(chalk.red("\nChoose an option: "), menuOptions);
                return;
            }

            rl.question("Enter the position of the word to replace: ", (positionInput) => {
                const position = parseInt(positionInput);
                if (isNaN(position) || position < 1) {
                    console.log("Invalid position. Returning to menu.");
                    rl.question(chalk.red("\nChoose an option: "), menuOptions);
                    return;
                }

                const line = lines[lineNumber - 1];
                const occurrencesInLine = [];
                let startPosition = 0;

                while ((startPosition = line.indexOf(oldWord, startPosition)) !== -1) {
                    occurrencesInLine.push(startPosition + 1);
                    startPosition += oldWord.length;
                }

                if (!occurrencesInLine.includes(position)) {
                    console.log(`The word '${oldWord}' is not found at position ${position} in line ${lineNumber}. Returning to menu.`);
                    rl.question(chalk.red("\nChoose an option: "), menuOptions);
                    return;
                }

                rl.question("Enter the new word to replace it with: ", (newWord) => {
                    const beforeWord = line.slice(0, position - 1);
                    const afterWord = line.slice(position - 1 + oldWord.length);
                    lines[lineNumber - 1] = beforeWord + newWord + afterWord;
                    fileContent = lines.join("\n");
                    console.log(`Successfully replaced '${oldWord}' with '${newWord}' on line ${lineNumber}, position ${position}.`);
                    rl.question(chalk.red("\nChoose an option: "), menuOptions);
                });
            });
        });
    });
}

//deleting a specific line
function deleteLine() {
    try {
        if (!fileContent) {
            console.log("No content in the file.");
        }
        else {
            const lines = fileContent.split("\n");
            rl.question("Enter the line number to delete: ", (lineNumber) => {
                lineNumber = parseInt(lineNumber);
                if (isNaN(lineNumber) || lineNumber < 1 || lineNumber > lines.length) {
                    console.log("Invalid line number.");
                } else {
                    lines.splice(lineNumber - 1, 1);
                    fileContent = lines.join("\n");
                    console.log(`Line ${lineNumber} deleted successfully.`);
                }
                rl.question(chalk.red("\nChoose an option: "), menuOptions);
            });
        }
    }
    catch (error) {
        console.error("An error occurred while deleting the line.");
        rl.question(chalk.red("\nChoose an option: "), menuOptions);
    }
}

//deleting a specific word
function deleteWord() {
    if (!fileContent) {
        console.log("No content in the file.");
        rl.question(chalk.red("\nChoose an option: "), menuOptions);
        return;
    }

    const lines = fileContent.split("\n");

    rl.question("Enter the word to delete: ", (word) => {
        let found = false;
        const occurrences = [];

        lines.forEach((line, lineIndex) => {
            let startPosition = 0;
            while ((startPosition = line.indexOf(word, startPosition)) !== -1) {
                occurrences.push({
                    lineNumber: lineIndex + 1,
                    position: startPosition + 1,
                });
                console.log(`Found '${word}' in line ${lineIndex + 1} at position ${startPosition + 1}`);
                startPosition += word.length;
                found = true;
            }
        });

        if (!found) {
            console.log(`Word '${word}' not found in the file.`);
            rl.question(chalk.red("\nChoose an option: "), menuOptions);
            return;
        }

        rl.question("Enter the line number where you want to delete the word: ", (lineNumberInput) => {
            const lineNumber = parseInt(lineNumberInput);

            if (isNaN(lineNumber) || lineNumber < 1 || lineNumber > lines.length) {
                console.log("Invalid line number. Returning to menu.");
                rl.question(chalk.red("\nChoose an option: "), menuOptions);
                return;
            }

            rl.question("Enter the position of the word to delete: ", (positionInput) => {
                const position = parseInt(positionInput);

                const occurrence = occurrences.find(
                    (occ) => occ.lineNumber === lineNumber && occ.position === position
                );

                if (!occurrence) {
                    console.log("Invalid position. Returning to menu.");
                    rl.question(chalk.red("\nChoose an option: "), menuOptions);
                    return;
                }

                const line = lines[lineNumber - 1];
                const beforeWord = line.slice(0, position - 1);
                const afterWord = line.slice(position - 1 + word.length);
                lines[lineNumber - 1] = beforeWord + afterWord;

                fileContent = lines.join("\n");
                console.log(`Successfully deleted '${word}' from line ${lineNumber}, position ${position}.`);
                rl.question(chalk.red("\nChoose an option: "), menuOptions);
            });
        });
    });
}

//displaying all the contents of the file
function displayFile() {
    try {
        if (!fileContent) {
            console.log("No content in the file.");
        } else {
            console.log("Current content: \n" + fileContent);
        }
    } catch (error) {
        console.error("An error occurred while displaying the file.");
    } finally {
        rl.question(chalk.red("\nChoose an option: "), menuOptions);
    }
}

menu();
