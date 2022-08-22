using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace KindleToolAPI.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Returns FileContentResult from given list of items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static FileContentResult ToTextFile<T>(this List<T> list, string fileName = "file", string contentType = "text/plain")
        {
            var data = JsonConvert.SerializeObject(list);
            var file = new FileContentResult(Encoding.Unicode.GetBytes(data), contentType);
            file.FileDownloadName = $"{fileName}.txt";

            return file;
        }

        /// <summary>
        /// Returns first N items from given list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IEnumerable<T> TakeFirstN<T>(this List<T> list, int n)
        {
            if (list == null)
            {
                throw new InvalidOperationException("List is empty");
            }

            if (list.Count < n)
            {
                n = list.Count;
            }

            for (int i = 0; i < n; i++)
            {
                yield return list[i];
            }
        }

        /// <summary>
        /// Returns last N items from given list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IEnumerable<T> TakeLastN<T>(this List<T> list, int n)
        {
            if (list == null)
            {
                throw new InvalidOperationException("List is empty");
            }

            if (list.Count < n)
            {
                n = list.Count;
            }

            for (int i = list.Count - n; i < list.Count; ++i)
            {
                yield return list[i];
            }
        }
    }
}
