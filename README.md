# Jobsity

### Main tools and libraries

- ASP.NET Identity - for authentications
- SignalR - for live chat websocket 
- SqlLite - just to store user credentials
- ASP.NET Razor - to make the front-end part as simple as possible
- Everything else was made with native asp net core libraries.
- Chat history has not been saved on databases, but there is a cache for 50 messages.

### What is missing?
- Rabbitmq implementation for the bot, so it would be fully decoupled from the main app.

### Requirements
Docker

- If it is a new docker machine on windows you may need to change "experimental" to true on Docker Engine Settings.

### Running
Open main folder and use: 
```docker-compose up -d --build``` 

Open a browser and navigate to:
```http://localhost:8000```

You can also run it using visual studio or visual studio code default compilers.

Create a new user with a username and password, then you will be able to use the chat.

Enjoy!
