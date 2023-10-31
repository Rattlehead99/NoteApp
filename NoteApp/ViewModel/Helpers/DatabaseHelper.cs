using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NoteApp.Model.Interfaces;
using SQLite;

namespace NoteApp.ViewModel.Helpers
{
    public class DatabaseHelper
    {
        private static string dbFile = Path.Combine(Environment.CurrentDirectory, "notesDb.db3");
        private static string dbPath = "https://wpfnoteapp-default-rtdb.europe-west1.firebasedatabase.app/";

        public static async Task<bool> Insert<T>(T item)
        {
            //SQLite Code
            //bool result = false;

            //using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            //{
            //    connection.CreateTable<T>();
            //    int rows = connection.Insert(item);
            //    if (rows > 0)
            //    {
            //        result = true;
            //    }
            //}

            //return result;

            string jsonBody = JsonConvert.SerializeObject(item);
            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage result =
                    await client.PostAsync($"{dbPath}{item.GetType().Name.ToLower()}.json", content);
                if (!result.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
        }


        public static async Task<bool> Update<T>(T item) where T : IIdentifiable
        {
            //SQLite code
            //bool result = false;

            //using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            //{
            //    connection.CreateTable<T>();
            //    int rows = connection.Update(item);
            //    if (rows > 0)
            //    {
            //        result = true;
            //    }
            //}

            //return result;

            string jsonBody = JsonConvert.SerializeObject(item);
            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage result =
                    await client.PatchAsync($"{dbPath}{item.GetType().Name.ToLower()}/{item.Id}.json", content);
                if (!result.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
        }

        public static async Task<bool> Delete<T>(T item) where T : IIdentifiable
        {
            //bool result = false;

            //using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            //{
            //    connection.CreateTable<T>();
            //    int rows = connection.Delete(item);
            //    if (rows > 0)
            //    {
            //        result = true;
            //    }
            //}

            //return result;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage result =
                    await client.DeleteAsync($"{dbPath}{item.GetType().Name.ToLower()}/{item.Id}.json");
                if (!result.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
        }

        public static async Task<List<T>> ReadAll<T>() where T : IIdentifiable//, new()
        {
            //SQLite code
            //List<T> items = new List<T>();

            //using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            //{
            //    connection.CreateTable<T>();
            //    items = connection.Table<T>().ToList();
            //}

            //return items;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync($"{dbPath}{typeof(T).Name.ToLower()}.json");

                var jsonResult = await result.Content.ReadAsStringAsync();

                if (!result.IsSuccessStatusCode)
                {
                    return null;
                }

                Dictionary<string,T>? objects = JsonConvert.DeserializeObject<Dictionary<string,T>>(jsonResult);

                List<T> itemList = new List<T>();
                foreach (var obj in objects)
                {
                    obj.Value.Id = obj.Key;
                    itemList.Add(obj.Value);
                }

                return itemList;
            }
        }
    }
}
