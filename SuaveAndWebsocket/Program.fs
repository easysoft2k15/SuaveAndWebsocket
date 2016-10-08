namespace SuaveAndWebsocket

open System

module MainEntry=
  let getConfig (argv:string array)=
    match argv with
    |[||] -> 8083,@"\wwwroot"
    |[|port;webroot|] -> int port,webroot
    |_ ->  8083,@"\wwwroot" 

  [<EntryPoint>]
  let main argv = 
  
    FileLog.Log "Msg from main program!!!"
    
    let port,webroot=getConfig argv
    (Webserver.StartServer port webroot) |> ignore
    Console.ReadKey() |> ignore
  
    0 // return an integer exit code
