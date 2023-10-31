using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using NoteApp.Model;

namespace NoteApp.ViewModel.Helpers
{
    public class FirebaseAuthHelper
    {
        private static string apiKey = "AIzaSyAwkTpO9nqJGd9Kx5x8ETSyvN2SmtgZ2pc";

        public static async Task<bool> Register(User user)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    Email = user.Username,
                    Password = user.Password,
                    returnSecureToken = true
                };

                string bodyJson = JsonConvert.SerializeObject(body);

                StringContent dataToSend = new StringContent(bodyJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response =
                    await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={apiKey}",
                        dataToSend);

                if (!response.IsSuccessStatusCode)
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    Error? error = JsonConvert.DeserializeObject<Error>(errorJson);

                    MessageBox.Show(error.Err.Message);

                    return false;
                }

                string resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<FirebaseResult>(resultJson);
                App.UserId = result.LocalId;
            }

            return true;
        }

        public static async Task<bool> Login(User user)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    Email = user.Username,
                    Password = user.Password,
                    returnSecureToken = true
                };

                string bodyJson = JsonConvert.SerializeObject(body);

                StringContent dataToSend = new StringContent(bodyJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response =
                    await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}",
                        dataToSend);

                if (!response.IsSuccessStatusCode)
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    Error? error = JsonConvert.DeserializeObject<Error>(errorJson);

                    MessageBox.Show(error.Err.Message);

                    return false;
                }

                string resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<FirebaseResult>(resultJson);
                App.UserId = result.LocalId;
            }

            return true;
        }

        public class FirebaseResult
        {
            public string Kind { get; set; }
            public string IdToken { get; set; }
            public string Email { get; set; }
            public string RefreshToken { get; set; }
            public string ExpresIn { get; set; }
            public string LocalId { get; set; }
        }

        public class ErrorDetails
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }

        public class Error
        {
            public ErrorDetails Err { get; set; }
        }
    }
}
