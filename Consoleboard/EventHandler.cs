using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;

namespace Consoleboard
{
    public class EventHandler
    {
        private Player _player { get; set; }
        private readonly int _consoleNumber;
        private bool isRunning = true;
        private string _consoleId => $"CN-{_consoleNumber}";
        private readonly string _apiUrl = string.Empty;

        /// <summary>
        /// Constructor to register instance with the MAIN BOARD.
        /// </summary>
        public EventHandler()
        {
            Console.Title = ConfigurationManager.AppSettings["ConsoleName"];
            _consoleNumber = DateTime.Now.Millisecond;
            _apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
            AutoRegister();
            DisplayOptions();
        }

        /// <summary>
        /// Display the option to perform actions: UPDATE scores, status. 
        /// </summary>
        public void DisplayOptions()
        {
            DisplayOptions:
            if (isRunning)
                Console.WriteLine($"1. {ConfigurationManager.AppSettings["addScore"]}");

            Console.WriteLine(isRunning ? $"2. {ConfigurationManager.AppSettings["STOP"]}" : $"2. {ConfigurationManager.AppSettings["START"]}");
            Console.WriteLine("3. Exit");

            #region validate the input
            int option = default(int);
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out option))
            {
                Console.WriteLine(ConfigurationManager.AppSettings["InvalidOption"]);
                goto DisplayOptions;
            }
            #endregion

            switch (Convert.ToInt16(input))
            {
                case 1:
                    if (isRunning)
                    {
                        Console.Write(ConfigurationManager.AppSettings["EnterScore"]);
                        _player.LastUpdated = DateTime.Now;
                        _player.Scores += Convert.ToInt32(Console.ReadLine());
                        Save(_player);
                    }
                    else
                        Console.WriteLine(ConfigurationManager.AppSettings["StartConsole"]);
                    break;
                case 2:
                    isRunning = !isRunning;
                    if (isRunning) //reset the score.
                        _player.Scores = 0;
                    _player.Status = isRunning ? Status.RUNNING.ToString() : Status.STOPPED.ToString();
                    Save(_player);
                    break;
                case 3:
                    Console.WriteLine("T.E.R.M.I.N.A.T.E");
                    return;
                default:
                    Console.WriteLine(ConfigurationManager.AppSettings["InvalidOption"]);
                    break;
            }

            Console.WriteLine($"#{_player.LastUpdated} #Score::{_player.Scores} #Status::{_player.Status}");
            DisplayOptions();
        }

        /// <summary>
        /// This will autoregister console as soon as it starts.
        /// </summary>
        private void AutoRegister()
        {
            Console.Write("Player Name: ");
            string playerName = Console.ReadLine();
            _player = new Player()
            {
                Scores = 0,
                Id = _consoleId,
                Number = _consoleNumber,
                PlayerName = playerName,
                Status = Status.RUNNING.ToString(),
                LastUpdated = DateTime.Now,
            };

            Save(_player);
            Console.WriteLine($"#REGISTERED with id:: {_player.Id}#");
            Console.Title = $"Welcome {_player.PlayerName ?? _player.Id}";
        }

        /// <summary>
        /// This method will hit API to save and update console data.
        /// </summary>
        /// <param name="data">The console data.</param>
        private void Save(Player data)
        {
            try
            {
                using (var client = new WebClient())
                {
                    var serializedData = JsonConvert.SerializeObject(data);
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    string response = client.UploadString(new Uri(_apiUrl), "POST", serializedData);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
