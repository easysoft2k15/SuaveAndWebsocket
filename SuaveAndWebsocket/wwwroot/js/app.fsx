#r "node_modules/fable-core/Fable.Core.dll"
open Fable.Core
open Fable.Import.Browser

module client=
  let (?) (doc: Document) name : 'R =
    doc.getElementById name :?> 'R
      
  let L (msg: string)=console.log msg
  let url="ws://localhost:8083/echo"
  let ws=WebSocket.Create url

  let Close e=
    L "WebSocket Close ..." 
    null

  let Open e=
    L "WebSocket Open ..." 
    null
  
  let Error e=
    L "WebSocket Error ..." 
    null
  
  let Message (e: MessageEvent)=
    L "WebSocket Message ..." 
    L (e.data.ToString())
    null

  let init()=
    ws.addEventListener_close (fun e -> Close e)
    ws.addEventListener_open (fun e -> Open e)
    ws.addEventListener_error (fun e -> Error e)
    ws.addEventListener_message (fun e -> Message e)

    let textMsg: HTMLTextAreaElement=document?textMsg
    //let textMsg: HTMLInputElement=document.getElementById "textMsg" :?> HTMLInputElement

    textMsg.value<-"test"

    let btnSend=document.getElementById "BtnSend"
    btnSend.onclick  <- fun e -> L textMsg.value; ws.send textMsg.value; null

    let btnClose=document.getElementById "BtnClose"
    btnClose.onclick <- fun e -> ws.close() ; null

//MAIN
//------------
client.init()

            