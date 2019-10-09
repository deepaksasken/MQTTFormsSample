using System;
using System.Diagnostics;
using System.Net.Mqtt;
using System.Text;
using System.Windows.Input;

using Xamarin.Forms;

namespace MQTTFormsSample.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));

            SetupMQTTAsync();
        }

        private async System.Threading.Tasks.Task SetupMQTTAsync()
        {
            try
            {

                var configuration = new MqttConfiguration();
                configuration.Port = 1883;
                var client = await MqttClient.CreateAsync("test.mosquitto.org", configuration);
                var sessionState = await client.ConnectAsync(new MqttClientCredentials("87d192d17f054250a6c35bb637e299ad"));

                await client.SubscribeAsync("home/garden/fountain", MqttQualityOfService.AtLeastOnce);

                client.MessageStream.Subscribe(msg => Console.WriteLine($"Message received in topic {Encoding.UTF8.GetString(msg.Payload)}"));

                var message2 = new MqttApplicationMessage("home/garden/fountain", Encoding.UTF8.GetBytes("Foo Message 2"));
                await client.PublishAsync(message2, MqttQualityOfService.AtLeastOnce);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public ICommand OpenWebCommand { get; }
    }
}