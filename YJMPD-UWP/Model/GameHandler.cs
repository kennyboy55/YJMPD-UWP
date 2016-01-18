﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YJMPD_UWP.Helpers.EventArgs;
using YJMPD_UWP.Model.Object;

namespace YJMPD_UWP.Model
{
    public class GameHandler
    {
        public delegate void OnStatusUpdateHandler(object sender, GameStatusUpdatedEventArgs e);
        public event OnStatusUpdateHandler OnStatusUpdate;

        public delegate void OnPlayersUpdateHandler(object sender, GamePlayersUpdatedEventArgs e);
        public event OnPlayersUpdateHandler OnPlayersUpdate;

        public enum GameStatus { STARTED, SEARCHING, WAITING, ENDED, STOPPED }
        public GameStatus Status { get; private set; }

        public List<Player> Players { get; private set; }

        private void UpdateGameStatus(GameStatus status)
        {
            Status = status;

            if (OnStatusUpdate == null) return;

            OnStatusUpdate(this, new GameStatusUpdatedEventArgs(status));
        }
        private void UpdateGamePlayers(Player player)
        {
            if (OnPlayersUpdate == null) return;

            OnPlayersUpdate(this, new GamePlayersUpdatedEventArgs(player));
        }

        public GameHandler()
        {
            Players = new List<Player>();
            Status = GameStatus.STOPPED;
        }

        public void AddPlayer(string username)
        {
            Player p = new Player(username);
            Players.Add(p);
            UpdateGamePlayers(p);
        }

        public void Reset()
        {
            Players.Clear();
            UpdateGamePlayers(null);
        }

        public void UpdatePlayer(string username, double pointstotal, double points)
        {
            foreach(Player p in Players)
            {
                if(p.Username == username)
                {
                    p.Update(pointstotal, points);
                    UpdateGamePlayers(p);
                    return;
                }
            }
        }


        //Searching

        public async Task<bool> Search()
        {
            return await SearchGame();
        }

        private async Task<bool> SearchGame()
        {
            UpdateGameStatus(GameStatus.SEARCHING);

            await App.Network.SearchGame();

            return true;
        }

        //Starting and Stopping

        public async Task<bool> Start()
        {
            return await StartGame();
        }

        public async Task<bool> Stop()
        {
            return await StopGame();
        }

        private async Task<bool> StartGame()
        {

            //Do stuff

            UpdateGameStatus(GameStatus.SEARCHING);

            await Task.Delay(TimeSpan.FromSeconds(2));
            AddPlayer("Kenneth");
            await Task.Delay(TimeSpan.FromSeconds(1));
            AddPlayer("Yorick");

            UpdateGameStatus(GameStatus.WAITING);

            await Task.Delay(TimeSpan.FromSeconds(3));

            UpdateGameStatus(GameStatus.STARTED);

            return true;
        }

        private async Task<bool> StopGame()
        {
            Reset();
            //Do stuff

            UpdateGameStatus(GameStatus.STOPPED);
            return true;
        }
    }
}
