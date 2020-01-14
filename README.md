# Translator for custom programming language
### Information
This software product is created for the purpose of course work and is not intended for commercial use. 
This application was created when I studied at the university at the third course in 2016-2017.

Coursework Tasks: Design and build a programming language and develop a translator for a developed programming language.
C # programming language and Microsoft Visual Studio 2015 programming environment were used to develop the translator.
A WPF framework was used to develop the GUI.
The translator was designed for use with Windows 10.

The developed programming language contains I/O , assignment, and conditional and loop operators.

Conditional switch operator:
```
if <logical expression> ¶ <operator list> ¶ else¶ <operator list> ¶ endif
```
Loop operator:
```
for <id> = <expression> to <expression> step <expression> ¶ <operator list> ¶ next
```
Input Operator: ```read (<id list>)```

Output Operator:``` write (<id list>)```

Assignment statement: ```<id> = <expression>```

The arithmetic expression contains the arithmetic operations +, -, *, /, floating-point constants, brackets (,).

A logical expression can contain any arithmetic expressions, brackets [], 
logical operations: or, not, and; as well as relation operations: <, <=,>,> =,! =, ==.

A translator for a given programming language has been developed. As a lexical analysis algorithm use the method: view to delimiter.
As an algorithm for parsing: ascending parsing.

A high-level language translator is designed to translate the program's source text into a computer-readable command format.
The general structure of the translator consists of three main components:
1) lexical analyzer;
2) parser;
3) code generator of machine commands.

The lexical analyzer reads the text of the program as a sequence of characters and converts them into elementary constructions called lexemes.
The lexemes can be elementary constructions of language, for example, identifier, constants, service words.
In the process of converting the input sequence of characters, the lexical analyzer checks them for admissibility 
and issues an error message if invalid characters are found.
The result of the lexical analysis, the lexemes are passed to the parser.

The parser parses the input program using the received lexemes, performs a grammar check of the program text and should 
issue a corresponding message if syntax errors are detected. As a result of the parser, an intermediate record is formed, 
in this work it is a RPN (Reverse Polish notation).

To describe the grammar of the programming language, the Backus-Naur form or BNF was used.
 In BNF, non-terminal characters are denoted by words and written in angular brackets <>, and 2 metasymbols are also used:
1) `::=` - is equal by definition;
2) `|` allows you to combine rules that have the same left part.

All other characters are considered terminal.

The code generator builds the code of the object machine on the basis of the result of the syntax analysis 
and generates the output of the translator: in this implementation, the RPN, which was built by the parser, is executed.

### Usage instruction
To use the program, you need to copy the `grammar.txt` file (it contains the grammar of the programming language) 
to the executable directory.

The `example.txt` text file contains an example program in a given programming language.

Input values are input and output via the console.
