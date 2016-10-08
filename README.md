---------------------------------
# GENERAL
---------------------------------

+ FABLE - https://fable-compiler.github.io/
+ SUAVE - https://suave.io/
+ AKKA.NET - http://getakka.net/docs/FSharp%20API

This is an example of how use the SUAVE+FABLE+AKKA.NET mix.
It's a very simple application intended mainly for my future reference.
It basically does nothing but it uses a mix of technology I'm interested in:
- web server + socket (Suave)
- client side FSharp (Fable)
- Actor System (Akka.NET)

> Please note: I'm using "F# Power Tools" to use folder inside my solution.

---------------------------------
## FILES/FOLDER DESCRIPTION
---------------------------------

### 'Program.fs'
Application entry point

### Webserver.fs
Very simple web server which serves:
1. static file (*.html + *.js) in order to support clint site scripting
2. websocket in order to allow realtime communication with clients

### ActorManager.fs
It is responsible for creating Actors.
There are 2 actors each ones with it's own receiving function attached:
1. processFunReceive: Is the function related to the RECEIVE ACTOR
2. processFunSend: Is the function related to the SEND ACTOR
The RECEIVE ACTOR get messages sent by the Echo function that is directly connected to the WebSocket.
The SEND ACTOR get messages from the RECEIVE ACTOR and deliver them directly to the WebSocket.
One point of interest is the fact that while creating the actor, the function (1) and (2) get some additional parameters (the function (1) get an IActorRef of the SEND ACTOR and the function (2) get the WebSocket used to send messages to the client). This parameters get captured as a closure in the called functions. Moreover this function return an (f:Actor<Message>->Cont<Message,'b>) that is used by the actor creator.

wwwroot
It has two folder:
..* /view: containig the *:HTML files
..* /js: containing *.FSX files.
The FSX files are FSharp files that get compiled into javascript by FABLE
	
---------------------------------
# WORKFLOW
---------------------------------

## CLIENT SIDE
Write Fsharp code inside Visual Studio.
Open a CMD onto the "/wwwroot/js" folder and lunch the command:
Fable -w
this make Fable watch for changes in FSX files and compiles them into javascript every time they change

## SERVER SIDE
Write Fsharp code and compile inside Visual Studio (usual stuff)




