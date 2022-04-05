﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TbsCore.Extensions;
using TbsCore.Helpers;

namespace TbsCore.Database
{
    public class UseragentDatabase
    {
        private static RestClient client;
        private static readonly Random rnd = new Random();

        private UseragentDatabase()
        {
            client = RestClientDatabase.Instance.GetLocalClient;
        }

        private static UseragentDatabase instance = null;

        public static UseragentDatabase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UseragentDatabase();
                }
                return instance;
            }
        }

        private List<string> _userAgentList { get; set; }
        private DateTime _dateTime { get; set; }

        private const string _userAgentUrl = "https://raw.githubusercontent.com/vinaghost/user-agent/main/user-agent.json";

        private void Update()
        {
            var reqest = new RestRequest(_userAgentUrl);
            var task = client.GetAsync(reqest);
            task.Wait();
            string responseBody = task.Result.Content;
            _userAgentList = JsonSerializer.Deserialize<List<string>>(responseBody);

            _dateTime = DateTime.Now.AddMonths(1); // need update after 1 month, thought so
            Save();
        }

        private void Save()
        {
            var userAgentJsonString = JsonSerializer.Serialize(new Model
            {
                UserAgentList = _userAgentList,
                DateTime = _dateTime,
            });
            File.WriteAllText(IoHelperCore.UseragentPath, userAgentJsonString);
        }

        public void Load()
        {
            if (!IoHelperCore.UserAgentExists())
            {
                Update();
                return;
            }
            var userAgentJsonString = File.ReadAllText(IoHelperCore.UseragentPath);
            var modelLoaded = JsonSerializer.Deserialize<Model>(userAgentJsonString);
            _userAgentList = modelLoaded.UserAgentList;
            _dateTime = modelLoaded.DateTime;

            if (_dateTime.IsExpired() || _userAgentList.Count < 1000)
            {
                Update();
            }
        }

        public string GetUserAgent()
        {
            var accounts = DbRepository.GetAccounts();
            bool duplicate;
            using (var hash = SHA256.Create())
            {
                int index;
                do
                {
                    index = rnd.Next(0, _userAgentList.Count);

                    var byteArray = hash.ComputeHash(Encoding.UTF8.GetBytes(_userAgentList[index]));
                    var userAgentHash = BitConverter.ToString(byteArray).ToLower();
                    duplicate = false;

                    foreach (var account in accounts)
                    {
                        foreach (var proxy in account.Access.AllAccess)
                        {
                            if (proxy.UseragentHash?.Equals(userAgentHash) ?? false)
                            {
                                duplicate = true;
                                break; // proxy loop
                            }
                        }
                        if (duplicate) break; // account loop
                    }
                }
                while (duplicate);

                var result = _userAgentList[index];
                _userAgentList.RemoveAt(index);
                Save();
                return result;
            }
        }

        private class Model
        {
            public List<string> UserAgentList { get; set; }
            public DateTime DateTime { get; set; }
        }
    }
}