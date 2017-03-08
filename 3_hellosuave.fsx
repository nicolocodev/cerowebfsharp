#r "./packages/Suave/lib/net40/Suave.dll"

open Suave

startWebServer defaultConfig (Successful.OK "Hello World!") 