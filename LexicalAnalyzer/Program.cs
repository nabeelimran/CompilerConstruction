using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex chkdec = new Regex(@"^([+-]?[0-9]+)?$");
            string text;
            string temp ="";
            int lineNo = 1;
            var punctuators = new char[] { '(', ')', '{', '}', '[', ']', ';', ':', ',' };

            using (var reader = new StreamReader(@"d:\code.txt"))                       // from where to read code file
            {
                text = reader.ReadToEnd();
            }
            var i = 0;

            while (i < text.Length)
            {
                if (text[i] == '\"')
                {
                    makeToken(temp, lineNo);
                    temp += text[i++];
                    while (true)
                    {
                        if (text[i] == '\"' || text[i] == '\r')
                        {
                            if (text[i] == '\r')
                            {
                                lineNo++;
                                i += 2;
                            }
                            else
                                temp += text[i++];

                            if (i>=3 && text[i-1] == '\"' && text[i-2] == '\\' && text[i - 3] != '\\')
                                continue;
                            makeToken(temp, lineNo);
                            break;
                        }
                        else
                            temp += text[i++];
                    }
                }

                else if (text[i] == '\'')
                {
                    int length;
                    makeToken(temp, lineNo);
                    temp += text[i++];
                    if (i < text.Length)
                    {
                        if (text[i] == '\\')
                            length = 3;
                        else
                            length = 2;

                        for (int j = 0; j < length; j++)
                        {
                            if (i < text.Length)
                                temp += text[i++];
                            else
                            {
                                makeToken(temp, lineNo);
                                break;
                            }
                        }
                        makeToken(temp, lineNo);
                    }
                    else
                        makeToken(temp, lineNo);
                }

                else if (text[i] == ' ' || text[i] == '\t')
                {
                    makeToken(temp, lineNo);
                    i++;
                }

                else if (text[i] == '.')
                {
                    if(i+1 < text.Length && (text[i + 1] >= '0' && text[i + 1] <= '9'))
                    {
                        if (!chkdec.IsMatch(temp))
                            makeToken(temp, lineNo);
                        temp += text[i++];
                    }
                    else
                    {
                        makeToken(temp, lineNo);
                        temp += text[i++];
                        makeToken(temp, lineNo);
                    }
                }

                else if (text[i] == '+')
                {
                    makeToken(temp, lineNo);
                    temp += text[i++];
                    if (i < text.Length)
                    {
                        if (text[i] == '+' || text[i] == '=')
                        {
                            temp += text[i++];
                            makeToken(temp, lineNo);
                        }
                    }
                }

                else if (text[i] == '-')
                {
                    makeToken(temp, lineNo);
                    temp += text[i++];
                    if (i < text.Length)
                    {
                        if (text[i] == '-' || text[i] == '=')
                        {
                            temp += text[i++];
                            makeToken(temp, lineNo);
                        }
                    }
                }

                else if (text[i] == '*')
                {
                    makeToken(temp, lineNo);
                    temp += text[i++];
                    if (i < text.Length)
                    {
                        if (text[i] == '=')
                            temp += text[i++];
                    }
                    makeToken(temp, lineNo);
                }

                else if (punctuators.Contains(text[i]))
                {
                    makeToken(temp, lineNo);
                    temp += text[i++];
                    makeToken(temp, lineNo);
                }

                else if (text[i] == '/')
                {
                    makeToken(temp, lineNo);
                    if (i + 1 < text.Length)
                    {
                        if (text[i + 1] == '*')
                        {
                            i += 2;
                            while (true)
                            {
                                if (text[i] == '\r')
                                    lineNo++;
                                else if (text[i] == '*')
                                {
                                    if (i + 1 >= text.Length)
                                    {
                                        i++;
                                        break;
                                    }
                                    else if (text[i + 1] == '/')
                                    {
                                        i += 2;
                                        break;
                                    }
                                }
                                i++;
                            }
                        }
                        else if (text[i + 1] == '/')
                        {
                            i += 2;
                            while (true)
                            {
                                if (i + 1 >= text.Length)
                                {
                                    i++;
                                    break;
                                }
                                else if (text[i] == '\r')
                                {
                                    lineNo++;
                                    i += 2;
                                    break;
                                }
                                i++;
                            }
                        }
                        else if (text[i + 1] == '=')
                        {
                            temp += text[i++];
                            makeToken(temp, lineNo);
                        }
                    }
                    else
                    {
                        temp += text[i++];
                        makeToken(temp, lineNo);
                    }
                }

                else if (text[i] == '<')
                {
                    makeToken(temp, lineNo);
                    temp += text[i++];
                    if (i < text.Length)
                    {
                        if (text[i] == '=' || text[i] == '<')
                            temp += text[i++];
                    }
                    makeToken(temp, lineNo);
                }

                else if (text[i] == '>')
                {
                    makeToken(temp, lineNo);
                    temp += text[i++];
                    if (i < text.Length)
                    {
                        if (text[i] == '=' || text[i] == '>')
                            temp += text[i++];
                    }
                    makeToken(temp, lineNo);
                }

                else if (text[i] == '!')
                {
                    makeToken(temp, lineNo);
                    temp += text[i++];
                    if (i < text.Length)
                    {
                        if (text[i] == '=')
                            temp += text[i++];
                    }
                    makeToken(temp, lineNo);
                }

                else if (text[i] == '&')
                {
                    makeToken(temp, lineNo);
                    temp += text[i++];
                    if (i < text.Length)
                    {
                        if (text[i] == '&')
                            temp += text[i++];
                    }
                    makeToken(temp, lineNo);
                }

                else if (text[i] == '|')
                {
                    makeToken(temp, lineNo);
                    temp += text[i++];
                    if (i < text.Length)
                    {
                        if (text[i] == '|')
                            temp += text[i++];
                    }
                    makeToken(temp, lineNo);
                }

                else if (text[i] == '=')
                {
                    makeToken(temp, lineNo);
                    temp += text[i++];
                    if (i < text.Length)
                    {
                        if (text[i] == '=')
                            temp += text[i++];
                    }
                    makeToken(temp, lineNo);
                }

                else if (text[i] == '\r')
                {
                    makeToken(temp, lineNo);
                    lineNo++;
                    i += 2;
                }

                else if ((text[i] >= '0' && text[i] <= '9') || (text[i] >= 'A' && text[i] <= 'Z') || (text[i] >= 'a' && text[i] <= 'z') || text[i] == '_')
                {
                    if (temp == "+" || temp == "-")
                    {
                        if (!(text[i] >= '0' && text[i] <= '9'))
                            makeToken(temp, lineNo);
                    }
                    temp += text[i++];
                }
                else
                    i++;
            }
            writeFile();

            void makeToken(string word, int line)
            {
                if (word != "")
                {
                    ValidateWord.validate(word, line);
                }
                temp = "";
            }

            void writeFile()
            {
                string tokenString = "";
                foreach (var item in ValidateWord.tokenSet)
                {
                    tokenString+="(" + item.classPart + ", " + item.valuePart + ", " + item.lineNo + ")" + Environment.NewLine;
                }
                File.WriteAllText(@"d:\tokens.txt", tokenString);           // where to write token file
            }
            Console.WriteLine("Done :)");

            Console.ReadKey();
        }

    }
}
