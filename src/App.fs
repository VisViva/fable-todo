module todo

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import

open Fable.Helpers.React
open Fable.Helpers.React.Props

type [<Pojo>] TodoAppProps =
    { name: String }

type [<Pojo>] TodoAppState =
    { name: String }

type TodoApp(props) =
    inherit React.Component<TodoAppProps, TodoAppState>(props)

    member this.render () =
        div [] [
            header [ ClassName "header" ] [
                h1 [] [ Fable.Helpers.React.str "todos" ]
                input [
                    ClassName "new-todo"
                    Placeholder "What needs to be done?"                    
                    AutoFocus true
                ]
            ]
        ]


let render() =
    ReactDom.render(
        Fable.Helpers.React.com<TodoApp,_,_> { name = "avs" } [],
        Browser.document.getElementById("root")
    )
render() 