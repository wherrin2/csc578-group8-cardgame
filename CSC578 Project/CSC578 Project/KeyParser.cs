using System;
using System.Diagnostics;

namespace CSC578_Project
{
    public class KeyParser
    {
        public string FileKey { get; private set; }
        public string Key { get; private set; }
        public string PartialKey { get; private set; }
        public string PartialKeyTerms { get; private set; }
        public string Value { get; private set; }
        public string Field { get; private set; }
        private const string All = "all";
        private const string Any = "any";
        private const string Current = "current";

        public KeyParser(string input)
        {
            input = input.Trim();
            var delimiter = new char[] {'.'};
            var words = input.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            int count = words.Length;
            try
            {
                if (count > 0)
                {
                    FileKey = words[0];
                    if (GameObjectManager.GameObjectExists(FileKey, input))
                    {
                        Key = input;
                    }
                    else
                    {
                        if (input.ToLower().Contains("." + Any))
                        {
                            var res = FindPartialKey(words, Any);
                        }
                        else if (input.ToLower().Contains("." + All))
                        {
                            var res = FindPartialKey(words, All);
                        }
                        else if (input.ToLower().Contains("." + Current))
                        {
                            var res = FindPartialKey(words, Current);
                        }
                        else
                        {
                            // last value must be reference to a value
                            string testKey = input.Substring(0, input.LastIndexOf("."));
                            if (GameObjectManager.GameObjectExists(FileKey, testKey))
                            {
                                Key = testKey;
                                Value = input.Substring(input.LastIndexOf(".") + 1);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private bool FindPartialKey(string[] words, string wildcard)
        {
            int count = words.Length;
            if (words[count - 1].ToLower().Equals(wildcard))
            {
                SetPartialKey(words, count -1);
                PartialKeyTerms = wildcard;
                return true;
            }
            else if (count > 1)
            {
                if (words[count - 2].ToLower().Equals(wildcard))
                {
                    SetPartialKey(words, count -2);
                    PartialKeyTerms = wildcard;
                    //last value would be a value of gameobject
                    Value = words[count - 1];
                    return true;
                }
            }

            return false;
        }

        private void SetPartialKey(string[] words, int count)
        {
            PartialKey = words[0];
            for (int i = 1; i < count; i++)
            {
                PartialKey += "." + words[i];
            }
        }
    }
}
