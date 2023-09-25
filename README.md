# Toggle Mute Input Device (TMID for shorts... yeah I'm not good ad naming)
I made this project to easly enable and disable the input device I use to listen to my Switch while gaming with the audio through my pc.
Feel free to use it as you like.
I hope to have made it quite modular so it's relatively easy to change and adapt to your need.
The important stuff are:
* you can select your device in 3 ways:
  1. Passing a string argument using the CLI
  2. With a file named "DeviceName.txt"
  3. By selecting from the program itself
    * Note: Those are the order of importance I decided matter, if you pass an argument the file will be ignored. If the method used to search for the device doens't find it, it will prompt to choose from you're current active ones
* No other stuff
* Really, It's a simple program, you can check it out yourself. It's just a single file that do all the work.

I may return to it in the future if I find myself in need to make some adjustment but for now it work as I intend.
