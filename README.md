# Score Saver mod for the [KTaNE Bot](https://github.com/Qkrisi/ktanecord)

First, make sure you can compile and run the Bot and the tp server! More about these in the bot's readme

-Clone this repository with `git clone https://github.com/Qkrisi/tp-score-saver`

In the `Assets` folder, rename `tpData.json.template` to `tpData.json`, and modify it according to your `config.json` of the KTaNE Bot:

| Name | Description |
| - | - |
| IP | Address of the server - IP:Port |
| Name | Name of the streamer |
| pwd | Security password name  (`tpServerPass` in `config.json`)|


Assign the JSON file to the TextAsset field (`DataText`) of the `tpScoreSaver` prefab (`tpScoreSaver` component), then build the mod.
