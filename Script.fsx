#r @"C:\Workspace\FSharp\Swot\bin\Debug\Swot.dll"

open System.IO
open Swot

if isTopLevelDomain "http://hot.ac.gg" then
    printfn "It is a top level domain"
else
    printfn "not a top level domain"
    
parts "http://www.hotmail.com"