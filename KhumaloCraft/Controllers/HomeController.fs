namespace KhumaloCraft.Controllers

open System
open System.Collections.Generic
open System.Diagnostics
open System.Threading.Tasks

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

open KhumaloCraft.Models

type HomeController(logger: ILogger<HomeController>) =
  inherit Controller()

  member this.Index () = this.View()  
  member this.About () = this.View() 
  member this.Contact () = this.View()  
  member this.MyWork () = this.View()  
  member this.product1 () = this.View() 
  member this.product2 () = this.View() 
  member this.product3 () = this.View() 
  member this.Login() = this.View()  

  [<ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)>]
    member this.Error () =
        let reqId = 
            if isNull Activity.Current then
                this.HttpContext.TraceIdentifier
            else
                Activity.Current.Id

        this.View({ RequestId = reqId })