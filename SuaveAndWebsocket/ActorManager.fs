namespace SuaveAndWebsocket

open Akka.FSharp
open Akka.Actor
open System
open Suave.Sockets
open Suave.Sockets.Control
open Suave.WebSocket
open Suave.Utils;

module ActorManager=
  let L (msg: string)=
    Console.WriteLine (msg)

  let actorSystem=Configuration.load() |> System.create "ActorSystem" 
  
  //Factory to create Actors
  //------------------------------
  let createActor name processFunMsg=
    let aref=spawn actorSystem name processFunMsg
    aref
  
  type TextMessage={From:string ; To:string; Msg:string}
  type SystemMessage={From:string ; To:string; Msg:string}

  type Message=
    | Msg of TextMessage
    | Sys of SystemMessage

  //Receive 
  let processFunReceive(arefSend: IActorRef): Actor<Message>->Cont<Message,'b>=
    fun mailbox ->
      let rec loop()=actor {
        let! msg=mailbox.Receive()
        L "Actor Receive got a message!!"
        match msg with
        | Msg x as y -> arefSend <! y
        | _ -> L "Receive Actor: Msg Unknow"
        return! loop()
      }
      loop()

  //Send 
  let processFunSend(ws: WebSocket): Actor<Message>->Cont<Message,'b>=
    fun mailbox ->
      let rec loop()=actor {
        let! msg=mailbox.Receive()
        L "Actor Send got a message!!"
        match msg with
        | Msg x -> 
          let s=socket {
            let! res=ws.send Text (ASCII.bytes ("From:"+x.From+" To: "+x.To+" Msg="+x.Msg)) true 
            return res;
            }
          Async.StartAsTask s |> ignore
        | _ -> L "Send Actor: Msg Unknow"
        return! loop()
      }
      loop()

        
