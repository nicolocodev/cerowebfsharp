(*
    Quotations:

    todo: run and use
*)

open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open Microsoft.FSharp.Quotations.DerivedPatterns

let recognizePlus quotation =
    match quotation with
    | SpecificCall <@ (+) @> (types, exprs, ls) -> 
        printfn "Plus! types=%A  exprs=%A ls=%A" types exprs ls
    | _ -> printfn "something else"










(*
    units of measure
*)

[<Measure>] 
type cm

[<Measure>] 
type mtrs =
    static member toCms (m : float<mtrs>) : float<cm> =
        m * 100.0<cm/mtrs>












(*
    type providers
    todo: talk about black magic: compilation services 
*)

#r "packages/FSharp.Text.RegexProvider/lib/net40/FSharp.Text.RegexProvider.dll"

open FSharp.Text.RegexProvider



[<Literal>]
let emailPattern = "^(?<UserName>[a-zA-Z\.]+)@(?<Domain>[a-zA-Z\.]+)\.(?<TopLevelDomain>[a-z]{3})$" 

//create a type
type EmailRegex = Regex<emailPattern>

// get an instance of the type
let emailR = EmailRegex()

//magic happens
let username = emailR.TypedMatch("nicolas@dominio.com").UserName.Value






















(*
    Mailbox Proccessor:

    talk about CQRS
*)

type Agent<'T> = MailboxProcessor<'T>

let agent = Agent.Start(fun (inbox:Agent<string>) ->  
    let rec loop() =
        async {
            let! msg = inbox.Receive()
            match msg with
            | "stop" -> printfn "Stopping agent"
            | _ -> 
                printfn "Message recieved: %s" msg
                return! loop() 
        }
    loop() 
)

let post (agent:Agent<'T>) message = agent.Post message






















(*
    Active patterns:
    - Partial Active Patterns
    - Parameterized Active Patterns
    - https://docs.microsoft.com/en-us/dotnet/articles/fsharp/language-reference/active-patterns
*)

open System

let (|Even|Odd|) input = if input % 2 = 0 then Even else Odd

let testNumber input =
   match input with
   | Even -> printfn "%d is even" input
   | Odd -> printfn "%d is odd" input

// Partial Active Patterns
let (|Int|_|) s =    
    match Int32.TryParse(s) with
    | (true,int) -> Some(int)
    | _ -> None

let (|Foo|_|) s =    
    match Int32.TryParse(s) with
    | (true,int) -> Some(int)
    | _ -> None