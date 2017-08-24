using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Manager;
using AuroraOs.Engine.Core.Interfaces;
using NLog;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;

namespace AuroraOs.Engine.Core.Services
{

    [AuroraService("Audio")]
    public class SpotifyService : ISpotifyService
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private SpotifyWebAPI _spotify;



        public SpotifyService()
        {
            InitSpotify();

        }

        private async void InitSpotify()
        {
            try
            {
                var clientId = ConfigManager.Instance.GetConfigValue<SpotifyService>("clientId", "dd204bfd47d64ad09e2f433aca1c918b");

                var accessToken = ConfigManager.Instance.GetConfigValue<SpotifyService>("access_token", "");

                if (string.IsNullOrEmpty(accessToken))
                {
                    var webApiFactory = new WebAPIFactory("http://localhost", 8000, clientId,
                        Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate |
                        Scope.UserLibraryRead |
                        Scope.UserReadPrivate | Scope.UserFollowRead | Scope.UserReadBirthdate | Scope.UserTopRead |
                        Scope.PlaylistReadCollaborative |
                        Scope.UserReadRecentlyPlayed | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState);

                    _spotify = await webApiFactory.GetWebApi();
                    ConfigManager.Instance.SetConfig<SpotifyService>("access_token", _spotify.AccessToken);
                }
                else
                {
                    _spotify = new SpotifyWebAPI()
                    {
                        AccessToken = accessToken,
                        TokenType = "Bearer",
                        UseAuth = true
                    };
                }

                var userInfo = await _spotify.GetPrivateProfileAsync();

                _logger.Info($"Spotify connected as {userInfo.DisplayName}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error during initializing Spotify Client => {ex.Message}");
            }
        }

        public void Dispose()
        {

        }
    }
}
