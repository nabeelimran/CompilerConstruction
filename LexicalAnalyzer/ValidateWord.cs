using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LexicalAnalyzer
{
    static class ValidateWord
    {
        public static string[,] keywords = new string[,]
            {
                {"int","DT"},
                {"float","DT"},
                {"bool","DT"},
                {"string","DT"},
                {"class",""},
                {"if",""},
                {"else",""},
                {"loop",""},
                {"break",""},
                {"continue",""},
                {"return",""},
                {"abstract",""},
                {"virtual","V/O"},
                {"override","V/O"},
                {"static",""},
                {"public","AM"},
                {"private","AM"},
                {"protected","AM"},
                {"struct",""},
                {"new",""},
                {"this",""},
                {"base",""},
                {"true","T/F"},
                {"false","T/F"},
                {"null",""},
                {"main",""},
                {"using",""},
                {"try",""},
                {"catch",""},
            };
        public static string[,] operators = new string[,]
            {
                {"+","PM"},
                {"-","PM"},
                {"*","MDM"},
                {"/","MDM"},
                {"%","MDM"},
                {"++","INC/DEC"},
                {"--","INC/DEC"},
                {"==","RO"},
                {"!=","RO"},
                {">","RO"},
                {">>","SO"},
                {"<<","SO"},
                {"<","RO"},
                {">=","RO"},
                {"<=","RO"},
                {"&&","LO"},
                {"||","LO"},
                {"!",""},
                {"&","BW"},
                {"|","BW"},
                {"=",""},
                {"+=","CA"},
                {"-=","CA"},
                {"*=","CA"},
                {"/=","CA"},
            };
        static char[] punctuators = new char[] { '[', ']', '{', '}', '(', ')', ',', ':', ';', '.' };
        static Regex idRegex = new Regex("^[a-zA-Z_]([a-zA-Z0-9_])*$");
        static Regex intRegex = new Regex("^[+-]?[0-9]+$");
        static Regex floatRegex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
        static Regex stringRegex = new Regex(@"^""(?:\\[\\""'nrt0a]|[^\\""'])*""$");
        static Regex charRegex = new Regex(@"^'(?:\\[\\""'nrt0a]|[^\\""'])'$");

        public static List<Token> tokenSet= new List<Token>();

        public static void validate(string lexeme, int lineNo)
        {
            if (lexeme[0] == '_')
            {
                if (isIdentifier(lexeme))
                {
                    tokenSet.Add(new Token("ID", lexeme, lineNo));
                }
                else
                {
                    tokenSet.Add(new Token("InvalidLexeme", lexeme, lineNo));
                }
            }
            else if (char.IsLetter(lexeme[0]))
            {
                if (isIdentifier(lexeme))
                {
                    string classP = isKeyword(lexeme);
                    if (classP == null)
                    {
                        tokenSet.Add(new Token("ID", lexeme, lineNo));
                    }
                    else
                    {
                        if (classP == "")
                        {
                            tokenSet.Add(new Token(lexeme, classP, lineNo));
                        }
                        else
                        {
                            tokenSet.Add(new Token(classP, lexeme, lineNo));
                        }
                    }
                }
                else
                {
                    tokenSet.Add(new Token("InvalidLexeme", lexeme, lineNo));
                }
            }

            else if (isOperator(lexeme) != null)
            {
                string classP = isOperator(lexeme);
                if (classP == "")
                {
                    tokenSet.Add(new Token(lexeme, classP, lineNo));
                }
                else
                {
                    tokenSet.Add(new Token(classP, lexeme, lineNo));
                }
            }

            else if (char.IsDigit(lexeme[0]) || lexeme[0]=='+' || lexeme[0] == '-')
            {
                if (isIntConst(lexeme))
                {
                    tokenSet.Add(new Token("IntConstant", lexeme, lineNo));
                }
                else if (isFloatConst(lexeme))
                {
                    tokenSet.Add(new Token("FloatConstant", lexeme, lineNo));
                }
                else
                {
                    tokenSet.Add(new Token("InvalidLexeme", lexeme, lineNo));
                }
            }
            else if (lexeme[0] == '.')
            {
                if (lexeme.Length == 1)
                {
                    tokenSet.Add(new Token(".", "", lineNo));
                }
                else if (isFloatConst(lexeme))
                {
                    tokenSet.Add(new Token("FloatConstant", lexeme, lineNo));
                }
                else
                {
                    tokenSet.Add(new Token("InvalidLexeme", lexeme, lineNo));
                }
            }
            else if (lexeme[0] == '"')
            {
                if (isStringConst(lexeme))
                {
                    tokenSet.Add(new Token("StringConstant", lexeme.Substring(1, lexeme.Length - 2), lineNo));
                }
                else
                {
                    tokenSet.Add(new Token("InvalidLexeme", lexeme, lineNo));
                }
            }

            else if (lexeme[0] == '\'')
            {
                if (isCharConst(lexeme))
                {
                    tokenSet.Add(new Token("CharacterConstant", lexeme.Substring(1, lexeme.Length - 2), lineNo));
                }
                else
                {
                    tokenSet.Add(new Token("InvalidLexeme", lexeme, lineNo));
                }
            }

            else if (isPunctuator(lexeme[0]) == "")
            {
                tokenSet.Add(new Token(lexeme, "", lineNo));
            }

            

            else
            {
                tokenSet.Add(new Token("InvalidLexeme", lexeme, lineNo));
            }
        }




        static string isKeyword(string word)
        {
            for (int i = 0; i < keywords.GetLength(0); i++)
            {
                if (word == keywords[i, 0])
                    return keywords[i, 1];
            }
            return null;
        }

        static string isOperator(string op)
        {

            for (int i = 0; i < operators.GetLength(0); i++)
            {
                if (op == operators[i, 0])
                    return operators[i, 1];
            }
            return null;
        }

        static string isPunctuator(char punc)
        {
            for (int i = 0; i < punctuators.Length; i++)
            {
                if (punc == punctuators[i])
                    return "";
            }
            return null;
        }

        static bool isIdentifier(string word)
        {
            return idRegex.IsMatch(word);
        }

        static bool isIntConst(string word)
        {
            return intRegex.IsMatch(word);
        }

        static bool isFloatConst(string word)
        {
            return floatRegex.IsMatch(word);
        }

        static bool isCharConst(string word)
        {
            return charRegex.IsMatch(word);
        }

        static bool isStringConst(string word)
        {
            return stringRegex.IsMatch(word);
        }
    }
}
