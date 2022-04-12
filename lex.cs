using System;
using System.Collections.Generic;

public class ExpressionParser {
    
    public enum Symbol
    {
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
    }

    /**
     * Generator for tokens. 
     * Use 'yield return' to return tokens one at a time.
     **/
    public static IEnumerable<Node> tokenize(System.IO.StreamReader src) 
    {
        int line = 1;
        int column = 1;
        System.Text.StringBuilder lexeme = new System.Text.StringBuilder();

        yield return null;
    }

    public static void Main(string[] args){
        try {
            foreach (Node n in tokenize(new System.IO.StreamReader(Console.OpenStandardInput()))){
                //Console.WriteLine($"{n.Symbol,-15}\t{n.Text}");
            }
        } catch (Exception e){
            Console.WriteLine(e.Message);
        }
    }
}
