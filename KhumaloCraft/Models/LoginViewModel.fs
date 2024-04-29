namespace KhumaloCraft.Models.LoginViewModel

open System.ComponentModel.DataAnnotations

type LoginViewModel() =
    // Email validation
    [<Required; EmailAddress>]
    member val Email = "" with get, set
    
    // Password validation
    [<Required; MinLength(6)>]
    member val Password = "" with get, set
