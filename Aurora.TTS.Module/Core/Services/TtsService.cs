using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aurora.TTS.Module.Core.Interfaces;
using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Data.Events;
using AuroraOs.Common.Core.Manager;
using AuroraOs.Common.Core.Services.Interfaces;
using NLog;

namespace Aurora.TTS.Module.Core.Services
{
    [AuroraService("TTS")]
    public class TtsService : ITtsService
    {

      
        public IEventQueueService EventQueueService { get; set; }
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private SpeechSynthesizer _speechSynthesizer;

        public TtsService(IEventQueueService eventQueueService)
        {
            EventQueueService = eventQueueService;
        }

        public Task Init()
        {

            var culture = new CultureInfo(ConfigManager.Instance.GetConfigValue<TtsService>("locale", Thread.CurrentThread.CurrentCulture.Name));

            _speechSynthesizer = new SpeechSynthesizer();
            var languages = _speechSynthesizer.GetInstalledVoices(culture);

            if (languages.Any())
            {
                _speechSynthesizer.SelectVoice(languages.FirstOrDefault()?.VoiceInfo.Name);

                EventQueueService.Subscribe<AiSpeechResultEvent>(e =>
                {
                    _speechSynthesizer.SpeakAsync(e.ResultText);
                });

                _speechSynthesizer.Speak("CIao");

            }
           

            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}
