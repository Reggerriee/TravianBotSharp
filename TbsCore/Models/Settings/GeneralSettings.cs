﻿using TbsCore.Models.Settings;
using TravBotSharp.Files.Models.AccModels;

namespace TravBotSharp.Files.Models.Settings
{
    public class GeneralSettings
    {
        public void Init()
        {
            this.FillFor = 2;
            this.FillInAdvance = 4;
            this.AutoReadIgms = true;
            this.Time = new TimeSettings();
            this.Time.Init();
        }
        public bool AutoActivateProductionBoost { get; set; }

        /// <summary>
        /// Main village where resources will get sent and sent from
        /// </summary>
        public int MainVillage { get; set; }
        /// <summary>
        /// For selecting for how many hours in advance do we want to fill troops in barracks/stable/GB/GS/workshop
        /// TODO: make is selectable for each village and each building separately.
        /// </summary>
        public int FillInAdvance { get; set; }
        /// <summary>
        /// If we fall below FillInAdvance hours, for how many hours do you want to train in advance
        /// eg. We want to fill barracks for 4 hours in advance. When we fall below 4h, train for another 2h.
        /// </summary>
        public int FillFor { get; set; }
        /// <summary>
        /// If true, bot will auto read new IGMs (~5min after detecting one)
        /// </summary>
        public bool AutoReadIgms { get; set; }
        /// <summary>
        /// Chrome selenium driver disable fetching images to save on memory
        /// </summary>
        public bool DisableImages { get; set; }
        /// <summary>
        /// Initialize Chrome selenium driver in headless mode
        /// </summary>
        public bool HeadlessMode { get; set; }
        /// <summary>
        /// Time settings for the bot
        /// </summary>
        public TimeSettings Time { get; set; }
    }
}
