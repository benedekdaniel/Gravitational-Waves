# Gravitational Waves

At Ian's Request: DO NOT DELETE BRANCHES EVEN ONCE THEY ARE DONE

# Feature lifecycle

1. Create Issue for Feature/Bug
2. Assign weight to the issue during sprint planning
3. Someone picks it up (assigns themselves to it) during the sprint
4. They create a feature branch for it using the format [issue number] - ['feature'/'bug'] /[description]
5. Development....
6. Create Pull Request from feature branch to develop
7. Ticket goes into review.
7. Another developer picks it up for Code Review
8. If any suggestions, write them in the comments, once all the issues are resolved, Approve the pull request
9. Press merge
10. Ticket moves into Done

Steps to take to deploy the project:

    1. First the user needs to build a dekstop version that will act as the host platform of the application. For that follow these steps:
        - Open the application in Unity
        - Do a desktop build by going into: File -> Build Settings
        - Select PC as the platform
        - Click Build and select a folder where the build will be placed (It is advised to create a separate folder for each build like webBuild and desktopBuild)
        - Click build
        - This should take a few minutes
        - The desktop build should ready

    2. Do a web-build by going into: File -> Build Settings then:
        - Select WebGL as the platform (Switch Platform)
        - Click build
        - Select a folder for the build
        - This should take longer to build
        - The web build should be ready

    3. Run a web server that is going to act as the client platform by opening the cli and typing: `python -m http.server --cgi 8360` then:
        - Open up a browser and open `localhost:8360`
        - This should open up the webGL server

    4. The host of the server can now host a game by clicking start in the main menu then:
        - Click host a game and create game
    
    5. User who would like to join the lobby:
        - Click join game
        - Click start
        - And select the server with the appropriate game code that has been generated for the hosted server.
