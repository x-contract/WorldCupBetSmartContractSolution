# WorldCupBetSmartContractSolution

The WorldCupBetSmartContractSolution is a smart contract base blockchain game that will be running on NEO infrstructure. This smart contract allows players bet soccer team by chat robot before fixture start.

## Data Source

All fixture odds data comes from https://www.macauslot.com. Macauslot is a traditional gambling website. It offers varity type of sport match games. Actually, the smart contract doesn't calculate odds. It retrieves odds data from macauslot website. It means that smart contract does not running a soccer gambling game. It just simulate soccer gambling game on blockchain.

## The Smart Contract

This smart contract offer some of methods that can help players bet soccer team during world cup 2018:
+ uint ApplyChips(byte[] address)

This method used for player to apply for WCC Chips by daily. Initial amount is 20000, fixed amount 1000
to encourage continusly watch by daily.

+ bool InputMatchResult(byte[] matchResultList)

This method used for administrator input result of soccer matches after a round completed.

+ PushOddsData(byte[] oddsData)

Input formatted binary odds data for players bet.

+ bool PushOddsList(string oddsList)

Input CSV format odds data for players bet.

+ BalanceOf(byte[] address)

Get the balance of specified player address.

+ string GetOddsList()

Retrieve world cup odds information for conversion.

+ CollectAward(byte[] address)

Get bet award to specified player address. 
It will affect balance of chips specified address.

+ byte[] Bet(byte[] address, int fixtureID, int betType, uint amount)

Player bets match.
address: player address
fixtureID: The ID of soccer match
betType: 0x01 home team win, 0x02 away team win, 0x03 draw, 0x04 Home win(Concede), 0x05 Away win(Concede)
amount: bet chips amount

+ bool Reset()

Clean all odds data and match result.

+ bool ResetAccount(byte[] address)

address: The player address.
Clear specified player's account data.

+ CollectAward(byte[] address)

Player awards.
address: The address of players.

## The Storage

NEO platform offers storage ability for smart contract developers. Player's information can be stored into blockchain. Below describe how a player information stored into block:

The structure of byte storage


Due to complication of World Cup game, we have to store a lot of data. Below information describe the 
serialization of byte array.

1 byte                   4 bytes uint array        4 bytes uint array             4 bytes uint array           bet record array
The length of balance  | The balance amount   |  Last apply chips timestamp   |  The amount of bet records   | The bets record.......

A sample of bet record:
4 bytes           1 byte (Win:0x1,Lose:0x2,Draw:0x3)         1 byte              4 bytes       4 bytes
Match ID     |               bet type                  |  is Calculated    |   odds x 1000 |  Win Amount   

MatchResult array(6 bytes):
4 bytes              1 byte              1 byte
Match ID     |   TeamA Score     |   TeamB Score
