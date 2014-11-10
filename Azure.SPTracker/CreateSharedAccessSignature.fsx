#r "../packages/WindowsAzure.ServiceBus.2.4.9.0/lib/net40-full/Microsoft.ServiceBus.dll"

open Microsoft.ServiceBus;
open Microsoft.ServiceBus.Messaging
open System.Runtime.Serialization



let serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", "sptracker", "hub1").ToString().Trim('/');
let generatedSaS = SharedAccessSignatureTokenProvider.GetSharedAccessSignature("SendAccessKey", "[KEY]", serviceUri, new System.TimeSpan(3,0,0));

printfn "%A" generatedSaS

let mfSettings = new MessagingFactorySettings()
mfSettings.TransportType <- TransportType.Amqp;
mfSettings.TokenProvider <- TokenProvider.CreateSharedAccessSignatureTokenProvider(generatedSaS);
let mf = MessagingFactory.Create("sb://sptracker.servicebus.windows.net", mfSettings);

// Create Client
let client = mf.CreateEventHubClient("hub1")
let data = new EventData(System.Text.Encoding.UTF8.GetBytes("Hello world"))

client.Send(data)
