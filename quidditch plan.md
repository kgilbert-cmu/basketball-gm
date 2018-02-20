Quidditch Overhaul of Basketball GM code

Positions

3 Chasers (goal-scoring forward types) : get points
2 Beaters (off. protectors, def. stoppers) : stop points, help chasers, help seekers
1 Keeper (guards against scoring by Chasers)
1 Seeker (150 pt scorer / game ends)

Balls

Quaffle - like basketball or soccer ball for normal scoring
Golden Snitch - Seekers try to get it. Possession = Goal
Bludger - weapon ball to try to hit / hurt / distract opponents.


### Brainstorming
	
How do Quidditch players score (Quaffle)


Fast manuevers, like a good dribbling ISO game.
Good Passing & Shooting, like a good Spread the Floor 3p shooting game.
Power Maneuvers, like Low Post, taking a Bludger

### Quidditch Transformation Steps

1. Naming / Slight League Tweaks
2. Simple Changes / Seekers (3p) 
3. Revamp Game Sim to have new ratings / modeling / still roughly basketball inspired.
4. Revamp League, make Scholastic Divisions, Rec divisions have their own limitations, 
and recruiting from Scholastic into Pro division. Retirements from Pro / Recruitment from Scholastic into Ministries / Department casual leagues.
5. Revamp Game Sim more. Game Ends by Snitch or Mutual Agreement. Adjustment Snitch difficulty by Division.

### Naming / Slight League Tweaks

Generate naming from British name lists (80%), German name lists (10%), French name lists (10%)

1. Implemented British Name lists with a 1000 Male First Names, 1000 Female First Names, 1000 ish Last Names, all England / Wales sourced.
   Processed from Web source, through Excel formulas into the JSON format and copied in to harrypotter_teams.json.
2. Implemented Scholastic, Rec and Pro Conferences. Unfortunately, I have Irish and Bulgarian national teams within the British Premier League.
   TODO: Add British / Irish and Bulgarian leagues as divisions within pro. Add some British Premier League team names, Irish soccer teams, Bulgarian teams if possible?
   TODO: Make Logo art for these new teams.
   MAYBE: International Competition Conference, with England, Wales, Ireland, Bulgaria and other National Teams?
   OR: Cut out the Irish and Bulgarian national teams entirely, and find a team for the well-known players like Viktor Krum.
   
Note: Started a regular BBGM playthrough as the Chudley Cannons with this version of the JSON
The names are MUCH better than before. Still want to add German and French name bases (irish and bulgarian too?)
   
### Simple Changes / Seekers (3p)

Change logic for the end of games, to be based on completed 3p percentage
Make 3p attempts 10 - 30 times harder to make (how many 3s per game now?)
Make only the 1st starter able to take 3s / or make them. (basically Seeker is first position, like the QB in the NFL / College Football simulators)

Specifically:

1 (done): If Quarter = 5, Quarter = 4 (don't end the regulation until a 3 is made)

If stats for team0 tp > 0, then break from regulation.
If a tie happens after that, then do overtime normally.

2 PTers are 10 points.
3 PTers are 150 points.

Make all 3 base chances for 3pts 1 tenth of normal values.

