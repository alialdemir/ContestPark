using System;
using System.Linq;
using System.Net.Mail;

namespace ContestPark.Entities
{
    public class GlobalSettingsManager : IGlobalSettings
    {
        /// <summary>
        /// Altın miktarını örneğin 1k 10k 100m gibi kısaltarak gösterir
        /// </summary>
        /// <param name="number">Altın miktarı</param>
        /// <returns></returns>
        public string NumberFormating(int number)
        {
            string number1 = number.ToString();
            if (number1.Length <= 4) return number1;
            if (number1.Length == 5) return number1.Substring(0, 2) + "K";
            else if (number1.Length >= 6 && number1.Length < 7) return number1.Substring(0, 3) + "K";
            else if (number1.Length >= 6)
            {
                if (number1.Substring(1, 1) != "0")
                    return number1.Substring(0, 1) + "," + number1.Substring(1, 1) + "M";
                else if (number1.Length == 8) return number1.Substring(0, 2) + "M";
                else if (number1.Length >= 9) return number1.Substring(0, 3) + "M";
            }
            return "-1";
        }

        /// <summary>
        /// asp.net identity için random şifre üreticisi
        /// </summary>
        /// <param name="length">Şifre uzunluğu</param>
        /// <returns>Şifre</returns>
        public string GenerateRandomString(int length)
        {
            char[] charArr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
            string randomString = String.Empty;
            Random objRandom = new Random();

            int rnd = objRandom.Next(length - 1);
            for (int i = 0; i < length; i++)
            {
                if (i == rnd)
                {
                    randomString += "-*";//rasgele sembol ekledik
                    continue;
                }
                int x = objRandom.Next(1, charArr.Length);
                if (!randomString.Contains(charArr.GetValue(x).ToString()))
                    randomString += charArr.GetValue(x);
                else
                    i--;
            }
            return randomString;
        }

        /// <summary>
        /// Kelimelerin baş harflerini büyük yazar
        /// </summary>
        /// <param name="name">Kelime - isim - cümle</param>
        /// <returns>Baş harfleri büyük olarak döndürür</returns>
        public string InitialsLarge(string name)
        {
            name = name.ToLower();
            string[] space = name.Split(' ');
            string names = space.Aggregate("", (current, item) => current + (item.Substring(0, 1).ToUpper() + item.Substring(1, item.Length - 1).ToLower() + " "));
            return names.Trim();
        }

        /// <summary>
        /// Parametreden gelen message değişkeni içinden türkçe karakterleri ingilizce karakter ile değiştirir
        /// </summary>
        /// <param name="message">Herhangi bir yazı</param>
        /// <returns>Türkçe karakterler çıkarılmış şekli</returns>
        public string ReplaceTurksihEnglishCharacter(string message)
        {
            message = message.Replace("\n", "").Trim().ToUpper();//sondaki boşluğu sildik ve büyük harf yaptık
            //türkçe harfler
            char[] oldValue = new char[] { 'ö', 'Ö', 'ü', 'Ü', 'ç', 'Ç', 'İ', 'ı', 'Ğ', 'ğ', 'Ş', 'ş' };
            //ingilizce harfler
            char[] newValue = new char[] { 'o', 'O', 'u', 'U', 'c', 'C', 'I', 'i', 'G', 'g', 'S', 's' };
            for (int i = 0; i <= oldValue.Length - 1; i++)
                message = message.Replace(oldValue[i], newValue[i]); //harfi değiştirdik
            return message.Replace(" ", "").ToLower();
        }

        /// <summary>
        /// Yazının içinde türkçe karakter var mı kontrol eder;
        /// </summary>
        /// <param name="message">Herhangi bir yazı</param>
        /// <returns>Türkçe harf varsa true yoksa false</returns>
        public bool TurkishCharacterControl(string message)//Türkçe karakter var mı kontrol eder
        {
            message = message.Replace("\n", "").Trim().ToLower();//sondaki boşluğu sildik ve büyük harf yaptık
            //türkçe harfler
            char[] oldValue = new char[] { 'ö', 'Ö', 'ü', 'Ü', 'ç', 'Ç', 'İ', 'ı', 'Ğ', 'ğ', 'Ş', 'ş' };
            foreach (char character in oldValue)
                if (message.IndexOf(character) >= 0)
                    return true;

            return false;
        }

        /// <summary>
        /// Email adress validation
        /// </summary>
        /// <param name="emailaddress"></param>
        /// <returns></returns>
        public bool IsEmailValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

    public interface IGlobalSettings
    {
        bool TurkishCharacterControl(string message);

        string ReplaceTurksihEnglishCharacter(string message);

        string InitialsLarge(string name);

        string GenerateRandomString(int length);

        string NumberFormating(int number);

        bool IsEmailValid(string emailaddress);
    }
}