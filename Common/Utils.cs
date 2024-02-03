using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeBE_LEM.Common
{
    public static class Utils
    {
        public static string CustomChangeToEnglishChar(this string str)
        {
            string[] VietNamChar = new string[]
            {
                "aAeEoOuUiIyYdD",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ",
                "đ",
                "Đ",
            };
            //Replace
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }

            return str;
        }
        public static bool HasSpecialChar(string str)
        {
            var regx = new Regex("[^a-zA-Z0-9._]");
            if (regx.IsMatch(str))
            {
                return true;
            }
            return false;
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static DateTime ConvertDateString(string dateString)
        {
            try
            {
                DateTime date = DateTime.ParseExact(dateString, "dd-MM-yyyy", null);
                return date;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
    }
}