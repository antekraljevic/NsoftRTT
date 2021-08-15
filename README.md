# NsoftRTT

First run Capturer to listen for connections, e.g.:
dotnet run RTTCalculator -cptr -port 13000 -bind 127.0.0.1

Then run Thrower to send messages to Capturer, e.g.:
dotnet run RTTCalculator -thrw -target 127.0.0.1 -port 13000 -mps 100 -size 200
