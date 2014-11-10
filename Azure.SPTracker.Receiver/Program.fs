// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.


open Microsoft.ServiceBus.Messaging
open System.Configuration
open Azure.SPTracker.Receiver
open Microsoft.ServiceBus

let GetServiceBusConnectionString() = 
    let connectionString = ConfigurationManager.AppSettings.["Microsoft.ServiceBus.ConnectionString"]
    let builder = new ServiceBusConnectionStringBuilder(connectionString)
    builder.TransportType <- TransportType.Amqp; 
    builder.ToString(); 

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    let eventHubConnectionString = GetServiceBusConnectionString()
    let eventHubClient = EventHubClient.CreateFromConnectionString(eventHubConnectionString, "hub1")
    // Get the default Consumer Group 
    let defaultConsumerGroup = eventHubClient.GetDefaultConsumerGroup()
    let blobConnectionString = ConfigurationManager.AppSettings.["AzureStorageConnectionString"] // Required for checkpoint/state 
    let eventProcessorHost = new EventProcessorHost("singleworker", eventHubClient.Path, defaultConsumerGroup.GroupName, eventHubConnectionString, blobConnectionString)
    eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>().Wait(); 
    let s = System.Console.ReadLine(); 
    0 // return an integer exit code

