// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2016 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}

namespace WebSharper

open WebSharper.JavaScript

[<Proxy(typeof<list<_>>)>]
[<Name "WebSharper.List.T">]
[<DefaultAugmentation(false)>]
type private ListProxy<'T> =
    | Empty
    | Cons of Head: 'T * Tail: List<'T>

    [<Name "Construct"; Pure>]
    static member Cons(head: 'T, tail: list<'T>) = head :: tail

    [<Name "Nil"; Pure>]
    static member Empty : list<'T> = []

    member this.Head with [<Inline>] get () = List.head (As this) : 'T
    member this.Tail with [<Inline>] get () = List.tail (As this) : list<'T>
    member this.IsEmpty with [<Inline "$this.$ == 0">] get () = X<bool>

    member this.Length with get () = List.length (As this)

    member this.Item with get (x: int) : 'T = List.nth (As this) x

    interface System.Collections.IEnumerable with
        member this.GetEnumerator() = (this :> _ seq).GetEnumerator() :> _

    interface seq<'T> with
        member this.GetEnumerator() =
            let data = As<list<'T>> this
            Enumerator.New data (fun e ->
                match e.State with
                | x :: xs ->
                    e.Current <- x
                    e.State <- xs
                    true
                | [] ->
                    false)

    member this.GetSlice(start, finish) : list<'T> =
        match start, finish with
        | None, None -> As this
        | Some i, None -> As this |> CollectionInternals.ListSkip i
        | None, Some j -> As this |> Seq.take (j + 1) |> List.ofSeq  
        | Some i, Some j -> As this |> CollectionInternals.ListSkip i |> Seq.take (j - i + 1) |> List.ofSeq        
