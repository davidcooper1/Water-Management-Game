# Water Management Game

## Video Demonstration
https://www.youtube.com/watch?v=PvTv1Ti4qYc

## How to Install the Windows 10 App
1. Download the repository ZIP.
2. Unzip the repository.
3. Open the City_X.X.X.X_Master_Test folder.
4. Install everything in the Dependencies folder like any other application.
5. Trust the certificate associated with the City_X.X.X.X_x86_Master.appxbundle.
  * Right click City_X.X.X.X_x86_Master.appxbundle.  
  *  Click Properties.
  * Go to the Digital Signatures tab.
  * Select the signature then click Details.
  * Click View Certificate.
  * Click Install Certificate.
  * Select the Local Machine option and click Next.
  * Select "Place all certificates in the following store" and click Browse.
  * Scroll down and select "Trusted People" then click OK.
  * Click Next then Finish.
  * Close all other dialogs by clicking OK.
6. Install the City_X.X.X.X_x86_Master.appxbundle like any other application.
7. Print out this image to use for the AR target of the application.
![Image Target](http://www.vergium.com/wp-content/uploads/2017/04/vuforia_stones_vergium.jpg)

## Project Report

### Things that we learned...
* Creating a Unity Project
* C#
* Vuforia
* Building a Windows 10 App
* Group contributions on Github

### Biggest Issues
* Breaking project when trying to copy assets.
* No redo button and cannot undo certain changes.
* Exporting a Vuforia application (can only export as Windows 10, iOS, or Android App).
* Could not build for mobile devices.
    - Required Andriod tablet or XCode and a Mac.
* Merging code to make finished game.
* Pushing lots of files to Github.
* Creating event generation script.
* Getting the App Signed.

### Contributors
* David Cooper
* Tien Dang
* Olisa Omekam 

### Work Distribution

* David Cooper
  - City Modeling
  - Building App
* Tien Dang
  - Script for Game Functionality
* Olisa Omekam
  - UI

### How the Game Works
* The player must meet the water consumption needs of the city through a turn base game.
  <br>![Var Pane](https://puu.sh/A3XN0/8bf1aef9d9.png)
  - i.e. Water distribution and supply must meet or exceed water consumption needs.
  - Water consumption is dependent upon temperature and population
    * Higher temperature and population means higher water consumption, and vice versa.
  - The player loses a life for each turn this requirement is not fulfilled.
  - After the player loses 10 lives then the game is over.
  - The game is considered won when the player has lasted until turn 50.
* The player can invest in different infrastructure to manipulate water consumption:
  <br>![UI Buttons](https://puu.sh/A3XM0/5f0da2bc12.png)
  - Water Tower 
     * Investments increases water distribution maximum.
  - Water Sources 
     * Investments increases water availability, the maximum ammount to be drawn from each source.
     * Availability cannot exceed the water source's reserve.
  - Water Shipments
     * Investment make available a supply of water that is not refilled by rain.
* Money comes in the form of taxes each turn and varies based on the population amount.
* Different events can occur that affect the flow of gameplay.
  <br>![Event Pane](https://puu.sh/A3Y2G/dbca60f760.png)
  - These events affect:
    * Populaion through migration events.
    * Temperature through cold and warm fronts.
    * Rain frequency through droughts and monsoons.
  - Water sources' reserves are replenished slightly on rain events.

### Modeling the City
* City was built using two different Low-Poly European City Packs that were free on the Unity Asset Store.
  - [Decorations Set](https://assetstore.unity.com/packages/3d/environments/urban/lowpoly-modern-city-decorations-set-66070)
  - [Buildings Set](https://assetstore.unity.com/packages/3d/environments/urban/lowpoly-modern-city-buildings-set-64427)
  ![City SS](https://puu.sh/A3XOb/033b99b502.png)
* Everything lies on a terrain with a hole left for the lake.
* Lake water was made using Advanced Water prefab in the unity standard assets package.
* Cloud was a model downloaded off Sketchfab. [Cloud Model](https://sketchfab.com/models/116f49c23c4347eba340d0f59b0601f7)
* Rain was created using the Unity Particle System.
  <br>![Rain Image](https://puu.sh/A3XPz/212214c353.png)
* The Lake moves up and down based on the amount of water in it.
