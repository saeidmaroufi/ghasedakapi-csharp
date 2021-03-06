# Ghasedak csharp

  ghasedak C#/.NET Helper Library 

## Adding Ghasedakapi libraries to your .NET project

  The best and easiest way to add the Ghasedak libraries to your .NET project is to use the NuGet package manager.

## Package Manager
   Install-Package Ghasedak
## .NET CLI 
   dotnet add package Ghasedak
   
## Simple Send

```c#

      try
            {
                var sms = new Ghasedak.Api("apikey");
                var result = await sms.SendSMS("message", "0912xxxxxxx");
                foreach (var item in result.Items)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Ghasedak.Exceptions.ApiException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Ghasedak.Exceptions.ConnectionException ex)
            {
                Console.WriteLine(ex.Message);
            }

```

## Bulk Send
   
   ```c#
   
          try
            {
                var bulksms = new Ghasedak.Api("apikey");
                var res = bulksms.SendSMS("message", "linenumber", new string[] { "0912xxxxxxx","0937xxxxxxxx" });
                foreach(var item in res.Items)
                {
                    Console.WriteLine("messageids:" + item);
                }
            }
            catch (Ghasedak.Exceptions.ApiException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Ghasedak.Exceptions.ConnectionException ex)
            {
                Console.WriteLine(ex.Message);
            }
```

## Verify 

```c#

     try
            {
                var sms = new Ghasedak.Api("apikey");
                var result = sms.Verify(1, "template", new string[] { "0912xxxxxxx", "0937xxxxxxxx" }, "test", "test2");
                foreach (var item in result.Items)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Ghasedak.Exceptions.ApiException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Ghasedak.Exceptions.ConnectionException ex)
            {
                Console.WriteLine(ex.Message);
            }

```
