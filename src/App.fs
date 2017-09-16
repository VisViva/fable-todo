module todo

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import

module Util =
    let load<'T> key: 'T option =
        !!Browser.localStorage.getItem(key)
        |> Option.map (fun json -> !!JS.JSON.parse(json))

    let save key (data: 'T) =
        Browser.localStorage.setItem(key, JS.JSON.stringify data)

type Todo = { id: Guid; title: string; completed: bool }

type TodoModel(key) =
    member val key = key
    member val todos: Todo[] = defaultArg (Util.load key) [||] with get, set
    member val onChanges: (unit->unit)[] = [||] with get, set

    member this.subscribe(onChange) =
        this.onChanges <- [|onChange|]

    member this.inform() =
        Util.save this.key this.todos
        this.onChanges |> Seq.iter (fun cb -> cb())

    member this.addTodo(title) =
        this.todos <-
            [|{ id=Guid.NewGuid(); title=title; completed=false }|]
            |> Array.append this.todos
        this.inform()

    member this.toggleAll(checked') =
        this.todos <- this.todos |> Array.map (fun todo ->
            { todo with completed = checked' })
        this.inform()

    member this.toggle(todoToToggle) =
        this.todos <- this.todos |> Array.map (fun todo ->
            if todo.id <> todoToToggle.id
            then todo
            else { todo with completed = (not todo.completed) })
        this.inform()

    member this.destroy(todoToDestroy) =
        this.todos <- this.todos |> Array.filter (fun todo ->
            todo.id <> todoToDestroy.id)
        this.inform()

    member this.save(todoToSave, text) =
        this.todos <- this.todos |> Array.map (fun todo ->
            if todo.id <> todoToSave.id
            then todo
            else { todo with title = text })
        this.inform()

    member this.clearCompleted() =
        this.todos <- this.todos |> Array.filter (fun todo ->
            not todo.completed)
        this.inform()

open Fable.Helpers.React.Props
