namespace Azure.SPTracker.Receiver

open System
open Microsoft.ServiceBus.Messaging
open System.Threading.Tasks

type test() = 
    let mutable context = 0

type SimpleEventProcessor() =  
    let mutable partitionContext : PartitionContext = null
    interface IEventProcessor with              
        member x.CloseAsync(context: PartitionContext, reason: CloseReason): Task = 
            printfn "Closed"
            Task.FromResult<System.Object>(null) :> Task
        
        member x.ProcessEventsAsync(context: PartitionContext, messages: Collections.Generic.IEnumerable<EventData>): Task =          
            messages |> Seq.iter(fun eventData ->
                let newData = System.Text.Encoding.UTF8.GetString(eventData.GetBytes())
                let key = eventData.PartitionKey
                Console.WriteLine(String.Format("Message received.  Partition: '{0}', Device: '{1}', Data: '{2}'", partitionContext.Lease.PartitionId, key, newData))
            )
            Task.FromResult<System.Object>(null) :> Task
                //Call checkpoint every 5 minutes, so that worker can resume processing from the 5 minutes back if it restarts. 
//                if (this.checkpointStopWatch.Elapsed > TimeSpan.FromMinutes(5)) 
//                { 
//                    await context.CheckpointAsync(); 
//                    lock (this) 
//                    { 
//                        this.checkpointStopWatch.Reset(); 
//                    } 
//                } 
//         
        member this.OpenAsync(context : PartitionContext) = 
            partitionContext <- context
            Console.WriteLine(String.Format("SimpleEventProcessor initialize.  Partition: '{0}', Offset: '{1}'", context.Lease.PartitionId, context.Lease.Offset))                        
            Task.FromResult<System.Object>(null) :> Task