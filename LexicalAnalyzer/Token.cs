namespace LexicalAnalyzer
{
    public class Token
    {
        public string classPart;
        public string valuePart;
        public int lineNo;
        public Token(string classPart, string valuePart, int lineNo)
        {
            this.classPart = classPart;
            this.valuePart = valuePart;
            this.lineNo = lineNo;
        }
    }
}
