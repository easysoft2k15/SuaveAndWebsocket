namespace SuaveAndWebsocket

open Suave
open Suave.Files
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open Suave.Web
open Suave.Sockets
open Suave.Sockets.Control
open Suave.WebSocket
open Suave.Utils

open Akka.FSharp

open System
open System.Net

module Webserver=
  let L (msg: string)=
    Console.WriteLine (msg)
  
  let echo (ws:WebSocket)=
    //Create Actors to send and receive msg
    //---------------------------------------
    let arefSend=ActorManager.createActor "send" (ActorManager.processFunSend ws)
    let arefReceive=ActorManager.createActor "receive" (ActorManager.processFunReceive arefSend)

    //Manage socket
    //-------------------
    fun ctx -> socket {
        let mutable loop=true
        while loop do
            let! res=ws.read()
            match res with
            | Text, data, true -> 
                L ("Data Received: " + ASCII.toString data)
                //do! ws.send Text data true
                let msg=ActorManager.Message.Msg {From="Unknow";To="Unknow";Msg=ASCII.toString data}
                arefReceive <! msg
                //arefReceive <! 
            | Close,_,_        -> 
                L  "Received Close Request"
                do! ws.send Close [||] true
                loop <- false
            | _               
                               -> ()
      }

  let StartServer()=
    let config={defaultConfig with homeFolder= Some __SOURCE_DIRECTORY__}
    let webRoot=__SOURCE_DIRECTORY__ + @"\wwwroot"

    let app=choose [
              //Html files
              GET >=> choose [
                  path "/" >=> browseFile webRoot @"\view\index.html"                
                  path "/index" >=> browseFile webRoot @"\view\index.html"                
              ]
              //Js files
              GET >=> choose [
                  pathScan "/node_modules/%s" ((fun s -> sprintf @"\js\node_modules\%s" s) >> browseFile webRoot)
                  pathScan "/out/%s" ((fun s -> sprintf @"\js\out\%s" s) >> browseFile webRoot)
              ]
              //WebSocket
              GET >=> path "/echo" >=> handShake echo

              NOT_FOUND "NOT FOUND"
            ]   

    let _,server=startWebServerAsync config app
    let cts=new System.Threading.CancellationTokenSource()
    Async.Start server,cts.Token

