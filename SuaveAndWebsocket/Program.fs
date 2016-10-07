namespace SuaveAndWebsocket
open System

module MainEntry=

    [<EntryPoint>]
    let main argv = 
    
        Webserver.StartServer() |> ignore
        Console.ReadKey() |> ignore

        0 // return an integer exit code
