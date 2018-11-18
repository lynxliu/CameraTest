using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using System.Reflection;
using System.Collections.Generic;

namespace SilverlightLFC.common.KeyWords
{
    public class KeyWordsManager//管理关键字的功能类
    {
        public static char KeyWordSeperateChar=';';
        public void AddKeyWords(AbstractLFCDataObject o,string word)
        {
            PropertyInfo pi = o.GetType().GetRuntimeProperty("KeyWords");//必须属性名为KeyWords
            if (pi != null)
            {
                string kws = pi.GetValue(o, null).ToString();
                List<string> ls = Environment.getStringListFromCommaString(kws, KeyWordsManager.KeyWordSeperateChar);
                if (ls.Contains(word)) { return; }
                ls.Add(word);
                kws = Environment.getCommaString(ls, KeyWordsManager.KeyWordSeperateChar);
                pi.SetValue(o, kws,null);
            }
        }
        public void RemoveKeyWords(AbstractLFCDataObject o, string word)
        {
            PropertyInfo pi = o.GetType().GetRuntimeProperty("KeyWords");//必须属性名为KeyWords
            if (pi != null)
            {
                string kws = pi.GetValue(o, null).ToString();
                List<string> ls = Environment.getStringListFromCommaString(kws, KeyWordsManager.KeyWordSeperateChar);
                if (ls.Contains(word)) { ls.Remove(word); }
                kws = Environment.getCommaString(ls, KeyWordsManager.KeyWordSeperateChar);
                pi.SetValue(o, kws, null);
            }
        }
        public void ClearKeyWords(AbstractLFCDataObject o)
        {
            PropertyInfo pi = o.GetType().GetRuntimeProperty("KeyWords");//必须属性名为KeyWords
            if (pi != null)
            {
                pi.SetValue(o, "", null);
            }
        }

        public List<string> getKeyWordsList(AbstractLFCDataObject o)
        {
            PropertyInfo pi = o.GetType().GetRuntimeProperty("KeyWords");//必须属性名为KeyWords
            if (pi != null)
            {
                string kws = pi.GetValue(o, null).ToString();
                List<string> ls = Environment.getStringListFromCommaString(kws, KeyWordsManager.KeyWordSeperateChar);
                return ls;
            }
            return null;
        }
    }
}
