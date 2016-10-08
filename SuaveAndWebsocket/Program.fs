namespace SuaveAndWebsocket
open System

module MainEntry=

    [<EntryPoint>]
    let main [|port;staticFilePath|] = 
    
        (Webserver.StartServer port) |> ignore
        Console.ReadKey() |> ignore

        0 // return an integer exit code
