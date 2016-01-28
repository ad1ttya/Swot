module Swot

open System.IO
open System.Text

///Splits the host and domain part of given url or email
let parts (emailOrDomain: string) : list<string> =
    let lower = emailOrDomain.Trim().ToLower()
    let host = 
        if lower.Contains("http") && lower.Contains("www") then
            lower.Substring(lower.IndexOf(".") + 1)
        elif lower.Contains("http") then
            lower.Substring(lower.LastIndexOf("://") + 3)
        else
            lower.Substring(lower.LastIndexOf('@') + 1)
    let d = host.Substring(host.IndexOf('.') + 1)
    [host; d;]

//need to update with StringBuilder

let findSchoolNames (parts: list<string>) : string =
    let fileName = parts.Head.Split('.') |> List.ofArray |> List.head
    let ext = parts.Item(1).Split('.')
    if ext.Length = 1 then
        if File.Exists("lib/domains/" + ext.[0] + "/" + fileName + ".txt") then
            let school = File.ReadAllText("lib/domains/" + ext.[0] + "/" + fileName + ".txt")
            school.Trim()
        else ""
    else
        if File.Exists("lib/domains/" + ext.[0] + "/" + ext.[1] + "/" + fileName + ".txt") then
            let school = File.ReadAllText("lib/domains/" + ext.[0] + "/" + ext.[1] + "/" + fileName + ".txt")
            school.Trim()
        else
            ""

///Returns the schoolName for a given domain
let schoolName (domain: string) = findSchoolNames (parts domain)

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

///Checks whether a given domain or email belongs to an academic institution
let isAcademic (email: string) =
    let parts = parts email
    if (isBlacklisted(email) |> not) && (isTopLevelDomain(email)) || (findSchoolNames(parts).Length > 1) then
        true
    else
        false