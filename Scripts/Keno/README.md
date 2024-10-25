# BEHAVIOUR.cs:
## STOPS AND WAITS
`interupt()` stops whatever Keno is doing, but be warned that it does not mess with `Entertained` and `ProceedTrue`, so you'll have to manually change those.
`uninterupt()` undoes what `interupt()` does, resulting in him returning to normal. `PR` should be disabled (`PR.SetActive(false)`) in events, and set back to active once the event ends.

`pause()` is responsible for the main wait in `update()` that makes the system wait before playing the next automated animation(s).

## VARIABLES
`whatToDo` usually handles determining what animation to play. `variant` is for if there is more than one choice needed.
`attitude` is how he feels in the moment (aka his mood; for example: mad, happy, sad, etc).

`waitForBoredom` is the float responsible for specifying how long Keno should wait before asking to play a game. This can be assigned in the inspector.
`isBored` handles sending off the Game Prompt.
~~`Entertained` handles stopping `waitForBoredom` (sorta) and preventing the play game dialog from showing. This should be used when a menu or activity is open.~~ `Entertained` has been replaced by `interupted`. `interupted` is theoretically simpler
`GameReqOn` handles preventing Game Requests when they are turned off.

`ProceedTrue` is for waiting until the user chooses an option.
`choice` temporarily stores what the user chose on an interface that had several options. Usually paired with `ProceedTrue`

# MISC NOTES
Keno uses `Unity` Version `2023.2.20f1`.
Keno's eye animations were made in FlipAClip. Find more details here: `https://drive.google.com/drive/u/0/folders/1g2JWixMCNBfmGVO0Fj2afdTYAIZWnxnO`.
Random commented lines that are part of script can be ignored, as they are likely there for future reference or there from past fails.

For more help and/or assistance, contact `YrekcazContact@gmail.com` or join the discord server `https://www.discord.gg//HcG84gbmUA`.

There is info missing here. This is a WIP, and help is greatly appreciated.

# KNOWN ISSUES
Sometimes upon ending the `Reading` mode, it will skip past the end animation and go straight back to normal without playing the end animation.

