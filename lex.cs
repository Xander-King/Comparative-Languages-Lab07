using System;
using System.Collections.Generic;

public class ExpressionParser {
    
    public enum Symbol
    {
        WS, ID, NUM, LITERAL
    }

    /**
     * Represents a node in a parse tree. 
     * - Should keep track of the 'text' of the node (the substring under the node)
     * - Should keep track of the line and column where the node begins. 
     * - Should keep track of the children of the node in the parse tree
     * - should keep track of the Symbol (see the enum) corresponding to the node
     * - Tokens are leaf nodes (the array of children should be null)
     * - Needs a constructor with symbol, text, line, and column
     **/
    public class Node{
        string text="";
        int line=0, column=0;
        List<Node> children = new List<Node>();
        Symbol symbol;

        public Node(Symbol symbol, string text="", int line=0, int column=0) {
            this.symbol = symbol;
            this.text = text;
            this.line = line;
            this.column = column;
        }

        public string Text {
            get {
                return text;
            }
            set {
                Console.WriteLine($"Text was changed from {text} to {value}");
                text = value;
            }
        }

        public Symbol Symbol{
            get {
                return symbol;
            }
        }

        public int Line{
            get {
                return line;
            }
        }

        public int Column {
            get {
                return column;
            }
        }
    }

    /**
     * Generator for tokens. 
     * Use 'yield return' to return tokens one at a time.
     **/
    public static IEnumerable<Node> 
                    tokenize(System.IO.StreamReader src) 
    {
        int line = 1;
        int column = 1;
        System.Text.StringBuilder lexeme = new System.Text.StringBuilder();
        
        int state = 0;

        while (src.Peek() != -1) {
            char c = (char)src.Peek();

            switch (state) {
                case 0: //start
                    if (c == '\n') {
                        line += 1;
                        column = 1;
                        src.Read();
                    } else if (char.IsWhiteSpace(c) || c == ' ' || c == '\t') {
                        column += 1;
                        src.Read();
                    } else if (c == '+' || c == '-') {
                        state = 1;
                        lexeme.Append(c);
                        column++;
                        src.Read();
                    } else if ('0' <= c && c <= '9') {
                        state = 2;
                        lexeme.Append(c);
                        column++;
                        src.Read();
                    } else if (c == '*') {
                        state = 4;
                        lexeme.Append(c);
                        column++;
                        src.Read();
                    } else if (char.IsLetter(c) || c == '_') {
                        state = 6;
                        lexeme.Append(c);
                        column++;
                        src.Read();
                    } else if ("=()".Contains(c)) {
                        state = 5;
                        lexeme.Append(c);
                        column++;
                        src.Read();
                    } else {
                        throw new Exception($"Invalid character \'{c}\' at line {line} column {column}");
                    }
                break;
                case 1:
                    if(char.IsDigit(c)) {
                        state = 2;
                        lexeme.Append(c);
                        column++;
                        src.Read();
                    } else {
                        yield return new Node(Symbol.LITERAL, lexeme.ToString(), line, column);
                        lexeme.Clear();
                        state = 0;
                    }
                break;
                case 2:
                    if (char.IsDigit(c)) {
                        lexeme.Append(c);
                        column++;
                        src.Read();
                    } else if (c == '.') {
                        state = 3;
                        lexeme.Append(c);
                        column++;
                        src.Read();
                    } else {
                        yield return new Node(Symbol.NUM, lexeme.ToString(), line, column);
                        lexeme.Clear();
                        state = 0;
                    }
                break;
                case 3:
                    if (char.IsDigit(c)) {
                        lexeme.Append(c);
                        column++;
                        src.Read();
                    } else {
                        yield return new Node(Symbol.NUM, lexeme.ToString(), line, column);
                        lexeme.Clear();
                        state = 0;
                    }
                break;
                case 4:
                    if (c == '*') {
                        state = 5;
                        lexeme.Append(c);
                        column++;
                        src.Read();
                    } else {
                        yield return new Node(Symbol.LITERAL, lexeme.ToString(), line, column);
                        lexeme.Clear();
                        state = 0;
                    }
                break;
                case 5:
                    yield return new Node(Symbol.LITERAL, lexeme.ToString(), line, column);
                    lexeme.Clear();
                    state = 0;
                break;
                case 6:
                    if (char.IsDigit(c) || char.IsLetter(c) || c == '_') {
                        lexeme.Append(c);
                        column++;
                        src.Read();
                    } else {
                        yield return new Node(Symbol.ID, lexeme.ToString(), line, column);
                        lexeme.Clear();
                        state = 0;
                    }
                break;
            }
                    
        }
    }


    

    public static void Main(string[] args){
        try {
            var stdin = Console.OpenStandardInput();
            var reader = new System.IO.StreamReader(stdin);
            foreach (Node n in tokenize(reader)){
                Console.WriteLine($"{n.Symbol,-15}\t{n.Text}");
            }
        } catch (Exception e){
            Console.WriteLine(e.Message);
        }
    }
}
