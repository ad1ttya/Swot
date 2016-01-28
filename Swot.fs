module Swot

open System.IO

let parts (emailOrDomain: string) : list<string> =
    let lower = emailOrDomain.Trim().ToLower()
    let host = 
        if lower.Contains("http") then
            lower.Substring(lower.LastIndexOf("://") + 3)
        else
            lower.Substring(lower.LastIndexOf('@') + 1)
    let d = host.Substring(host.IndexOf('.') + 1)
    [host; d;]
    
let blackLists = File.ReadAllLines "lib/domains/blacklist.txt" |> List.ofArray

///Returns Whether a domain is blacklisted (true) or not (false)
let isBlacklisted (domain: string) =
    let domainParts = parts domain 
    List.exists (fun i -> i = domainParts.Head) blackLists
    
let tlds = File.ReadAllLines "lib/domains/tlds.txt" |> List.ofArray

///Returns Whether a domain is a top level domain (true) or not (false)
let isTopLevelDomain (domain: string) =
    let domainParts = parts domain
    List.exists (fun i -> i = domainParts.Item(1)) tlds