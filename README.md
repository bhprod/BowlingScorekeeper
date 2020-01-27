# BowlingScorekeeper
A simple score keeper for bowling, created in WPF/C# using the MVVM design pattern.

Usage:

Keep score of an individual player's bowling scores by entering the number of pins hit in the textboxes for each corresponding frame.
You can add player names and create as many score tracks as you wish, so scoring is possible for multiple players.
Enter 'x' for a strike, '/' for a spare, '-' for a miss, otherwise type the number for how many pins were hit.
Strikes count as 10 points plus the next two rolls. If the next frame is also a strike then the first roll of the corresponding frame will be scored.
Spares count as 10 points plus the next roll.
Open frames (no strikes or spares) count only the number of pins hit.
After the score is entered the textboxes for each frame are disabled to prevent errors.
You may also clear every track by pressing the "Clear Board" button.
Because the score is updated in real time, an incomplete game will still score up to the last point value entered.

How to open:

Download and extract the zip, then open in visual studio and build solution.

Challenges & tests:

The specific challenge in creating this project was real-time updating of scores and using the MVVM pattern without a 3rd party library.
Unit tests are included for the two model classes containing logic.

Notes & credit:
Correct score for test case references were from https://www.bowlinggenius.com/
