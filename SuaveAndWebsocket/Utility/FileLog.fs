namespace SuaveAndWebsocket

open System
open System.IO

module FileLog=

  let Log (msg:string)=
    use sw=new StreamWriter(Path.GetFullPath "log.txt")
    sw.WriteLine msg

