﻿Imports System.IO


Module Module1
    Public fightTurn As Integer = -1

    Function round(ByVal num As Double) As Integer
        If num > 0 Then
            Return Math.Floor(num + 0.5)
        Else
            If num = 0 Then
                Return 0
            End If
            Return Math.Floor(num - 0.5)
        End If
    End Function
    Class BasePerson
        Public Health As Integer
        Public MaxHealth As Integer
        Public Attack As Integer
        Public Heals As Integer = 5
        Public MaxHeals As Integer = 5
        Public Defence As Double
        Public Value As Integer
        Public Heal As Integer
        Public DefenceBoost As Double
        Public attackBoost As Integer
        Public usedPower As Boolean = False
        Public isDefending As Boolean = False
        Public Name As String
        Public Price As Integer

        'If you change attack change the move Opponent power check script

        Public Overridable Sub power()
            '
        End Sub

        Public Overrides Function ToString() As String
            Return $"{Name}: [{vbNewLine}Health: {Health},{vbNewLine}Attack: {Attack},{vbNewLine}Max Health: {MaxHealth},{vbNewLine}Defence: {Defence * 100}%,{vbNewLine}isDefending: {isDefending},{vbNewLine}Heal: {Heal}{vbNewLine}]{vbNewLine}"
        End Function

        Sub ShowStats(ByVal opponent As Object)
            'Console.WriteLine($"{Name}:{vbNewLine}Health-{Health},{vbNewLine}Defence-{If(isDefending, $"{(Defence + DefenceBoost) * 100}% - {(opponent.attack + opponent.attackBoost) * (1 - (Defence + DefenceBoost))} -- {round((opponent.attack + opponent.attackBoost) * (1 - (Defence + DefenceBoost)))}", "0%")},{vbNewLine}Attack: {Attack + attackBoost}{vbNewLine}")
        End Sub

    End Class

    Class BasicKnight
        Inherits BasePerson

        Public Overrides Sub power()
            If Health < round(MaxHealth * 0.5) Then
                Health = round(MaxHealth * 0.5)
            End If
            isDefending = True
            DefenceBoost = 0.7
            attackBoost = 2
        End Sub

        Sub New()

            Price = 10
            Health = 5
            Value = 1
            MaxHealth = 5
            Attack = 2
            isDefending = False
            Defence = 0.3
            Heal = 2
            Name = "Basic Knight"

        End Sub

    End Class

    Class TrainedSoldier
        Inherits BasePerson

        Public Overrides Sub power()
            If Health < round(MaxHealth * 0.5) Then
                Health = round(MaxHealth * 0.5)
            End If
            isDefending = True
            DefenceBoost = 0.6
            attackBoost = 2
        End Sub

        Sub New()
            Price = 20
            Health = 7
            MaxHealth = 7
            Value = 2
            Attack = 2
            isDefending = False
            Defence = 0.4
            Heal = 2
            Name = "Trained Soldier"
        End Sub
    End Class

    Class LegendaryWarrior
        Inherits BasePerson

        Public Overrides Sub power()
            If Health < round(MaxHealth * 0.5) Then
                Health = round(MaxHealth * 0.5)
            End If
            isDefending = True
            DefenceBoost = 0.7
            attackBoost = 2
        End Sub

        Sub New()
            Price = 30
            Health = 9
            MaxHealth = 9
            Value = 3.5
            Attack = 3
            isDefending = False
            Defence = 0.3
            Heal = 2
            Name = "Legendary Warrior"
        End Sub

    End Class



    Class Player
        Public first As Boolean = True
        Public cards As New ArrayList()
        Public opponentsKilled = 0
        Public coins As Integer = 35
        'Public coins As Integer = 10

        Public rating As Double = 0




        Sub updateRating()
            rating = 0
            For Each card In cards
                rating += card.Value
            Next
            rating += round(coins / 10)

        End Sub

        Sub doChoice(ByVal playerCard As Object, ByVal oppCard As Object, ByVal choice As String, Optional isOpponent As Boolean = False, Optional ByVal isComputer As Boolean = True)
            Dim defendedAttack As Integer
            Select Case choice
                Case "a"
                    If oppCard.isDefending = False Then
                        If oppCard.Health - (playerCard.attack + playerCard.attackBoost) < 1 Then
                            oppCard.Health = 0
                        Else
                            oppCard.Health -= (playerCard.attack + playerCard.attackBoost)
                        End If

                    Else
                        defendedAttack = round((playerCard.attack + playerCard.attackBoost) * (1 - (oppCard.Defence + oppCard.DefenceBoost)))

                        If Not oppCard.DefenceBoost = 0 Then
                            oppCard.DefenceBoost = 0
                        End If


                        If oppCard.Health - defendedAttack < 1 Then
                            oppCard.Health = 0
                        Else
                            oppCard.Health -= defendedAttack
                        End If
                        oppCard.isDefending = False
                    End If

                    If Not playerCard.attackBoost = 0 Then
                        playerCard.attackBoost = 0
                    End If

                Case "d"
                    playerCard.isDefending = True


                Case "h"
                    playerCard.Heals -= 1
                    If playerCard.Health + playerCard.heal > playerCard.MaxHealth Then
                        playerCard.Health = playerCard.MaxHealth
                    Else
                        playerCard.Health += playerCard.heal
                    End If
                Case "p"
                    If playerCard.usedPower Then
                        If isOpponent = False Then
                            Console.Write("You have already used your power, please pick another option.")
                            Console.ReadLine()
                            movePlayer(playerCard, oppCard, isComputer)
                        End If

                    Else
                        playerCard.power()
                        playerCard.usedPower = True
                    End If


            End Select
        End Sub
        Sub movePlayer(ByVal playerCard As Object, ByVal oppCard As Object, ByVal isComputer As Boolean, Optional ByVal playerprompt As Integer = False)
            Dim playerChoice As String
            Dim sprites As String()()

            If isComputer Then
                playerChoice = doOptionButtons(playerCard, oppCard, 23, isComputer:=isComputer)(0)(0)
                doChoice(playerCard, oppCard, playerChoice, isComputer:=isComputer)
            Else
                playerChoice = doOptionButtons(playerCard, oppCard, 23, isComputer:=isComputer)(0)(0)
                doChoice(oppCard, playerCard, playerChoice, isComputer:=isComputer)
            End If

            sprites = doOptionButtons(playerCard, oppCard, 23, True)

            displayCharAndStats(playerCard, oppCard, sprites(0), sprites(1))

            Console.SetCursorPosition(0, 0)
            If oppCard.Health < 1 Or playerCard.Health < 1 Then
                'Pass
            Else
                If Not playerprompt Then
                    oppMovePrompt()
                Else
                    playerMovePrompt()
                End If
                waitForEnter()
            End If



        End Sub

        Sub moveOpponent(ByVal playerCard As Object, ByVal oppCard As Object, ByVal Player As Player)
            Dim opponentChoice As String
            Dim opponentOptions As New List(Of String)
            Dim sprites As String()() = doOptionButtons(playerCard, oppCard, 23, True)
            Dim PlayerSprite As String() = sprites(0)
            Dim OpponentSprite As String() = sprites(1)
            Dim attackValue As Integer
            Dim pAttackValue As Integer

            opponentOptions.Clear()

            attackValue = (oppCard.Attack + oppCard.AttackBoost)

            If playerCard.isDefending Then
                attackValue = (oppCard.Attack + oppCard.AttackBoost) * (1 - (playerCard.Defence + playerCard.DefenceBoost))
            End If

            pAttackValue = (playerCard.Attack + playerCard.AttackBoost)

            If oppCard.isDefending Then
                attackValue = (playerCard.Attack + playerCard.AttackBoost) * (1 - (oppCard.Defence + oppCard.DefenceBoost))
            End If

            If playerCard.Health - attackValue <= 0 And Player.rating >= 2 Then
                opponentOptions.Add("a")
            Else

                If (oppCard.Attack + oppCard.AttackBoost) >= 4 Then
                    opponentOptions.Add("a")
                    opponentOptions.Add("a")
                    opponentOptions.Add("a")
                    opponentOptions.Add("a")
                    opponentOptions.Add("a")
                    opponentOptions.Add("a")
                    opponentOptions.Add("a")
                    opponentOptions.Add("a")

                End If

                If oppCard.Health <= round(oppCard.MaxHealth) * 0.3 And oppCard.usedPower = False Then
                    opponentOptions.Add("p")
                Else
                    If oppCard.Health - pAttackValue < 1 Then
                        If oppCard.Heals < 1 Or oppCard.Health - (attackValue * (1 - (oppCard.Defence + oppCard.DefenceBoost))) > 0 Then

                            opponentOptions.Add("d")
                            opponentOptions.Add("d")
                            opponentOptions.Add("d")
                            opponentOptions.Add("d")
                            opponentOptions.Add("d")
                            opponentOptions.Add("d")
                        Else
                            If oppCard.Health + oppCard.Heal - attackValue > 0 Then
                                opponentOptions.Add("h")
                                opponentOptions.Add("h")
                                opponentOptions.Add("h")
                                opponentOptions.Add("h")
                                opponentOptions.Add("h")
                                opponentOptions.Add("h")
                            End If
                        End If
                    End If

                    If oppCard.Health - pAttackValue < 1 Then
                        If oppCard.Heals > 0 Then
                            opponentOptions.Add("h")
                            opponentOptions.Add("h")
                            opponentOptions.Add("h")

                        Else
                            opponentOptions.Add("d")
                            opponentOptions.Add("d")
                        End If
                        opponentOptions.Add("d")

                    End If

                    If oppCard.Health >= round(oppCard.MaxHealth * 0.5) Then
                        opponentOptions.Add("a")
                        opponentOptions.Add("a")
                        opponentOptions.Add("a")
                        If oppCard.Health > round(oppCard.MaxHealth * 0.8 And Player.rating <= 5) Then
                            opponentOptions.Add("a")
                        End If
                        opponentOptions.Add("a")
                        If Not oppCard.Health = oppCard.MaxHealth Then
                            If oppCard.Heals > 0 Then
                                opponentOptions.Add("h")
                            End If
                        End If
                        If oppCard.isDefending = False Then
                            If oppCard.Health > round(oppCard.MaxHealth * 0.8 And Player.rating <= 5) Then

                            Else
                                opponentOptions.Add("d")
                                opponentOptions.Add("d")
                            End If

                        End If
                    Else

                        If oppCard.Heals > 0 Then
                            opponentOptions.Add("h")
                            opponentOptions.Add("h")
                        End If
                        If Not (oppCard.Attack + oppCard.AttackBoost) = 4 Then
                            If oppCard.Heals > 0 Then
                                opponentOptions.Add("h")
                                opponentOptions.Add("h")
                            End If
                        End If
                        opponentOptions.Add("a")
                        If oppCard.Health > round(oppCard.MaxHealth * 0.4) Then
                            opponentOptions.Add("a")
                        End If
                        If oppCard.Health > playerCard.Health Then
                            opponentOptions.Add("a")
                            opponentOptions.Add("a")
                            opponentOptions.Add("a")
                        End If
                        If oppCard.isDefending = False Then
                            If Not oppCard.Heals < 1 Then
                                opponentOptions.Add("d")
                            End If
                            opponentOptions.Add("d")
                            opponentOptions.Add("d")
                            If oppCard.usedPower = False Then
                                opponentOptions.Add("p")
                            End If
                        End If

                    End If
                End If
            End If



            If False Then
                Console.Clear()
                For Each line As String In opponentOptions
                    Console.WriteLine(line)
                Next
                waitForEnter()
            End If



            Randomize()
            opponentChoice = opponentOptions(CInt(Math.Floor((Rnd() * opponentOptions.Count) + 0)))
            showOpponentChoice(opponentChoice, playerCard, oppCard, PlayerSprite, OpponentSprite)
            doChoice(oppCard, playerCard, opponentChoice, True)
        End Sub

        Sub fight(ByVal Opp As Object, ByVal Player As Player, ByVal isComputer As Boolean, Optional ByVal cardIndex As Integer = -2763, Optional ByVal oppIndex As Integer = -27633)
            If cardIndex = -2763 Then
                cardIndex = cards.Count - 1 ' last card if card index is 2763 for testing
            End If
            Randomize()
            Dim playerCard As Object = cards(cardIndex)
            Dim oppCard As Object
            Dim randomStart As Double = Rnd()

            If isComputer Then
                oppCard = Opp.cards((Opp.cards.Count - 1)) ' gets last card of opponent 
            Else
                oppCard = Opp.cards(oppIndex)
            End If

            Dim sprites As String()() = doOptionButtons(playerCard, oppCard, 23, True) ' gets sprites array of arrays of strings
            Dim PlayerSprite As String() = sprites(0)
            Dim OpponentSprite As String() = sprites(1)

            If Not isComputer Then
                If randomStart > 0.5 Then
                    While True

                        'check if opp is dead (player wins)
                        If oppCard.Health < 1 Then
                            playerWon(playerCard, oppCard, PlayerSprite, OpponentSprite)
                            Player.opponentsKilled += 1
                            Opp.cards.Remove(oppCard)

                            Select Case oppCard.Name
                                Case "Basic Knight"
                                    coins += 10
                                Case "Trained Soldier"
                                    coins += 15
                                Case "Legendary Warrior"
                                    coins += 25
                            End Select

                            Exit While
                        End If

                        'check if player is dead (opponent wins)
                        If playerCard.Health < 1 Then
                            opponentWon(playerCard, oppCard, PlayerSprite, OpponentSprite) ' shows opponent screen

                            cards.Remove(playerCard)
                            opponentsKilled += 1

                            Select Case playerCard.Name
                                Case "Basic Knight"
                                    Opp.coins += 10
                                Case "Trained Soldier"
                                    Opp.coins += 15
                                Case "Legendary Warrior"
                                    Opp.coins += 25
                            End Select
                            Exit While
                        End If

                        If Not isComputer Then
                            fightTurn = 0
                        End If
                        movePlayer(playerCard, oppCard, True, True)



                        'repeats this check after the move

                        'check if opp is dead (player wins)
                        If oppCard.Health < 1 Then
                            playerWon(playerCard, oppCard, PlayerSprite, OpponentSprite)
                            Player.opponentsKilled += 1
                            Opp.cards.Remove(oppCard)

                            Select Case oppCard.Name
                                Case "Basic Knight"
                                    coins += 10
                                Case "Trained Soldier"
                                    coins += 15
                                Case "Legendary Warrior"
                                    coins += 25
                            End Select

                            Exit While
                        End If

                        'check if player is dead (opponent wins)
                        If playerCard.Health < 1 Then
                            opponentWon(playerCard, oppCard, PlayerSprite, OpponentSprite) ' shows opponent screen

                            cards.Remove(playerCard)
                            opponentsKilled += 1

                            Select Case playerCard.Name
                                Case "Basic Knight"
                                    Opp.coins += 10
                                Case "Trained Soldier"
                                    Opp.coins += 15
                                Case "Legendary Warrior"
                                    Opp.coins += 25
                            End Select
                            Exit While
                        End If



                        If isComputer Then
                            moveOpponent(playerCard, oppCard, Player)
                        Else
                            If Not isComputer Then
                                fightTurn = 1
                            End If
                            movePlayer(playerCard, oppCard, False, True)
                        End If


                    End While
                Else
                    While True

                        'check if opp is dead (player wins)
                        If oppCard.Health < 1 Then
                            playerWon(playerCard, oppCard, PlayerSprite, OpponentSprite)
                            Player.opponentsKilled += 1
                            Opp.cards.Remove(oppCard)

                            Select Case oppCard.Name
                                Case "Basic Knight"
                                    coins += 10
                                Case "Trained Soldier"
                                    coins += 15
                                Case "Legendary Warrior"
                                    coins += 25
                            End Select

                            Exit While
                        End If

                        'check if player is dead (opponent wins)
                        If playerCard.Health < 1 Then
                            opponentWon(playerCard, oppCard, PlayerSprite, OpponentSprite) ' shows opponent screen

                            cards.Remove(playerCard)
                            opponentsKilled += 1

                            Select Case playerCard.Name
                                Case "Basic Knight"
                                    Opp.coins += 10
                                Case "Trained Soldier"
                                    Opp.coins += 15
                                Case "Legendary Warrior"
                                    Opp.coins += 25
                            End Select
                            Exit While
                        End If


                        If isComputer Then
                            moveOpponent(playerCard, oppCard, Player)
                        Else
                            If Not isComputer Then
                                fightTurn = 1
                            End If
                            movePlayer(playerCard, oppCard, False, True)
                        End If


                        'repeats this check after the move

                        'check if opp is dead (player wins)
                        If oppCard.Health < 1 Then
                            playerWon(playerCard, oppCard, PlayerSprite, OpponentSprite)
                            Player.opponentsKilled += 1
                            Opp.cards.Remove(oppCard)

                            Select Case oppCard.Name
                                Case "Basic Knight"
                                    coins += 10
                                Case "Trained Soldier"
                                    coins += 15
                                Case "Legendary Warrior"
                                    coins += 25
                            End Select

                            Exit While
                        End If

                        'check if player is dead (opponent wins)
                        If playerCard.Health < 1 Then
                            opponentWon(playerCard, oppCard, PlayerSprite, OpponentSprite) ' shows opponent screen

                            cards.Remove(playerCard)
                            opponentsKilled += 1

                            Select Case playerCard.Name
                                Case "Basic Knight"
                                    Opp.coins += 10
                                Case "Trained Soldier"
                                    Opp.coins += 15
                                Case "Legendary Warrior"
                                    Opp.coins += 25
                            End Select
                            Exit While
                        End If





                        If Not isComputer Then
                            fightTurn = 0
                        End If
                        movePlayer(playerCard, oppCard, True, True)
                    End While
                End If

            Else
                While True

                    'check if opp is dead (player wins)
                    If oppCard.Health < 1 Then
                        playerWon(playerCard, oppCard, PlayerSprite, OpponentSprite)
                        Player.opponentsKilled += 1
                        Opp.cards.Remove(oppCard)

                        Select Case oppCard.Name
                            Case "Basic Knight"
                                coins += 10
                            Case "Trained Soldier"
                                coins += 15
                            Case "Legendary Warrior"
                                coins += 25
                        End Select

                        Exit While
                    End If

                    'check if player is dead (opponent wins)
                    If playerCard.Health < 1 Then
                        opponentWon(playerCard, oppCard, PlayerSprite, OpponentSprite) ' shows opponent screen

                        cards.Remove(playerCard)
                        opponentsKilled += 1

                        Select Case playerCard.Name
                            Case "Basic Knight"
                                Opp.coins += 10
                            Case "Trained Soldier"
                                Opp.coins += 15
                            Case "Legendary Warrior"
                                Opp.coins += 25
                        End Select
                        Exit While
                    End If

                    If Not isComputer Then
                        fightTurn = 0
                    End If
                    movePlayer(playerCard, oppCard, True)



                    'repeats this check after the move

                    'check if opp is dead (player wins)
                    If oppCard.Health < 1 Then
                        playerWon(playerCard, oppCard, PlayerSprite, OpponentSprite)
                        Player.opponentsKilled += 1
                        Opp.cards.Remove(oppCard)

                        Select Case oppCard.Name
                            Case "Basic Knight"
                                coins += 10
                            Case "Trained Soldier"
                                coins += 15
                            Case "Legendary Warrior"
                                coins += 25
                        End Select

                        Exit While
                    End If

                    'check if player is dead (opponent wins)
                    If playerCard.Health < 1 Then
                        opponentWon(playerCard, oppCard, PlayerSprite, OpponentSprite) ' shows opponent screen

                        cards.Remove(playerCard)
                        opponentsKilled += 1

                        Select Case playerCard.Name
                            Case "Basic Knight"
                                Opp.coins += 10
                            Case "Trained Soldier"
                                Opp.coins += 15
                            Case "Legendary Warrior"
                                Opp.coins += 25
                        End Select
                        Exit While
                    End If



                    If isComputer Then
                        moveOpponent(playerCard, oppCard, Player)
                    Else
                        If Not isComputer Then
                            fightTurn = 1
                        End If
                        movePlayer(playerCard, oppCard, False)
                    End If


                End While

                If Player.first Then
                    Player.first = False
                End If
            End If


        End Sub


        Sub showCards(Optional ByVal index = -1)
            If index = -1 Then
                For Each card In cards
                    Console.WriteLine(card)
                Next
            Else
                Console.WriteLine(cards(index))
            End If

        End Sub

    End Class

    Class Opponent
        Inherits Player

        Sub addRandomCard(ByVal rating As Double, ByVal player As Player)
            Dim cardOptions As New ArrayList()
            Dim randomCard As Object

            If player.first Then
                cards.Add(New BasicKnight())
                Exit Sub
            End If

            If rating <= 5 Then
                cardOptions.Add(New BasicKnight())
                cardOptions.Add(New BasicKnight())
                cardOptions.Add(New BasicKnight())
                cardOptions.Add(New BasicKnight())

                cardOptions.Add(New TrainedSoldier())
                cardOptions.Add(New TrainedSoldier())
                cardOptions.Add(New TrainedSoldier())




            ElseIf rating > 5 And rating <= 7 Then

                cardOptions.Add(New BasicKnight())

                cardOptions.Add(New TrainedSoldier())
                cardOptions.Add(New TrainedSoldier())
                cardOptions.Add(New TrainedSoldier())

                cardOptions.Add(New LegendaryWarrior())

            ElseIf rating >= 8 Then
                cardOptions.Add(New BasicKnight())

                cardOptions.Add(New TrainedSoldier())
                cardOptions.Add(New TrainedSoldier())
                cardOptions.Add(New TrainedSoldier())


                cardOptions.Add(New LegendaryWarrior())
                cardOptions.Add(New LegendaryWarrior())
                cardOptions.Add(New LegendaryWarrior())

            End If

            Randomize()

            randomCard = cardOptions(CInt(Math.Floor((Rnd() * cardOptions.Count) + 0)))
            cards.Add(randomCard)

        End Sub
    End Class

    Function startAnimation(ByVal title As String, Optional ByVal bgColor As ConsoleColor = ConsoleColor.Black, Optional ByVal fgColor As ConsoleColor = ConsoleColor.White) As Boolean
        Const Delay As Double = 45
        Console.BackgroundColor = bgColor
        Console.ForegroundColor = fgColor
        For i As Integer = 0 To 21
            For j As Integer = 0 To i
                Console.WriteLine()
            Next
            Console.Write(title)
            Threading.Thread.Sleep(Delay)

            Console.Clear()
        Next

        For i As Integer = 0 To 20
            For j As Integer = 0 To 19 - i
                Console.WriteLine()
            Next
            Console.Write(title)
            Threading.Thread.Sleep(Delay)
            If Not i = 20 Then
                Console.Clear()
            End If
        Next
        Return True
    End Function
    Sub startButton(ByVal selected As Boolean, Optional ByVal bgColor As ConsoleColor = ConsoleColor.Black, Optional ByVal fgColor As ConsoleColor = ConsoleColor.White)
        Dim button As String = "                                                                             
                                                    __                    __ 
                                            _____  / /_  ____ _   _____  / /_
                                           / ___/ / __/ / __ `/  / ___/ / __/
                                          (__  ) / /_  / /_/ /  / /    / /_  
                                         /____/  \__/  \__,_/  /_/     \__/ 
                                                                             "
        If selected Then
            button = "                                        -------------------------------------
                                       |            __                    __ |
                                       |    _____  / /_  ____ _   _____  / /_|
                                       |   / ___/ / __/ / __ `/  / ___/ / __/|
                                       |  (__  ) / /_  / /_/ /  / /    / /_  |
                                       | /____/  \__/  \__,_/  /_/     \__/  |
                                       |                                     |
                                        -------------------------------------"
        End If
        Console.BackgroundColor = bgColor
        Console.ForegroundColor = fgColor
        Console.WriteLine(button)

    End Sub

    Sub singleplayer(ByVal selected As Boolean, Optional ByVal bgColor As ConsoleColor = ConsoleColor.Black, Optional ByVal fgColor As ConsoleColor = ConsoleColor.White)
        Dim button As String = "                                                             
                                   _____ _             __           __                     
                                  / ___/(_)___  ____ _/ /__  ____  / /___ ___  _____  _____
                                  \__ \/ / __ \/ __ `/ / _ \/ __ \/ / __ `/ / / / _ \/ ___/
                                 ___/ / / / / / /_/ / /  __/ /_/ / / /_/ / /_/ /  __/ /    
                                /____/_/_/ /_/\__, /_/\___/ .___/_/\__,_/\__, /\___/_/   
                                             /____/      /_/            /____/             
                                                                "
        If selected Then
            button = "                                -----------------------------------------------------------
                               |   _____ _             __           __                     |
                               |  / ___/(_)___  ____ _/ /__  ____  / /___ ___  _____  _____|
                               |  \__ \/ / __ \/ __ `/ / _ \/ __ \/ / __ `/ / / / _ \/ ___/|
                               | ___/ / / / / / /_/ / /  __/ /_/ / / /_/ / /_/ /  __/ /    |
                               |/____/_/_/ /_/\__, /_/\___/ .___/_/\__,_/\__, /\___/_/     |
                               |             /____/      /_/            /____/             |
                                -----------------------------------------------------------"
        End If
        Console.BackgroundColor = bgColor
        Console.ForegroundColor = fgColor
        Console.WriteLine(button)

    End Sub
    Sub multiplayer(ByVal selected As Boolean, Optional ByVal bgColor As ConsoleColor = ConsoleColor.Black, Optional ByVal fgColor As ConsoleColor = ConsoleColor.White)
        Dim button As String = "
                                     __  ___      ____  _       __                     
                                    /  |/  /_  __/ / /_(_)___  / /___ ___  _____  _____
                                   / /|_/ / / / / / __/ / __ \/ / __ `/ / / / _ \/ ___/
                                  / /  / / /_/ / / /_/ / /_/ / / /_/ / /_/ /  __/ /    
                                 /_/  /_/\__,_/_/\__/_/ .___/_/\__,_/\__, /\___/_/
                                                   /_/            /____/              
"
        If selected Then
            button = "                                 ------------------------------------------------------
                                |    __  ___      ____  _       __                     |
                                |   /  |/  /_  __/ / /_(_)___  / /___ ___  _____  _____|
                                |  / /|_/ / / / / / __/ / __ \/ / __ `/ / / / _ \/ ___/|
                                | / /  / / /_/ / / /_/ / /_/ / / /_/ / /_/ /  __/ /    |
                                |/_/  /_/\__,_/_/\__/_/ .___/_/\__,_/\__, /\___/_/     |
                                |                  /_/            /____/               |
                                 ------------------------------------------------------"
        End If
        Console.BackgroundColor = bgColor
        Console.ForegroundColor = fgColor
        Console.WriteLine(button)

    End Sub

    Sub fightButton(ByVal selected As Boolean, Optional ByVal bgColor As ConsoleColor = ConsoleColor.Black, Optional ByVal fgColor As ConsoleColor = ConsoleColor.White, Optional ByVal disabled As Boolean = False)
        Dim button As String = "
                                              __________________  ________
                                             / ____/  _/ ____/ / / /_  __/
                                            / /_   / // / __/ /_/ / / /   
                                           / __/ _/ // /_/ / __  / / /    
                                          /_/   /___/\____/_/ /_/ /_/     
                            "
        If selected Then
            button = "                                          --------------------------------
                                         |    __________________  ________|
                                         |   / ____/  _/ ____/ / / /_  __/|
                                         |  / /_   / // / __/ /_/ / / /   |
                                         | / __/ _/ // /_/ / __  / / /    |
                                         |/_/   /___/\____/_/ /_/ /_/     |
                                         |                                |
                                          --------------------------------"
        End If
        '43 for not select
        '42 for select

        Console.BackgroundColor = bgColor
        Console.ForegroundColor = fgColor
        If disabled Then
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If
        Console.WriteLine(button)
    End Sub

    Sub shopButton(ByVal selected As Boolean, Optional ByVal bgColor As ConsoleColor = ConsoleColor.Black, Optional ByVal fgColor As ConsoleColor = ConsoleColor.White)
        Dim button As String = "                                                _____ __  ______  ____ 
                                               / ___// / / / __ \/ __ \
                                               \__ \/ /_/ / / / / /_/ /
                                              ___/ / __  / /_/ / ____/ 
                                             /____/_/ /_/\____/_/
                                         "
        If selected Then
            button = "                                            --------------------------- 
                                           |    _____ __  ______  ____ |
                                           |   / ___// / / / __ \/ __ \|
                                           |   \__ \/ /_/ / / / / /_/ /|
                                           |  ___/ / __  / /_/ / ____/ |
                                           | /____/_/ /_/\____/_/      |
                                           |                           |
                                            ---------------------------"
        End If
        '43 for not select
        '42 for select

        Console.BackgroundColor = bgColor
        Console.ForegroundColor = fgColor
        Console.WriteLine(button)
    End Sub


    Sub endButton(ByVal selected As Boolean, Optional ByVal bgColor As ConsoleColor = ConsoleColor.Black, Optional ByVal fgColor As ConsoleColor = ConsoleColor.White)
        Dim button As String = "                                                                     __
                                                  ___    ____   ____/ /
                                                 / _ \  / __ \ / __  / 
                                                /  __/ / / / // /_/ /  
                                                \___/ /_/ /_/ \__,_/    "
        If selected Then
            button = "                                               -------------------------
                                              |                      __ |
                                              |   ___    ____   ____/ / |
                                              |  / _ \  / __ \ / __  /  |
                                              | /  __/ / / / // /_/ /   |
                                              | \___/ /_/ /_/ \__,_/    |
                                              |                         |
                                               ------------------------- "
        End If
        'FONT = SLANT
        Console.BackgroundColor = bgColor
        Console.ForegroundColor = fgColor
        Console.WriteLine(button)
    End Sub
    Sub startMenu(ByVal title As String, Optional ByVal animatin As Boolean = False)
        Dim input As String
        Dim inputY As Integer = 1
        Const bg As ConsoleColor = ConsoleColor.Black
        Dim fg As ConsoleColor = ConsoleColor.DarkCyan
        Dim fgA As ConsoleColor = ConsoleColor.DarkCyan
        Dim buttonGap As Integer = 3
        Dim animationFinshed As Boolean = False

        If animatin Then
            animationFinshed = startAnimation(title, fgColor:=ConsoleColor.DarkRed)
            While Not animationFinshed
                'pass
            End While
        Else
            Console.ForegroundColor = ConsoleColor.DarkRed
            Console.WriteLine(title)
            Console.ForegroundColor = ConsoleColor.White
        End If

        startButton(True, bg, fgA)
        For lineNum As Integer = 0 To buttonGap
            Console.WriteLine()
        Next
        endButton(False, bg, fg)
        Console.SetCursorPosition(0, 0)
        While True
            input = Console.ReadKey(True).Key
            If input = 38 Or input = 87 Then
                If inputY = 0 Then
                    inputY = 1
                End If
            ElseIf input = 40 Or input = 83 Then
                If inputY = 1 Then

                    inputY = 0
                End If
            ElseIf input = 13 Or input = 32 Then

                If inputY = 0 Then
                    End
                ElseIf inputY = 1 Then
                    Exit While
                End If

            End If

            Console.Clear()
            Console.ForegroundColor = ConsoleColor.DarkRed
            Console.WriteLine(title)

            If inputY = 1 Then
                startButton(True, bg, fgA)
                For lineNum As Integer = 0 To buttonGap
                    Console.WriteLine()
                Next
                endButton(False, bg, fg)
            ElseIf inputY = 0 Then
                startButton(False, bg, fg)
                For lineNum As Integer = 0 To buttonGap
                    Console.WriteLine()
                Next
                endButton(True, bg, fgA)
            End If
            Console.SetCursorPosition(0, 0)
        End While




    End Sub



    Function numberOfPlayers(ByVal title As String) As Boolean
        Dim input As String
        Dim inputY As Integer = 1
        Const bg As ConsoleColor = ConsoleColor.Black
        Dim fg As ConsoleColor = ConsoleColor.Cyan
        Dim fgA As ConsoleColor = ConsoleColor.Cyan

        Dim fg2 As ConsoleColor = ConsoleColor.Magenta
        Dim fgA2 As ConsoleColor = ConsoleColor.Magenta
        Dim buttonGap As Integer = 3

        Console.ForegroundColor = ConsoleColor.DarkRed
        Console.WriteLine(title)
        Console.ForegroundColor = ConsoleColor.White
        singleplayer(True, bg, fgA)
        For lineNum As Integer = 0 To buttonGap
            Console.WriteLine()
        Next
        multiplayer(False, bg, fg2)
        Console.SetCursorPosition(0, 0)
        While True
            input = Console.ReadKey(True).Key
            If input = 38 Or input = 87 Then
                If inputY = 0 Then
                    inputY = 1
                End If
            ElseIf input = 40 Or input = 83 Then
                If inputY = 1 Then

                    inputY = 0
                End If
            ElseIf input = 13 Or input = 32 Then

                If inputY = 0 Then
                    Return False
                ElseIf inputY = 1 Then
                    Return True
                End If

            End If

            Console.Clear()
            Console.ForegroundColor = ConsoleColor.DarkRed
            Console.WriteLine(title)

            If inputY = 1 Then
                singleplayer(True, bg, fgA)
                For lineNum As Integer = 0 To buttonGap
                    Console.WriteLine()
                Next
                multiplayer(False, bg, fg2)
            ElseIf inputY = 0 Then
                singleplayer(False, bg, fg)
                For lineNum As Integer = 0 To buttonGap
                    Console.WriteLine()
                Next
                multiplayer(True, bg, fgA2)
            End If
            Console.SetCursorPosition(0, 0)
        End While

    End Function
    Sub Debug(ByVal Str As Object)
        Console.Clear()
        Console.WriteLine(Str)
        waitForEnter()
    End Sub

    Sub shopTitle(ByVal person As String, ByVal fg As ConsoleColor, Optional ByVal isSelect As Boolean = False)
        Dim knightText As String() = {"██████╗  █████╗ ███████╗██╗ ██████╗    ██╗  ██╗███╗   ██╗██╗ ██████╗ ██╗  ██╗████████╗", "██╔══██╗██╔══██╗██╔════╝██║██╔════╝    ██║ ██╔╝████╗  ██║██║██╔════╝ ██║  ██║╚══██╔══╝", "██████╔╝███████║███████╗██║██║         █████╔╝ ██╔██╗ ██║██║██║  ███╗███████║   ██║   ", "██╔══██╗██╔══██║╚════██║██║██║         ██╔═██╗ ██║╚██╗██║██║██║   ██║██╔══██║   ██║   ", "██████╔╝██║  ██║███████║██║╚██████╗    ██║  ██╗██║ ╚████║██║╚██████╔╝██║  ██║   ██║   ", "╚═════╝ ╚═╝  ╚═╝╚══════╝╚═╝ ╚═════╝    ╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝ ╚═════╝ ╚═╝  ╚═╝   ╚═╝   "}
        Dim soldierText As String() = {"████████╗██████╗  █████╗ ██╗███╗   ██╗███████╗██████╗     ███████╗ ██████╗ ██╗     ██████╗ ██╗███████╗██████╗ ", "╚══██╔══╝██╔══██╗██╔══██╗██║████╗  ██║██╔════╝██╔══██╗    ██╔════╝██╔═══██╗██║     ██╔══██╗██║██╔════╝██╔══██╗", "   ██║   ██████╔╝███████║██║██╔██╗ ██║█████╗  ██║  ██║    ███████╗██║   ██║██║     ██║  ██║██║█████╗  ██████╔╝", "   ██║   ██╔══██╗██╔══██║██║██║╚██╗██║██╔══╝  ██║  ██║    ╚════██║██║   ██║██║     ██║  ██║██║██╔══╝  ██╔══██╗", "   ██║   ██║  ██║██║  ██║██║██║ ╚████║███████╗██████╔╝    ███████║╚██████╔╝███████╗██████╔╝██║███████╗██║  ██║", "   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝╚═╝  ╚═══╝╚══════╝╚═════╝     ╚══════╝ ╚═════╝ ╚══════╝╚═════╝ ╚═╝╚══════╝╚═╝  ╚═╝"}
        Dim warriorText As String() = {"██╗    ██╗ █████╗ ██████╗ ██████╗ ██╗ ██████╗ ██████╗ ", "██║    ██║██╔══██╗██╔══██╗██╔══██╗██║██╔═══██╗██╔══██╗", "██║ █╗ ██║███████║██████╔╝██████╔╝██║██║   ██║██████╔╝", "██║███╗██║██╔══██║██╔══██╗██╔══██╗██║██║   ██║██╔══██╗", "╚███╔███╔╝██║  ██║██║  ██║██║  ██║██║╚██████╔╝██║  ██║", " ╚══╝╚══╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝ ╚═════╝ ╚═╝  ╚═╝"}

        Dim cX As Integer = 19 'DO NOT CHAGE THERE ARE DEPENDENCE
        Dim cY As Integer = 0
        Dim fgS As ConsoleColor = ConsoleColor.DarkYellow


        Dim line As String

        person = person.ToLower

        Select Case person
            Case 0

                Console.SetCursorPosition(cX, cY)
                For lineIndex = 0 To knightText.Length - 1
                    Console.ForegroundColor = fg
                    If isSelect Then
                        Console.ForegroundColor = fgS
                    End If
                    line = knightText(lineIndex)
                    Console.WriteLine(line)
                    Console.SetCursorPosition(cX, lineIndex + 1 + cY)
                Next
            Case 1
                Console.SetCursorPosition(cX - 14, cY)
                For lineIndex = 0 To soldierText.Length - 1
                    Console.ForegroundColor = fg
                    If isSelect Then
                        Console.ForegroundColor = fgS
                    End If
                    line = soldierText(lineIndex)
                    Console.WriteLine(line)
                    Console.SetCursorPosition(cX - 14, lineIndex + 1 + cY)
                Next
            Case 2
                Console.SetCursorPosition(cX + 15, cY)
                For lineIndex = 0 To warriorText.Length - 1
                    Console.ForegroundColor = fg
                    If isSelect Then
                        Console.ForegroundColor = fgS
                    End If
                    line = warriorText(lineIndex)
                    Console.WriteLine(line)
                    Console.SetCursorPosition(cX + 15, lineIndex + 1 + cY)
                Next
        End Select
        Console.SetCursorPosition(0, 0)

    End Sub

    Sub showPerson(ByVal person As Integer, ByVal fg As ConsoleColor, ByVal player As Object, Optional isSelect As Boolean = False, Optional cardIndex As Integer = 0, Optional ShowName As Boolean = False, Optional ByVal showOpp As Boolean = False)
        Dim fgPerson As ConsoleColor = fg
        Dim line As String = ""
        Dim cX As Integer = 0
        Dim cY As Integer = 8
        Dim xOffset As Integer = 27
        Dim yOffset As Integer = 0
        Dim statsXOffset As Integer = 3
        Dim coinsColor As ConsoleColor = ConsoleColor.Blue


        Dim kSprite As String() = {"         .--.", "        /.--.\", "        |====|", "        |`::`|", "    .-;`\..../`;-.", "   /  |...::...|  \", "  |   /'''::'''\   |", "  ;--'\   ::   /\--;", "  <__>,>._::_.<,<__>", "  |  |/   ^^   \|  |", "  \::/|        |\::/", "  |||\|        |/|||", "  ''' |___/\___| '''", "       \_ || _/", "       <_ >< _>", "       |  ||  |", "       |  ||  |", "      _\.:||:./_", "     /____/\____\"}
        Dim sSprite As String() = {" /\          .--.", " ||         /.--.\", " ||         |====|", " ||         |`::`|", "_||_    .-;`\..../`;-.", " /\\   /  |...::...|  \", " |:'\ |   /'''::'''\   |", "  \ /\;-,/\   ::   /\--;", "   \ <` >  >._::_.<,<__>", "    `""`   /   ^^   \|  |", "          |        |\::/", "          |        |/|||", "          |___/\___| '''", "           \_ || _/", "           <_ >< _>", "           |  ||  |", "           |  ||  |", "          _\.:||:./_", "         /____/\____\"}
        Dim wSprite As String() = {"  ,   |          .--.", " / \, | ,       /.--.\", "|    =|= >      |====|", " \ /` | `       |`::`|", "  `   |     .-;`\..../`;-.", "     /\\/  /  |...::...|  \", "     |:'\ |   /'''::'''\   |", "      \ /\;-,/\   ::   /\--;", "      |\ <` >  >._::_.<,<__>", "      | `""`   /   ^^   \|  |", "      |       |        |\::/", "      |       |        |/|||", "      |       |___/\___| '''", "      |        \_ || _/", "      |        <_ >< _>", "      |        |  ||  |", "      |        |  ||  |", "      |       _\.:||:./_", "      |      /____/\____\"}
        If ShowName Then
            kSprite = {"         .--.", "        /.--.\", "        |====|", "        |`::`|", "    .-;`\..../`;-.", "   /  |...::...|  \", "  |   /'''::'''\   |", "  ;--'\   ::   /\--;", "  <__>,>._::_.<,<__>", "  |  |/   ^^   \|  |", "  \::/|        |\::/", "  |||\|        |/|||", "  ''' |___/\___| '''", "       \_ || _/", "       <_ >< _>", "       |  ||  |", "       |  ||  |", "      _\.:||:./_", "     /____/\____\", "     Basic Knight"}
            sSprite = {" /\          .--.", " ||         /.--.\", " ||         |====|", " ||         |`::`|", "_||_    .-;`\..../`;-.", " /\\   /  |...::...|  \", " |:'\ |   /'''::'''\   |", "  \ /\;-,/\   ::   /\--;", "   \ <` >  >._::_.<,<__>", "    `""`   /   ^^   \|  |", "          |        |\::/", "          |        |/|||", "          |___/\___| '''", "           \_ || _/", "           <_ >< _>", "           |  ||  |", "           |  ||  |", "          _\.:||:./_", "         /____/\____\", "       Trained  Soldier"}
            wSprite = {"  ,   |          .--.", " / \, | ,       /.--.\", "|    =|= >      |====|", " \ /` | `       |`::`|", "  `   |     .-;`\..../`;-.", "     /\\/  /  |...::...|  \", "     |:'\ |   /'''::'''\   |", "      \ /\;-,/\   ::   /\--;", "      |\ <` >  >._::_.<,<__>", "      | `""`   /   ^^   \|  |", "      |       |        |\::/", "      |       |        |/|||", "      |       |___/\___| '''", "      |        \_ || _/", "      |        <_ >< _>", "      |        |  ||  |", "      |        |  ||  |", "      |       _\.:||:./_", "      |      /____/\____\", "          Legendary  Warrior"}
        End If
        If ShowName Then
            yOffset = 0
        End If

        Select Case person
            Case 0
                Console.SetCursorPosition(cX + xOffset + 8, cY + yOffset)
                For lineIndex = 0 To kSprite.Length - 1
                    Console.ForegroundColor = fgPerson
                    line = kSprite(lineIndex)
                    Console.WriteLine(line)
                    Console.SetCursorPosition(cX + xOffset + 8, lineIndex + 1 + cY + yOffset)
                Next

                If Not isSelect Then
                    showStatsCard(New BasicKnight(), New BasicKnight(), 31 + xOffset + statsXOffset, -14, showOpp)
                    showPrice(New BasicKnight(), coinsColor, 68, 28)
                Else
                    showStatsCard(player.cards(cardIndex), New BasicKnight(), 31 + xOffset + statsXOffset, -14, showOpp)
                End If
            Case 1

                Console.SetCursorPosition(cX + xOffset + 4, cY + yOffset)
                For lineIndex = 0 To sSprite.Length - 1
                    Console.ForegroundColor = fgPerson
                    line = sSprite(lineIndex)
                    Console.WriteLine(line)
                    Console.SetCursorPosition(cX + xOffset + 4, lineIndex + 1 + cY + yOffset)
                Next


                If Not isSelect Then
                    showStatsCard(New TrainedSoldier(), New TrainedSoldier(), 31 + xOffset + statsXOffset, -14, showOpp)
                    showPrice(New TrainedSoldier(), coinsColor, 68, 28)
                Else
                    showStatsCard(player.cards(cardIndex), New BasicKnight(), 31 + xOffset + statsXOffset, -14, showOpp)
                End If
            Case 2S
                Console.SetCursorPosition(cX + xOffset, cY + yOffset)
                For lineIndex = 0 To wSprite.Length - 1
                    Console.ForegroundColor = fgPerson
                    line = wSprite(lineIndex)
                    Console.WriteLine(line)
                    Console.SetCursorPosition(cX + xOffset, lineIndex + 1 + cY + yOffset)
                Next
                If Not isSelect Then
                    showStatsCard(New LegendaryWarrior(), New LegendaryWarrior(), 31 + xOffset + statsXOffset, -14, showOpp)
                    showPrice(New LegendaryWarrior(), coinsColor, 68, 28)
                Else
                    showStatsCard(player.cards(cardIndex), New BasicKnight(), 31 + xOffset + statsXOffset, -14, showOpp)
                End If
        End Select
        If isSelect And Not ShowName Then
            Console.SetCursorPosition(65, 28)
            Console.ForegroundColor = ConsoleColor.White
            Console.Write("Card Number: ")
            Console.ForegroundColor = ConsoleColor.DarkCyan
            Console.WriteLine(cardIndex + 1)
        End If

        Console.SetCursorPosition(0, 0)



    End Sub

    Sub arrowRight(ByVal fg As ConsoleColor, ByVal Selected As Boolean, ByVal cX As Integer, ByVal cY As Integer, Optional ByVal disabled As Boolean = False)
        Dim arrowRight As String() = {"", "               .", "   .. .........;;.", "    ..:::::::::;;;;.", "  . . :::::::::;;:'", "               :'"}
        If Selected Then
            arrowRight = {" --------------------", "|              .     |", "|  .. .........;;.   |", "|   ..:::::::::;;;;. |", "| . . :::::::::;;:'  |", "|              :'    |", " --------------------"}
        End If
        Dim line As String

        If disabled Then
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If



        Console.SetCursorPosition(cX, cY)
        For lineIndex = 0 To arrowRight.Length - 1
            Console.ForegroundColor = fg
            If disabled Then
                Console.ForegroundColor = ConsoleColor.DarkGray
            End If
            line = arrowRight(lineIndex)
            Console.WriteLine(line)
            Console.SetCursorPosition(cX, lineIndex + 1 + cY)
        Next
    End Sub

    Sub arrowLeft(ByVal fg As ConsoleColor, ByVal Selected As Boolean, ByVal cX As Integer, ByVal cY As Integer, Optional ByVal disabled As Boolean = False)
        Dim arrowLeft As String() = {"", "       .", "     .;;......... ..", "   .;;;;:::::::::..", "    ':;;::::::::: . .", "      ':"}
        If Selected Then
            arrowLeft = {" --------------------", "|      .             |", "|    .;;......... .. |", "|  .;;;;:::::::::..  |", "|   ':;;::::::::: . .|", "|     ':             |", " --------------------"}
        End If
        Dim line As String

        If disabled Then
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If
        Console.SetCursorPosition(cX, cY)
        For lineIndex = 0 To arrowLeft.Length - 1
            Console.ForegroundColor = fg
            If disabled Then
                Console.ForegroundColor = ConsoleColor.DarkGray
            End If
            line = arrowLeft(lineIndex)
            Console.WriteLine(line)
            Console.SetCursorPosition(cX, lineIndex + 1 + cY)
        Next
    End Sub

    Sub UseButton(ByVal fg As ConsoleColor, ByVal Selected As Boolean, ByVal cX As Integer, ByVal cY As Integer, Optional ByVal disabled As Boolean = False)
        Dim useButton As String() = {" -----------------------", "| _   _   ____    _____ |", "|| | | | / ___|  | ____||", "|| | | | \___ \  |  _|  |", "|| |_| |  ___) | | |___ |", "| \___/  |____/  |_____||", "|                       |", " -----------------------"}
        If Not Selected Then
            useButton = {"", "  _   _   ____    _____ ", " | | | | / ___|  | ____|", " | | | | \___ \  |  _|  ", " | |_| |  ___) | | |___ ", "  \___/  |____/  |_____|"}
        End If
        Dim line As String

        If disabled Then
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If

        Console.SetCursorPosition(cX, cY)
        For lineIndex = 0 To useButton.Length - 1
            Console.ForegroundColor = fg
            If disabled Then
                Console.ForegroundColor = ConsoleColor.DarkGray
            End If
            line = useButton(lineIndex)
            Console.WriteLine(line)
            Console.SetCursorPosition(cX, lineIndex + 1 + cY)
        Next

    End Sub

    Sub BuyButton(ByVal fg As ConsoleColor, ByVal Selected As Boolean, ByVal cX As Integer, ByVal cY As Integer, Optional ByVal disabled As Boolean = False)
        Dim buyButton As String() = {" -------------------------", "|  ____                   |", "| | __ )   _   _   _   _  |", "| |  _ \  | | | | | | | | |", "| | |_) | | |_| | | |_| | |", "| |____/   \__,_|  \__, | |", "|                  |___/  |", " -------------------------"}
        If Not Selected Then
            buyButton = {"", "   ____                  ", "  | __ )   _   _   _   _ ", "  |  _ \  | | | | | | | |", "  | |_) | | |_| | | |_| |", "  |____/   \__,_|  \__, |", "                   |___/ "}
        End If
        Dim line As String

        If disabled Then
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If

        Console.SetCursorPosition(cX, cY)
        For lineIndex = 0 To buyButton.Length - 1
            Console.ForegroundColor = fg
            If disabled Then
                Console.ForegroundColor = ConsoleColor.DarkGray
            End If
            line = buyButton(lineIndex)
            Console.WriteLine(line)
            Console.SetCursorPosition(cX, lineIndex + 1 + cY)
        Next

    End Sub

    Sub ExitShopButton(ByVal fg As ConsoleColor, ByVal Selected As Boolean, ByVal cX As Integer, ByVal cY As Integer, Optional ByVal disabled As Boolean = False, Optional ByVal isSelect As Boolean = False)
        Dim exitShopButton As String() = {" ------------------------", "| _____          _   _   |", "|| ____| __  __ (_) | |_ |", "||  _|   \ \/ / | | | __||", "|| |___   >  <  | | | |_ |", "||_____| /_/\_\ |_|  \__||", "|                        |", " ------------------------"}
        If Not Selected Then
            exitShopButton = {"", "  _____          _   _   ", " | ____| __  __ (_) | |_ ", " |  _|   \ \/ / | | | __|", " | |___   >  <  | | | |_ ", " |_____| /_/\_\ |_|  \__|"}
        End If

        If isSelect Then
            exitShopButton = {" -------------------------", "| _____ _       _     _   |", "||  ___(_) __ _| |__ | |_ |", "|| |_  | |/ _` | '_ \| __||", "||  _| | | (_| | | | | |_ |", "||_|   |_|\__, |_| |_|\__||", "|         |___/           |", " -------------------------"}
            If Not Selected Then
                exitShopButton = {"", "  _____ _       _     _   ", " |  ___(_) __ _| |__ | |_ ", " | |_  | |/ _` | '_ \| __|", " |  _| | | (_| | | | | |_ ", " |_|   |_|\__, |_| |_|\__|", "          |___/           "}
            End If
        End If

        Dim line As String

        If disabled Then
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If

        Console.SetCursorPosition(cX, cY)
        For lineIndex = 0 To exitShopButton.Length - 1
            Console.ForegroundColor = fg
            If disabled Then
                Console.ForegroundColor = ConsoleColor.DarkGray
            End If
            line = exitShopButton(lineIndex)
            Console.WriteLine(line)
            Console.SetCursorPosition(cX, lineIndex + 1 + cY)
        Next
    End Sub
    Sub showCoins(ByVal player As Player, ByVal fg As ConsoleColor, ByVal cX As Integer, ByVal cY As Integer)
        Console.ForegroundColor = fg
        Console.SetCursorPosition(cX, cY)
        Console.Write("Coins: ")
        Console.ForegroundColor = ConsoleColor.Green
        Console.WriteLine(player.coins)
        Console.SetCursorPosition(0, 0)
    End Sub
    Sub showPrice(ByVal person As Object, ByVal fg As ConsoleColor, ByVal cX As Integer, ByVal cY As Integer)
        Console.ForegroundColor = fg
        Console.SetCursorPosition(cX, cY)
        Console.Write("Price: ")
        Console.ForegroundColor = ConsoleColor.Green
        Console.WriteLine(person.price)
        Console.SetCursorPosition(0, 0)
    End Sub

    Function shopButtons(ByVal xOffset As Integer, ByVal person As Integer, ByVal buttonNumber As Integer, ByVal player As Player, ByVal isComputer As Boolean, ByVal turn As Integer) As Integer
        Dim xInput, yInput, input As Integer
        Dim coinsColor As ConsoleColor = ConsoleColor.Magenta
        Dim coinX As Integer = 68
        Dim coinY As Integer = 26
        Dim currentCard As Object = New BasicKnight()
        Select Case person
            Case 0
                currentCard = New BasicKnight()
            Case 1
                currentCard = New TrainedSoldier()
            Case 2
                currentCard = New LegendaryWarrior()
        End Select


        Select Case buttonNumber
            Case 0
                xInput = 0
                yInput = 0
            Case 1
                xInput = 1
                yInput = 1
            Case 2
                xInput = 1
                yInput = 0
            Case 3
                xInput = 0
                yInput = 1
        End Select

        Console.Clear()
        showCoins(player, coinsColor, coinX, coinY)
        If Not isComputer And turn = 0 Then
            Console.ForegroundColor = ConsoleColor.Cyan
            Console.Write("





---------------------------------------------------- PLAYER ONE --------------------------------------------------------")
        ElseIf Not isComputer And turn = 1 Then
            Console.ForegroundColor = ConsoleColor.Magenta
            Console.Write("





---------------------------------------------------- PLAYER TWO --------------------------------------------------------")
        End If
        showPerson(person, ConsoleColor.White, player)
        shopTitle(person, ConsoleColor.DarkYellow)
        showVisualStats(currentCard, currentCard, True)



        If yInput = 0 Then
            Select Case xInput
                Case 0
                    If person - 1 < 0 Then
                        arrowLeft(ConsoleColor.DarkCyan, True, 0, 13, True)
                    Else
                        arrowLeft(ConsoleColor.DarkCyan, True, 0, 13)
                    End If
                    If player.coins >= currentCard.Price Then
                        BuyButton(ConsoleColor.Cyan, False, 65 + xOffset, 21)
                    Else
                        BuyButton(ConsoleColor.Cyan, False, 65 + xOffset, 21, True)
                    End If
                    ExitShopButton(ConsoleColor.Red, False, -23 + xOffset, 21)
                    If person + 1 > 2 Then
                        arrowRight(ConsoleColor.DarkCyan, False, 97, 13, True)
                    Else
                        arrowRight(ConsoleColor.DarkCyan, False, 97, 13)
                    End If
                Case 1
                    If person - 1 < 0 Then
                        arrowLeft(ConsoleColor.DarkCyan, False, 0, 13, True)
                    Else
                        arrowLeft(ConsoleColor.DarkCyan, False, 0, 13)
                    End If
                    If player.coins >= currentCard.Price Then
                        BuyButton(ConsoleColor.Cyan, False, 65 + xOffset, 21)
                    Else
                        BuyButton(ConsoleColor.Cyan, False, 65 + xOffset, 21, True)
                    End If
                    ExitShopButton(ConsoleColor.Red, False, -23 + xOffset, 21)
                    If person + 1 > 2 Then
                        arrowRight(ConsoleColor.DarkCyan, True, 97, 13, True)
                    Else
                        arrowRight(ConsoleColor.DarkCyan, True, 97, 13)
                    End If
            End Select
        Else
            If person - 1 < 0 Then
                arrowLeft(ConsoleColor.DarkCyan, False, 0, 13, True)
            Else
                arrowLeft(ConsoleColor.DarkCyan, False, 0, 13)
            End If
            If person + 1 > 2 Then
                arrowRight(ConsoleColor.DarkCyan, False, 97, 13, True)
            Else
                arrowRight(ConsoleColor.DarkCyan, False, 97, 13)
            End If
            Select Case xInput
                Case 0
                    If player.coins >= currentCard.Price Then
                        BuyButton(ConsoleColor.Cyan, False, 65 + xOffset, 21)
                    Else
                        BuyButton(ConsoleColor.Cyan, False, 65 + xOffset, 21, True)
                    End If
                    ExitShopButton(ConsoleColor.Red, True, -23 + xOffset, 21)
                Case 1
                    If player.coins >= currentCard.Price Then
                        BuyButton(ConsoleColor.Cyan, True, 65 + xOffset, 21)
                    Else
                        BuyButton(ConsoleColor.Cyan, True, 65 + xOffset, 21, True)
                    End If
                    ExitShopButton(ConsoleColor.Red, False, -23 + xOffset, 21)
            End Select
        End If
        Console.SetCursorPosition(0, 0)
        While True
            Select Case person
                Case 0
                    currentCard = New BasicKnight()
                Case 1
                    currentCard = New TrainedSoldier()
                Case 2
                    currentCard = New LegendaryWarrior()
            End Select

            input = Console.ReadKey(True).Key
            If (input = 37 Or input = 65) Then
                If (xInput = 1) Then
                    xInput = 0
                End If
            ElseIf (input = 39 Or input = 68) Then
                If xInput = 0 Then
                    xInput = 1
                End If
            ElseIf (input = 38 Or input = 87) Then
                If (yInput = 1) Then
                    yInput = 0
                End If
            ElseIf (input = 40 Or input = 83) Then
                If (yInput = 0) Then
                    yInput = 1
                End If
            ElseIf (input = 13 Or input = 32) Then
                If yInput = 0 Then
                    Select Case xInput
                        Case 0
                            Return 0
                        Case 1
                            Return 2
                    End Select
                Else
                    Select Case xInput
                        Case 0
                            Return 3
                        Case 1 ' Buy
                            If player.coins >= currentCard.Price Then
                                Return 1
                            End If

                    End Select

                End If
            End If
            Console.Clear()
            showCoins(player, coinsColor, coinX, coinY)
            If Not isComputer And turn = 0 Then
                Console.ForegroundColor = ConsoleColor.Cyan
                Console.Write("





---------------------------------------------------- PLAYER ONE --------------------------------------------------------")
            ElseIf Not isComputer And turn = 1 Then
                Console.ForegroundColor = ConsoleColor.Magenta
                Console.Write("





---------------------------------------------------- PLAYER TWO --------------------------------------------------------")
            End If
            showPerson(person, ConsoleColor.White, player)
            shopTitle(person, ConsoleColor.DarkYellow)
            showVisualStats(currentCard, currentCard, True)

            If yInput = 0 Then
                Select Case xInput
                    Case 0
                        If person - 1 < 0 Then
                            arrowLeft(ConsoleColor.DarkCyan, True, 0, 13, True)
                        Else
                            arrowLeft(ConsoleColor.DarkCyan, True, 0, 13)
                        End If
                        If player.coins >= currentCard.Price Then
                            BuyButton(ConsoleColor.Cyan, False, 65 + xOffset, 21)
                        Else
                            BuyButton(ConsoleColor.Cyan, False, 65 + xOffset, 21, True)
                        End If
                        ExitShopButton(ConsoleColor.Red, False, -23 + xOffset, 21)
                        If person + 1 > 2 Then
                            arrowRight(ConsoleColor.DarkCyan, False, 97, 13, True)
                        Else
                            arrowRight(ConsoleColor.DarkCyan, False, 97, 13)
                        End If
                    Case 1
                        If person - 1 < 0 Then
                            arrowLeft(ConsoleColor.DarkCyan, False, 0, 13, True)
                        Else
                            arrowLeft(ConsoleColor.DarkCyan, False, 0, 13)
                        End If
                        If player.coins >= currentCard.Price Then
                            BuyButton(ConsoleColor.Cyan, False, 65 + xOffset, 21)
                        Else
                            BuyButton(ConsoleColor.Cyan, False, 65 + xOffset, 21, True)
                        End If
                        ExitShopButton(ConsoleColor.Red, False, -23 + xOffset, 21)
                        If person + 1 > 2 Then
                            arrowRight(ConsoleColor.DarkCyan, True, 97, 13, True)
                        Else
                            arrowRight(ConsoleColor.DarkCyan, True, 97, 13)
                        End If
                End Select
            Else
                If person - 1 < 0 Then
                    arrowLeft(ConsoleColor.DarkCyan, False, 0, 13, True)
                Else
                    arrowLeft(ConsoleColor.DarkCyan, False, 0, 13)
                End If
                If person + 1 > 2 Then
                    arrowRight(ConsoleColor.DarkCyan, False, 97, 13, True)
                Else
                    arrowRight(ConsoleColor.DarkCyan, False, 97, 13)
                End If
                Select Case xInput
                    Case 0
                        If player.coins >= currentCard.Price Then
                            BuyButton(ConsoleColor.Cyan, False, 65 + xOffset, 21)
                        Else
                            BuyButton(ConsoleColor.Cyan, False, 65 + xOffset, 21, True)
                        End If
                        ExitShopButton(ConsoleColor.Red, True, -23 + xOffset, 21)
                    Case 1
                        If player.coins >= currentCard.Price Then
                            BuyButton(ConsoleColor.Cyan, True, 65 + xOffset, 21)
                        Else
                            BuyButton(ConsoleColor.Cyan, True, 65 + xOffset, 21, True)
                        End If
                        ExitShopButton(ConsoleColor.Red, False, -23 + xOffset, 21)
                End Select
            End If


            Console.SetCursorPosition(0, 0)
        End While

        Return -1

    End Function


    Function selectButtons(ByVal xOffset As Integer, ByVal Card As Object, ByVal buttonNumber As Integer, ByVal player As Player, ByVal selectedIndex As Integer, ByVal isComputer As Boolean, ByVal turn As Integer) As Integer
        Dim xInput, yInput, input As Integer
        Dim coinsColor As ConsoleColor = ConsoleColor.Magenta
        Dim coinX As Integer = 68
        Dim coinY As Integer = 26
        Dim currentCard As Object = Card
        Dim currentCardIndex As Integer = player.cards.IndexOf(currentCard)
        Dim person As Integer

        Select Case Card.Name
            Case "Basic Knight"
                person = 0
            Case "Trained Soldier"
                person = 1
            Case "Legendary Warrior"
                person = 2
        End Select

        Select Case buttonNumber
            Case 0
                xInput = 0
                yInput = 0
            Case 1
                xInput = 1
                yInput = 1
            Case 2
                xInput = 1
                yInput = 0
            Case 3
                xInput = 0
                yInput = 1
        End Select

        Console.Clear()
        showPerson(person, ConsoleColor.White, player, True, currentCardIndex)
        Console.ForegroundColor = ConsoleColor.Cyan
        If Not isComputer And turn = 0 Then
            Console.Write("





---------------------------------------------------- PLAYER ONE --------------------------------------------------------")
        ElseIf Not isComputer And turn = 1 Then
            Console.ForegroundColor = ConsoleColor.Magenta
            Console.Write("





---------------------------------------------------- PLAYER TWO --------------------------------------------------------")
        End If
        shopTitle(person, ConsoleColor.DarkYellow, True)
        showVisualStats(currentCard, currentCard, True)



        If yInput = 0 Then
            Select Case xInput
                Case 0
                    If currentCardIndex - 1 < 0 Then
                        arrowLeft(ConsoleColor.DarkCyan, True, 0, 13, True)
                    Else
                        arrowLeft(ConsoleColor.DarkCyan, True, 0, 13)
                    End If

                    If selectedIndex = currentCardIndex Then
                        UseButton(ConsoleColor.Cyan, False, 65 + xOffset, 21, True)
                    Else
                        UseButton(ConsoleColor.Cyan, False, 65 + xOffset, 21)
                    End If


                    ExitShopButton(ConsoleColor.Red, False, -23 + xOffset, 21, isSelect:=True)
                    If currentCardIndex + 1 > player.cards.Count - 1 Then
                        arrowRight(ConsoleColor.DarkCyan, False, 97, 13, True)
                    Else
                        arrowRight(ConsoleColor.DarkCyan, False, 97, 13)
                    End If
                Case 1
                    If currentCardIndex - 1 < 0 Then
                        arrowLeft(ConsoleColor.DarkCyan, False, 0, 13, True)
                    Else
                        arrowLeft(ConsoleColor.DarkCyan, False, 0, 13)
                    End If

                    If selectedIndex = currentCardIndex Then
                        UseButton(ConsoleColor.Cyan, False, 65 + xOffset, 21, True)
                    Else
                        UseButton(ConsoleColor.Cyan, False, 65 + xOffset, 21)
                    End If

                    ExitShopButton(ConsoleColor.Red, False, -23 + xOffset, 21, isSelect:=True)
                    If currentCardIndex + 1 > player.cards.Count - 1 Then
                        arrowRight(ConsoleColor.DarkCyan, True, 97, 13, True)
                    Else
                        arrowRight(ConsoleColor.DarkCyan, True, 97, 13)
                    End If
            End Select
        Else
            If currentCardIndex - 1 < 0 Then
                arrowLeft(ConsoleColor.DarkCyan, False, 0, 13, True)
            Else
                arrowLeft(ConsoleColor.DarkCyan, False, 0, 13)
            End If
            If currentCardIndex + 1 > player.cards.Count - 1 Then
                arrowRight(ConsoleColor.DarkCyan, False, 97, 13, True)
            Else
                arrowRight(ConsoleColor.DarkCyan, False, 97, 13)
            End If
            Select Case xInput
                Case 0
                    If selectedIndex = currentCardIndex Then
                        UseButton(ConsoleColor.Cyan, False, 65 + xOffset, 21, True)
                    Else
                        UseButton(ConsoleColor.Cyan, False, 65 + xOffset, 21)
                    End If
                    ExitShopButton(ConsoleColor.Red, True, -23 + xOffset, 21, isSelect:=True)
                Case 1
                    If selectedIndex = currentCardIndex Then
                        UseButton(ConsoleColor.Cyan, True, 65 + xOffset, 21, True)
                    Else
                        UseButton(ConsoleColor.Cyan, True, 65 + xOffset, 21)
                    End If
                    ExitShopButton(ConsoleColor.Red, False, -23 + xOffset, 21, isSelect:=True)
            End Select
        End If

        Console.SetCursorPosition(0, 0)
        While True


            input = Console.ReadKey(True).Key
            If (input = 37 Or input = 65) Then
                If (xInput = 1) Then
                    xInput = 0
                End If
            ElseIf (input = 39 Or input = 68) Then
                If xInput = 0 Then
                    xInput = 1
                End If
            ElseIf (input = 38 Or input = 87) Then
                If (yInput = 1) Then
                    yInput = 0
                End If
            ElseIf (input = 40 Or input = 83) Then
                If (yInput = 0) Then
                    yInput = 1
                End If
            ElseIf (input = 13 Or input = 32) Then
                If yInput = 0 Then
                    Select Case xInput
                        Case 0
                            Return 0
                        Case 1
                            Return 2
                    End Select
                Else
                    Select Case xInput
                        Case 0
                            Return 3
                        Case 1 ' Buy
                            If Not selectedIndex = currentCardIndex Then
                                Return 1
                            End If

                    End Select

                End If
            End If
            Console.Clear()

            showPerson(person, ConsoleColor.White, player, True, currentCardIndex)
            shopTitle(person, ConsoleColor.DarkYellow, True)
            If Not isComputer And turn = 0 Then
                Console.ForegroundColor = ConsoleColor.Cyan
                Console.Write("





---------------------------------------------------- PLAYER ONE --------------------------------------------------------")
            ElseIf Not isComputer And turn = 1 Then
                Console.ForegroundColor = ConsoleColor.Magenta
                Console.Write("





---------------------------------------------------- PLAYER TWO --------------------------------------------------------")
            End If
            showVisualStats(currentCard, currentCard, True)

            If yInput = 0 Then
                Select Case xInput
                    Case 0
                        If currentCardIndex - 1 < 0 Then
                            arrowLeft(ConsoleColor.DarkCyan, True, 0, 13, True)
                        Else
                            arrowLeft(ConsoleColor.DarkCyan, True, 0, 13)
                        End If

                        If selectedIndex = currentCardIndex Then
                            UseButton(ConsoleColor.Cyan, False, 65 + xOffset, 21, True)
                        Else
                            UseButton(ConsoleColor.Cyan, False, 65 + xOffset, 21)
                        End If


                        ExitShopButton(ConsoleColor.Red, False, -23 + xOffset, 21, isSelect:=True)
                        If currentCardIndex + 1 > player.cards.Count - 1 Then
                            arrowRight(ConsoleColor.DarkCyan, False, 97, 13, True)
                        Else
                            arrowRight(ConsoleColor.DarkCyan, False, 97, 13)
                        End If
                    Case 1
                        If currentCardIndex - 1 < 0 Then
                            arrowLeft(ConsoleColor.DarkCyan, False, 0, 13, True)
                        Else
                            arrowLeft(ConsoleColor.DarkCyan, False, 0, 13)
                        End If

                        If selectedIndex = currentCardIndex Then
                            UseButton(ConsoleColor.Cyan, False, 65 + xOffset, 21, True)
                        Else
                            UseButton(ConsoleColor.Cyan, False, 65 + xOffset, 21)
                        End If

                        ExitShopButton(ConsoleColor.Red, False, -23 + xOffset, 21, isSelect:=True)
                        If currentCardIndex + 1 > player.cards.Count - 1 Then
                            arrowRight(ConsoleColor.DarkCyan, True, 97, 13, True)
                        Else
                            arrowRight(ConsoleColor.DarkCyan, True, 97, 13)
                        End If
                End Select
            Else
                If currentCardIndex - 1 < 0 Then
                    arrowLeft(ConsoleColor.DarkCyan, False, 0, 13, True)
                Else
                    arrowLeft(ConsoleColor.DarkCyan, False, 0, 13)
                End If
                If currentCardIndex + 1 > player.cards.Count - 1 Then
                    arrowRight(ConsoleColor.DarkCyan, False, 97, 13, True)
                Else
                    arrowRight(ConsoleColor.DarkCyan, False, 97, 13)
                End If
                Select Case xInput
                    Case 0
                        If selectedIndex = currentCardIndex Then
                            UseButton(ConsoleColor.Cyan, False, 65 + xOffset, 21, True)
                        Else
                            UseButton(ConsoleColor.Cyan, False, 65 + xOffset, 21)
                        End If
                        ExitShopButton(ConsoleColor.Red, True, -23 + xOffset, 21, isSelect:=True)
                    Case 1
                        If selectedIndex = currentCardIndex Then
                            UseButton(ConsoleColor.Cyan, True, 65 + xOffset, 21, True)
                        Else
                            UseButton(ConsoleColor.Cyan, True, 65 + xOffset, 21)
                        End If
                        ExitShopButton(ConsoleColor.Red, False, -23 + xOffset, 21, isSelect:=True)
                End Select
            End If


            Console.SetCursorPosition(0, 0)
        End While

        Return -1

    End Function

    Sub shop(ByVal player As Player, ByVal isComputer As Boolean, ByVal turn As Integer)
        'ANSI SHADOW
        Const title As String = "       █     █░ ▄▄▄       ██▀███       ██████  ██▓ ███▄ ▄███▓ █    ██  ██▓    ▄▄▄     ▄▄▄█████▓ ▒█████   ██▀███  
       ▓█░ █ ░█░▒████▄    ▓██ ▒ ██▒   ▒██    ▒ ▓██▒▓██▒▀█▀ ██▒ ██  ▓██▒▓██▒   ▒████▄   ▓  ██▒ ▓▒▒██▒  ██▒▓██ ▒ ██▒
       ▒█░ █ ░█ ▒██  ▀█▄  ▓██ ░▄█ ▒   ░ ▓██▄   ▒██▒▓██    ▓██░▓██  ▒██░▒██░   ▒██  ▀█▄ ▒ ▓██░ ▒░▒██░  ██▒▓██ ░▄█ ▒
       ░█░ █ ░█ ░██▄▄▄▄██ ▒██▀▀█▄       ▒   ██▒░██░▒██    ▒██ ▓▓█  ░██░▒██░   ░██▄▄▄▄██░ ▓██▓ ░ ▒██   ██░▒██▀▀█▄  
       ░░██▒██▓  ▓█   ▓██▒░██▓ ▒██▒   ▒██████▒▒░██░▒██▒   ░██▒▒▒█████▓ ░██████▒▓█   ▓██▒ ▒██▒ ░ ░ ████▓▒░░██▓ ▒██▒
       ░ ▓░▒ ▒   ▒▒   ▓▒█░░ ▒▓ ░▒▓░   ▒ ▒▓▒ ▒ ░░▓  ░ ▒░   ░  ░░▒▓▒ ▒ ▒ ░ ▒░▓  ░▒▒   ▓▒█░ ▒ ░░   ░ ▒░▒░▒░ ░ ▒▓ ░▒▓░
         ▒ ░ ░    ▒   ▒▒ ░  ░▒ ░ ▒░   ░ ░▒  ░ ░ ▒ ░░  ░      ░░░▒░ ░ ░ ░ ░ ▒  ░ ▒   ▒▒ ░   ░      ░ ▒ ▒░   ░▒ ░ ▒░
         ░   ░    ░   ▒     ░░   ░    ░  ░  ░   ▒ ░░      ░    ░░░ ░ ░   ░ ░    ░   ▒    ░      ░ ░ ░ ▒    ░░   ░ 
           ░          ░  ░   ░              ░   ░         ░      ░         ░  ░     ░  ░            ░ ░     ░     "

        Dim xOffset As Integer = 24
        Dim shopOption, buttonNumber As Integer
        buttonNumber = 3
        Dim person As Integer = 0
        Dim currentCard As Object = New BasicKnight

        While True
            shopOption = shopButtons(xOffset, person, buttonNumber, player, isComputer, turn)

            Select Case person
                Case 0
                    currentCard = New BasicKnight()
                Case 1
                    currentCard = New TrainedSoldier()
                Case 2
                    currentCard = New LegendaryWarrior()
            End Select

            Select Case shopOption
                Case 0 'Left
                    buttonNumber = 0
                    If person > 0 Then
                        person -= 1
                    End If
                Case 2 'Right
                    buttonNumber = 2
                    If person < 2 Then
                        person += 1
                    End If
                Case 1 ' BUY
                    buttonNumber = 1
                    player.coins -= currentCard.price
                    player.cards.Add(currentCard)
                Case 3 'Exit
                    Console.Clear()
                    fightShop(title, player, isComputer:=isComputer, turn:=turn)
                    Exit While
            End Select

        End While

        Console.Clear()



    End Sub

    Function selectCard(ByVal player As Player, ByVal isComputer As Boolean, ByVal turn As Integer)
        Const title As String = "       █     █░ ▄▄▄       ██▀███       ██████  ██▓ ███▄ ▄███▓ █    ██  ██▓    ▄▄▄     ▄▄▄█████▓ ▒█████   ██▀███  
       ▓█░ █ ░█░▒████▄    ▓██ ▒ ██▒   ▒██    ▒ ▓██▒▓██▒▀█▀ ██▒ ██  ▓██▒▓██▒   ▒████▄   ▓  ██▒ ▓▒▒██▒  ██▒▓██ ▒ ██▒
       ▒█░ █ ░█ ▒██  ▀█▄  ▓██ ░▄█ ▒   ░ ▓██▄   ▒██▒▓██    ▓██░▓██  ▒██░▒██░   ▒██  ▀█▄ ▒ ▓██░ ▒░▒██░  ██▒▓██ ░▄█ ▒
       ░█░ █ ░█ ░██▄▄▄▄██ ▒██▀▀█▄       ▒   ██▒░██░▒██    ▒██ ▓▓█  ░██░▒██░   ░██▄▄▄▄██░ ▓██▓ ░ ▒██   ██░▒██▀▀█▄  
       ░░██▒██▓  ▓█   ▓██▒░██▓ ▒██▒   ▒██████▒▒░██░▒██▒   ░██▒▒▒█████▓ ░██████▒▓█   ▓██▒ ▒██▒ ░ ░ ████▓▒░░██▓ ▒██▒
       ░ ▓░▒ ▒   ▒▒   ▓▒█░░ ▒▓ ░▒▓░   ▒ ▒▓▒ ▒ ░░▓  ░ ▒░   ░  ░░▒▓▒ ▒ ▒ ░ ▒░▓  ░▒▒   ▓▒█░ ▒ ░░   ░ ▒░▒░▒░ ░ ▒▓ ░▒▓░
         ▒ ░ ░    ▒   ▒▒ ░  ░▒ ░ ▒░   ░ ░▒  ░ ░ ▒ ░░  ░      ░░░▒░ ░ ░ ░ ░ ▒  ░ ▒   ▒▒ ░   ░      ░ ▒ ▒░   ░▒ ░ ▒░
         ░   ░    ░   ▒     ░░   ░    ░  ░  ░   ▒ ░░      ░    ░░░ ░ ░   ░ ░    ░   ▒    ░      ░ ░ ░ ▒    ░░   ░ 
           ░          ░  ░   ░              ░   ░         ░      ░         ░  ░     ░  ░            ░ ░     ░     "

        Dim xOffset As Integer = 24
        Dim shopOption, buttonNumber As Integer
        buttonNumber = 3
        Dim person As Integer = 0
        Dim currentCardIndex As Integer = 0
        Dim currentCard As Object = player.cards(currentCardIndex)
        Dim selectedIndex As Integer = 0
        While True


            currentCard = player.cards(currentCardIndex)
            shopOption = selectButtons(xOffset, currentCard, buttonNumber, player, selectedIndex, isComputer:=isComputer, turn:=turn)

            Select Case shopOption
                Case 0 'Left
                    buttonNumber = 0
                    If currentCardIndex > 0 Then
                        currentCardIndex -= 1
                    End If
                Case 2 'Right
                    buttonNumber = 2
                    If currentCardIndex < player.cards.Count - 1 Then
                        currentCardIndex += 1
                    End If
                Case 1 ' Use
                    buttonNumber = 1
                    selectedIndex = currentCardIndex

                Case 3 'Fight
                    Console.Clear()
                    Exit While
            End Select

        End While
        Return selectedIndex
        Console.Clear()
    End Function

    Function showkillshigh(ByVal Player As Player, Optional ByVal high As Boolean = False)
        Player.updateRating()
        Dim kills As Integer = Player.opponentsKilled, rating As Integer = Player.rating

        If high Then
            Dim stats As Object = getRateKills()
            kills = stats(0)
            rating = stats(1)
        End If

        Console.ForegroundColor = ConsoleColor.White
        If high Then
            Console.ForegroundColor = ConsoleColor.Cyan
        End If

        Console.Write("Player rating: ")
        Console.ForegroundColor = ConsoleColor.Cyan
        If high Then
            Console.ForegroundColor = ConsoleColor.DarkGreen
        End If
        Console.Write(rating)

        Console.ForegroundColor = ConsoleColor.White
        If high Then
            Console.ForegroundColor = ConsoleColor.Cyan
        End If
        Console.Write("   Opponents killed: ")

        Console.ForegroundColor = ConsoleColor.Cyan
        If high Then
            Console.ForegroundColor = ConsoleColor.DarkGreen
        End If
        Console.Write(kills)

        Console.ForegroundColor = ConsoleColor.White

        Return kills.ToString.Length + rating.ToString.Length
    End Function

    Sub showkills(ByVal Player As Player, Optional ByVal high As Boolean = False, Optional fg As ConsoleColor = ConsoleColor.Cyan)
        Player.updateRating()
        Dim kills As Integer = Player.opponentsKilled, rating As Integer = Player.rating

        If high Then
            Dim stats As Object = getRateKills()
            kills = stats(0)
            rating = stats(1)
        End If

        Console.ForegroundColor = ConsoleColor.White
        If high Then
            Console.ForegroundColor = fg
        End If

        Console.Write("                                       Player rating: ")
        Console.ForegroundColor = fg
        If high Then
            Console.ForegroundColor = ConsoleColor.DarkGreen
        End If
        Console.Write(rating)

        Console.ForegroundColor = ConsoleColor.White
        If high Then
            Console.ForegroundColor = fg
        End If
        Console.Write("   Opponents killed: ")

        Console.ForegroundColor = fg
        If high Then
            Console.ForegroundColor = ConsoleColor.DarkGreen
        End If
        Console.WriteLine(kills)

        Console.ForegroundColor = ConsoleColor.White


    End Sub

    Sub fightShop(ByVal title As String, ByVal player As Player, Optional ByVal yOverride As Boolean = False, Optional ByVal isComputer As Boolean = True, Optional ByVal turn As Integer = 0)
        saveRateKills(player)
        Dim input As String
        Dim inputY As Integer = 0
        Dim len As Integer = showkillshigh(player, True)
        Const bg As ConsoleColor = ConsoleColor.Black
        Dim fg As ConsoleColor = ConsoleColor.DarkCyan
        Dim buttonGap As Integer = 1
        If yOverride Then
            inputY = 1
        End If

        Console.Clear()
        Console.ForegroundColor = ConsoleColor.DarkRed
        Console.Write(title)
        Console.SetCursorPosition(0, Console.CursorTop - 1)




        If isComputer Then
            showkills(player)
            Console.ForegroundColor = ConsoleColor.Gray
            Console.Write("---------------------------High Score |")
            showkillshigh(player, True)

            Console.ForegroundColor = ConsoleColor.Gray

            Console.Write(" |High Score")
            For i As Integer = 0 To 33 - len - 1
                Console.Write("-")
            Next
        Else



            If turn = 0 Then
                showkills(player, fg:=ConsoleColor.Cyan)
                Console.WriteLine()
                Console.ForegroundColor = ConsoleColor.Cyan
                Console.Write("---------------------------------------------------- PLAYER ONE --------------------------------------------------------")
            Else
                showkills(player, fg:=ConsoleColor.Magenta)
                Console.WriteLine()
                Console.ForegroundColor = ConsoleColor.Magenta
                Console.Write("---------------------------------------------------- PLAYER TWO --------------------------------------------------------")
            End If
        End If

        Console.WriteLine(vbNewLine)

        Console.ForegroundColor = ConsoleColor.White

        If inputY = 1 Then
            If player.cards.Count > 0 Then
                fightButton(True, bg, fg)
            Else
                fightButton(True, bg, fg, True)
            End If

            For lineNum As Integer = 0 To buttonGap
                Console.WriteLine()
            Next
            shopButton(False, bg, fg)
        ElseIf inputY = 0 Then
            If player.cards.Count > 0 Then
                fightButton(False, bg, fg)
            Else
                fightButton(False, bg, fg, True)
            End If
            For lineNum As Integer = 0 To buttonGap
                Console.WriteLine()
            Next
            shopButton(True, bg, fg)
        End If

        Console.SetCursorPosition(0, 0)
        While True
            input = Console.ReadKey(True).Key
            If input = 38 Or input = 87 Then
                inputY = 1
            ElseIf input = 40 Or input = 83 Then
                inputY = 0
            ElseIf input = 13 Or input = 32 Then
                If inputY = 0 Then
                    shop(player, isComputer:=isComputer, turn:=turn)
                    Exit While
                ElseIf inputY = 1 Then
                    If player.cards.Count > 0 Then
                        Exit While
                    End If

                End If

            End If

            Console.Clear()
            Console.ForegroundColor = ConsoleColor.DarkRed
            Console.Write(title)
            Console.SetCursorPosition(0, Console.CursorTop - 1)
            If isComputer Then
                showkills(player)
                Console.ForegroundColor = ConsoleColor.Gray
                Console.Write("---------------------------High Score |")
                showkillshigh(player, True)

                Console.ForegroundColor = ConsoleColor.Gray

                Console.Write(" |High Score")
                For i As Integer = 0 To 33 - len - 1
                    Console.Write("-")
                Next
            Else



                If turn = 0 Then
                    showkills(player, fg:=ConsoleColor.Cyan)
                    Console.WriteLine()
                    Console.ForegroundColor = ConsoleColor.Cyan
                    Console.Write("---------------------------------------------------- PLAYER ONE --------------------------------------------------------")
                Else
                    showkills(player, fg:=ConsoleColor.Magenta)
                    Console.WriteLine()
                    Console.ForegroundColor = ConsoleColor.Magenta
                    Console.Write("---------------------------------------------------- PLAYER TWO --------------------------------------------------------")
                End If
            End If

            Console.WriteLine(vbNewLine)

            Console.ForegroundColor = ConsoleColor.White


            If inputY = 1 Then
                If player.cards.Count > 0 Then
                    fightButton(True, bg, fg)
                Else
                    fightButton(True, bg, fg, True)
                End If

                For lineNum As Integer = 0 To buttonGap
                    Console.WriteLine()
                Next
                shopButton(False, bg, fg)
            ElseIf inputY = 0 Then
                If player.cards.Count > 0 Then
                    fightButton(False, bg, fg)
                Else
                    fightButton(False, bg, fg, True)
                End If
                For lineNum As Integer = 0 To buttonGap
                    Console.WriteLine()
                Next
                shopButton(True, bg, fg)
            End If
            Console.SetCursorPosition(0, 0)
        End While



    End Sub



    Sub tempAskCard(ByVal player As Player)
        Dim cardN As String

        Console.Clear()
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Write($"Pick a card (K,S,W): ")
        cardN = Console.ReadLine().ToLower
        Select Case cardN
            Case "k"
                player.cards.Add(New BasicKnight())
            Case "s"
                player.cards.Add(New TrainedSoldier())
            Case "w"
                player.cards.Add(New LegendaryWarrior())
        End Select

    End Sub




    Sub AttackButton(ByVal rPad As Integer, ByVal selected As Boolean, ByVal cX As Integer, ByVal cY As Integer, Optional ByVal bgColor As ConsoleColor = ConsoleColor.Black, Optional ByVal fgColor As ConsoleColor = ConsoleColor.DarkRed, Optional ByVal endC As String = "", Optional ByVal disabled As Boolean = False)
        Dim line As String
        Dim lineIndex As Integer = 0

        Dim button As String() = {"", "       _   _           _    ", "  __ _| |_| |_ __ _ __| |__ ", " / _` |  _|  _/ _` / _| / / ", " \__,_|\__|\__\__,_\__|_\_\ "}
        If selected Then
            button = {" ---------------------------", "|      _   _           _    |", "| __ _| |_| |_ __ _ __| |__ |", "|/ _` |  _|  _/ _` / _| / / |", "|\__,_|\__|\__\__,_\__|_\_\ |", "|                           |", " ---------------------------"}
        End If
        'FONT = small SMUSH(U)*2
        Console.BackgroundColor = bgColor
        Console.ForegroundColor = fgColor
        If disabled Then
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If

        Console.SetCursorPosition(cX, cY)
        For lineIndex = 0 To button.Length - 2
            line = button(lineIndex)
            For i As Integer = 0 To rPad
                Console.Write(" ")
            Next
            Console.WriteLine(line)
            Console.SetCursorPosition(cX, lineIndex + 1 + cY)
        Next
        For i As Integer = 0 To rPad
            Console.Write(" ")
        Next

        Console.Write($"{button(lineIndex)}{endC}")
    End Sub

    Sub DefendButton(ByVal rPad As Integer, ByVal selected As Boolean, ByVal cX As Integer, ByVal cY As Integer, Optional ByVal bgColor As ConsoleColor = ConsoleColor.Black, Optional ByVal fgColor As ConsoleColor = ConsoleColor.White, Optional ByVal endC As String = "", Optional ByVal disabled As Boolean = False)
        Dim line As String
        Dim lineIndex As Integer = 0
        Dim button As String() = {"", "     _      __             _ ", "  __| |___ / _|___ _ _  __| |", " / _` / -_|  _/ -_| ' \/ _` |", " \__,_\___|_| \___|_||_\__,_|"}
        If selected Then
            button = {" ----------------------------", "|    _      __             _ |", "| __| |___ / _|___ _ _  __| ||", "|/ _` / -_|  _/ -_| ' \/ _` ||", "|\__,_\___|_| \___|_||_\__,_||", "|                            |", " ----------------------------"}
        End If


        Console.BackgroundColor = bgColor
        Console.ForegroundColor = fgColor
        If disabled Then
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If
        Console.SetCursorPosition(cX, cY)
        For lineIndex = 0 To button.Length - 2
            line = button(lineIndex)
            For i As Integer = 0 To rPad
                Console.Write(" ")
            Next
            Console.WriteLine(line)
            Console.SetCursorPosition(cX, lineIndex + 1 + cY)
        Next
        For i As Integer = 0 To rPad
            Console.Write(" ")
        Next

        Console.Write($"{button(lineIndex)}{endC}")
        Console.SetCursorPosition(cX, cY)
    End Sub

    Sub HealButton(ByVal rPad As Integer, ByVal selected As Boolean, ByVal cX As Integer, ByVal cY As Integer, Optional ByVal bgColor As ConsoleColor = ConsoleColor.Black, Optional ByVal fgColor As ConsoleColor = ConsoleColor.DarkGreen, Optional ByVal endC As String = "", Optional ByVal disabled As Boolean = False)
        Dim line As String
        Dim lineIndex As Integer = 0
        Dim button As String() = {"", "  _             _ ", " | |_  ___ __ _| |", " | ' \/ -_/ _` | |", " |_||_\___\__,_|_|"}
        If selected Then
            button = {" -----------------", "| _             _ |", "|| |_  ___ __ _| ||", "|| ' \/ -_/ _` | ||", "||_||_\___\__,_|_||", "|                 |", " -----------------"}
        End If

        Console.BackgroundColor = bgColor
        Console.ForegroundColor = fgColor
        If disabled Then
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If
        Console.SetCursorPosition(cX, cY)
        For lineIndex = 0 To button.Length - 2
            line = button(lineIndex)
            For i As Integer = 0 To rPad
                Console.Write(" ")
            Next
            Console.WriteLine(line)
            Console.SetCursorPosition(cX, lineIndex + 1 + cY)
        Next
        For i As Integer = 0 To rPad
            Console.Write(" ")
        Next

        Console.Write($"{button(lineIndex)}{endC}")
        Console.SetCursorPosition(cX, cY)
    End Sub

    Sub waitForEnter()
        Dim input As Integer
        While True
            input = Console.ReadKey(True).Key
            If input = 13 Or input = 32 Then
                Exit While
            End If
        End While

    End Sub

    Sub waitForInput()
        Dim input As Integer
        While True
            input = Console.ReadKey(True).Key
            If input = 13 Or input = 32 Or input = 37 Or input = 65 Or input = 39 Or input = 68 Then
                Exit While
            End If
        End While
    End Sub

    Sub PowerButton(ByVal rPad As Integer, ByVal selected As Boolean, ByVal cX As Integer, ByVal cY As Integer, Optional ByVal bgColor As ConsoleColor = ConsoleColor.Black, Optional ByVal fgColor As ConsoleColor = ConsoleColor.Blue, Optional ByVal endC As String = "", Optional ByVal disabled As Boolean = False)
        Dim line As String
        Dim lineIndex As Integer = 0
        Dim button As String() = {"", "  _ __ _____ __ _____ _ _ ", " | '_ / _ \ V  V / -_| '_|", " | .__\___/\_/\_/\___|_|  ", " |_|                      "}
        If selected Then
            button = {" -------------------------", "| _ __ _____ __ _____ _ _ |", "|| '_ / _ \ V  V / -_| '_||", "|| .__\___/\_/\_/\___|_|  |", "||_|                      |", "|                         |", " -------------------------"}
        End If

        Console.BackgroundColor = bgColor
        Console.ForegroundColor = fgColor
        If disabled Then
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If
        Console.SetCursorPosition(cX, cY)
        For lineIndex = 0 To button.Length - 2
            line = button(lineIndex)
            For i As Integer = 0 To rPad
                Console.Write(" ")
            Next
            Console.WriteLine(line)
            Console.SetCursorPosition(cX, lineIndex + 1 + cY)
        Next
        For i As Integer = 0 To rPad
            Console.Write(" ")
        Next

        Console.Write($"{button(lineIndex)}{endC}")
        Console.SetCursorPosition(cX, cY)
    End Sub
    Sub showStatsCard(ByVal PlayerCard As Object, ByVal oppCard As Object, ByVal rPad As Integer, Optional ByVal cY As Integer = 0, Optional ByVal showOpp As Boolean = False)
        Dim attackValue As Integer
        Dim fg As ConsoleColor = ConsoleColor.DarkYellow
        Dim fgText As ConsoleColor = ConsoleColor.Yellow
        Dim yOffset As Integer = -3
        If showOpp Then
            yOffset = -4
        End If


        Console.SetCursorPosition(rPad, Console.CursorTop + yOffset + cY)
        Console.ForegroundColor = fg
        Console.Write(" --------")
        Console.ForegroundColor = fgText
        Console.Write("STATS")
        Console.ForegroundColor = fg
        Console.WriteLine("-------- ")
        Console.ForegroundColor = fgText

        Console.SetCursorPosition(rPad, Console.CursorTop)

        Console.ForegroundColor = fg
        Console.WriteLine("|                     | ")
        Console.SetCursorPosition(rPad, Console.CursorTop)



        attackValue = PlayerCard.Attack + PlayerCard.AttackBoost
        If oppCard.isDefending Then
            attackValue *= 1 - (oppCard.Defence + oppCard.DefenceBoost)
        End If

        Console.Write("|")
        Console.ForegroundColor = fgText
        Console.Write($"     Attack: -{attackValue}hp    ")
        Console.ForegroundColor = fg
        Console.WriteLine("|")


        Console.SetCursorPosition(rPad, Console.CursorTop)

        Console.Write("|")
        Console.ForegroundColor = fgText
        Console.Write($"     Heal: +{PlayerCard.Heal}hp      ")
        Console.ForegroundColor = fg
        Console.WriteLine("|")


        Console.SetCursorPosition(rPad, Console.CursorTop)

        Console.ForegroundColor = fg
        Console.Write("|")
        Console.ForegroundColor = fgText
        Console.Write($"     Defence: {(PlayerCard.Defence + PlayerCard.DefenceBoost) * 100}%  ")
        Console.ForegroundColor = fg
        If (PlayerCard.Defence + PlayerCard.DefenceBoost).ToString().Length > 2 Then
            Console.WriteLine("  |")
        Else
            Console.WriteLine(" |")
        End If
        Console.SetCursorPosition(rPad, Console.CursorTop)

        If PlayerCard.isDefending Then
            Console.Write("|   ")
        Else
            Console.Write("|  ")
        End If
        Console.ForegroundColor = fgText
        Console.Write($"Defending: {PlayerCard.isDefending}   ")
        Console.ForegroundColor = fg
        Console.WriteLine("|")
        Console.SetCursorPosition(rPad, Console.CursorTop)

        '------------------------------------------------

        Console.ForegroundColor = fg
        If PlayerCard.usedPower Then
            Console.Write("|   ")
        Else
            Console.Write("|  ")
        End If
        Console.ForegroundColor = fgText
        Console.Write($"PowerUsed: {PlayerCard.usedPower}   ")
        Console.ForegroundColor = fg
        Console.WriteLine("|")
        Console.SetCursorPosition(rPad, Console.CursorTop)

        Console.ForegroundColor = fg
        Console.WriteLine("|                     |")
        Console.SetCursorPosition(rPad, Console.CursorTop)


        Console.Write("|-----")
        Console.ForegroundColor = fgText
        Console.Write("Power Stats")
        Console.ForegroundColor = fg
        Console.WriteLine("-----|")
        Console.SetCursorPosition(rPad, Console.CursorTop)

        Console.ForegroundColor = fg
        Console.WriteLine("|                     |")
        Console.SetCursorPosition(rPad, Console.CursorTop)

        Console.Write("|")
        Console.ForegroundColor = fgText
        Console.Write("    Defence: 100%    ")
        Console.ForegroundColor = fg
        Console.WriteLine("|")

        Console.SetCursorPosition(rPad, Console.CursorTop)

        Console.Write("|")
        Console.ForegroundColor = fgText
        Console.Write($"     Attack: -4hp    ")
        Console.ForegroundColor = fg
        Console.WriteLine("|")

        Console.SetCursorPosition(rPad, Console.CursorTop)

        Console.ForegroundColor = fg
        Console.Write("|")
        Console.ForegroundColor = fgText
        Console.Write($"   Defending: True   ")
        Console.ForegroundColor = fg
        Console.WriteLine("|")
        Console.SetCursorPosition(rPad, Console.CursorTop)

        Console.Write("|")
        Console.ForegroundColor = fgText
        Console.Write($"    Health:  {round(PlayerCard.MaxHealth * 0.5)}hp     ")
        Console.ForegroundColor = fg
        Console.WriteLine("|")
        Console.SetCursorPosition(rPad, Console.CursorTop)
        Console.WriteLine(" --------------------- ")
        Console.ForegroundColor = ConsoleColor.White
    End Sub
    Sub showVisualStats(ByVal PlayerCard As Object, ByVal OppCard As Object, Optional ByVal shopDisplay As Boolean = False)
        Dim full, empty As String
        Dim tPad As Integer = 0
        Dim line As Integer = 0
        Dim rPad As Integer = 35
        Dim xOffset As Integer
        Dim attackValue As Integer

        full = "▓▓"
        empty = "▓▓"
        'MAKE CURSOR INVIS



        If (shopDisplay) Then
            Select Case PlayerCard.Name
                Case "Basic Knight"
                    xOffset = -2
                Case "Trained Soldier"
                    xOffset = -4
                Case "Legendary Warrior"
                    xOffset = -7
            End Select

            For line = 0 To tPad + 28
                Console.WriteLine()
                Console.SetCursorPosition(rPad + xOffset, line)
            Next

            Console.ForegroundColor = ConsoleColor.White
            Console.Write($"Health:")
            Console.Write("[")
            Console.ForegroundColor = ConsoleColor.Green

            For i As Integer = 0 To PlayerCard.Health - 1
                Console.Write(full)
            Next
            Console.ForegroundColor = ConsoleColor.Gray


            For j As Integer = 0 To PlayerCard.MaxHealth - PlayerCard.Health - 1
                Console.Write(empty)
            Next

            Console.WriteLine($"]({PlayerCard.Health}/{PlayerCard.MaxHealth})")
            Exit Sub
        End If

        For line = 0 To tPad
            Console.WriteLine()
            Console.SetCursorPosition(rPad, line)
        Next

        Console.ForegroundColor = ConsoleColor.White
        Console.Write($"Player Health ({PlayerCard.Health}/{PlayerCard.MaxHealth}):")

        Console.WriteLine()
        Console.WriteLine()

        Console.SetCursorPosition(rPad, Console.CursorTop)




        Console.Write("[")
        Console.ForegroundColor = ConsoleColor.Green
        If PlayerCard.Attack + PlayerCard.AttackBoost = 4 Or (PlayerCard.Defence + PlayerCard.DefenceBoost) * 100 = 100 Then
            Console.ForegroundColor = ConsoleColor.Blue
        End If


        attackValue = (OppCard.Attack + OppCard.AttackBoost)
        If PlayerCard.isDefending Then
            attackValue = (OppCard.Attack + OppCard.AttackBoost) * (1 - (PlayerCard.Defence + PlayerCard.DefenceBoost))
        End If

        If PlayerCard.Health - attackValue <= 0 Then
            Console.ForegroundColor = ConsoleColor.Red
        End If

        For i As Integer = 0 To PlayerCard.Health - 1
            Console.Write(full)
        Next
        Console.ForegroundColor = ConsoleColor.Gray


        For j As Integer = 0 To PlayerCard.MaxHealth - PlayerCard.Health - 1
            Console.Write(empty)
        Next

        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine($"]{round(PlayerCard.Health / PlayerCard.MaxHealth * 100)}%")

        Console.WriteLine()
        Console.SetCursorPosition(rPad, Console.CursorTop)
        Console.Write("Heals Left: ")
        Console.WriteLine($"({PlayerCard.Heals}/{PlayerCard.MaxHeals})")

        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine()

        showStatsCard(PlayerCard, OppCard, rPad)



        Console.SetCursorPosition(0, 0)

    End Sub

    Sub ShowVisualStatsOpponent(ByVal OpponentCard As Object, ByVal PlayerCard As Object)
        Dim full, empty As String
        Dim tPad As Integer = 0
        Dim line As Integer = 0
        Dim rPad As Integer = 94
        Dim attackValue As Integer

        full = "▓▓"
        empty = "▓▓"

        For line = 0 To tPad
            Console.WriteLine()
            Console.SetCursorPosition(rPad, line)
        Next

        Console.ForegroundColor = ConsoleColor.White
        Console.Write($"Opponent Health ({OpponentCard.Health}/{OpponentCard.MaxHealth}):")

        Console.WriteLine()
        Console.WriteLine()

        Console.SetCursorPosition(rPad, Console.CursorTop)




        Console.Write("[")
        Console.ForegroundColor = ConsoleColor.Green
        If OpponentCard.Attack + OpponentCard.AttackBoost = 4 Or (OpponentCard.Defence + OpponentCard.DefenceBoost) * 100 = 100 Then
            Console.ForegroundColor = ConsoleColor.Blue
        End If

        attackValue = (PlayerCard.Attack + PlayerCard.AttackBoost)

        If OpponentCard.isDefending Then
            attackValue = (PlayerCard.Attack + PlayerCard.AttackBoost) * (1 - (OpponentCard.Defence + OpponentCard.DefenceBoost))
        End If



        If OpponentCard.Health - attackValue <= 0 Then
            Console.ForegroundColor = ConsoleColor.Red
        End If

        For i As Integer = 0 To OpponentCard.Health - 1
            Console.Write(full)
        Next
        Console.ForegroundColor = ConsoleColor.Gray


        For j As Integer = 0 To OpponentCard.MaxHealth - OpponentCard.Health - 1
            Console.Write(empty)
        Next

        Console.ForegroundColor = ConsoleColor.White

        Console.WriteLine($"]{round(OpponentCard.Health / OpponentCard.MaxHealth * 100)}%")


        Console.WriteLine()
        Console.SetCursorPosition(rPad, Console.CursorTop)
        Console.Write("Heals Left: ")
        Console.WriteLine($"({OpponentCard.Heals}/{OpponentCard.MaxHeals})")

        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine()

        showStatsCard(OpponentCard, PlayerCard, rPad)

        Console.SetCursorPosition(0, 0)
    End Sub

    Sub displayCharAndStats(ByVal PlayerCard As Object, ByVal OpponentCard As Object, ByVal PlayerSprite As String(), ByVal OpponentSprite As String())
        Dim fg As ConsoleColor = ConsoleColor.White
        Dim bg As ConsoleColor = ConsoleColor.Black

        Dim pTextColorFG As ConsoleColor = ConsoleColor.Cyan
        Dim oTextColorFG As ConsoleColor = ConsoleColor.Magenta

        Dim index As Integer = 0

        For Each line As String In PlayerSprite
            index = Array.IndexOf(PlayerSprite, line)

            Console.ForegroundColor = fg
            Console.BackgroundColor = bg

            If fightTurn = 1 Then
                Console.ForegroundColor = ConsoleColor.DarkGray
            End If

            If index = PlayerSprite.Length - 1 Then
                Console.ForegroundColor = pTextColorFG

            End If

            Console.SetCursorPosition(0, Console.CursorTop)
            Console.WriteLine(line)
        Next

        showVisualStats(PlayerCard, OpponentCard)

        For Each line As String In OpponentSprite
            index = Array.IndexOf(OpponentSprite, line)
            Console.ForegroundColor = fg
            Console.BackgroundColor = bg
            If fightTurn = 0 Then
                Console.ForegroundColor = ConsoleColor.DarkGray
            End If
            If index = OpponentSprite.Length - 1 Then
                Console.ForegroundColor = oTextColorFG
            End If

            Select Case OpponentCard.Name
                Case "Basic Knight"
                    Console.SetCursorPosition(65, Console.CursorTop)
                Case "Trained Soldier"
                    Console.SetCursorPosition(62, Console.CursorTop)
                Case "Legendary Warrior"
                    Console.SetCursorPosition(61, Console.CursorTop)
            End Select

            Console.WriteLine(line)
        Next
        Console.ForegroundColor = fg
        Console.BackgroundColor = bg
        ShowVisualStatsOpponent(OpponentCard, PlayerCard)

        Console.SetCursorPosition(0, 0)
    End Sub

    Function doOptionButtons(ByVal PlayerCard As Object, ByVal OpponentCard As Object, Optional ByVal yOffset As Integer = 0, Optional ByVal wantSprites As Boolean = False, Optional isComputer As Boolean = True) As String()()
        Dim rPad As Integer = 2
        Dim maxAttackLen As Integer = 29 + 1 + rPad
        Dim maxDefendLen As Integer = 30 + 1 + rPad
        Dim maxHealLen As Integer = 19 + 1 + rPad
        Dim input, xInput As Integer
        Dim selected As String() = {"null"}
        Dim returnValue As String()() = {}
        Dim playerSprite, opponentSprite As String()
        playerSprite = {}
        opponentSprite = {}

        Dim kSprite As String() = {"         .--.", "        /.--.\", "        |====|", "        |`::`|", "    .-;`\..../`;-.", "   /  |...::...|  \", "  |   /'''::'''\   |", "  ;--'\   ::   /\--;", "  <__>,>._::_.<,<__>", "  |  |/   ^^   \|  |", "  \::/|        |\::/", "  |||\|        |/|||", "  ''' |___/\___| '''", "       \_ || _/", "       <_ >< _>", "       |  ||  |", "       |  ||  |", "      _\.:||:./_", "     /____/\____\", "", "     Basic Knight"}
        Dim sSprite As String() = {" /\          .--.", " ||         /.--.\", " ||         |====|", " ||         |`::`|", "_||_    .-;`\..../`;-.", " /\\   /  |...::...|  \", " |:'\ |   /'''::'''\   |", "  \ /\;-,/\   ::   /\--;", "   \ <` >  >._::_.<,<__>", "    `""`   /   ^^   \|  |", "          |        |\::/", "          |        |/|||", "          |___/\___| '''", "           \_ || _/", "           <_ >< _>", "           |  ||  |", "           |  ||  |", "          _\.:||:./_", "         /____/\____\", "", "       Trained  Soldier"}
        Dim wSprite As String() = {"  ,   |          .--.", " / \, | ,       /.--.\", "|    =|= >      |====|", " \ /` | `       |`::`|", "  `   |     .-;`\..../`;-.", "     /\\/  /  |...::...|  \", "     |:'\ |   /'''::'''\   |", "      \ /\;-,/\   ::   /\--;", "      |\ <` >  >._::_.<,<__>", "      | `""`   /   ^^   \|  |", "      |       |        |\::/", "      |       |        |/|||", "      |       |___/\___| '''", "      |        \_ || _/", "      |        <_ >< _>", "      |        |  ||  |", "      |        |  ||  |", "      |       _\.:||:./_", "      |      /____/\____\", "", "          Legendary  Warrior"}

        Dim kSpriteD As String() = {"         .--.", "        /.--.\", "        |====|", "        |`::`|", "    .-;`\..../`;_.-^-._", "   /  |...::..|`   :   `|", "  |   /'''::''|   .:.   |", "  ;--'\   ::  |..:::::..|", "  <__> >._::_.| ':::::' |", "  |  |/   ^^  |   ':'   |", "  \::/|       \    :    /", "  |||\|        \   :   /", "  ''' |___/\___|`-.:.-`", "       \_ || _/    `", "       <_ >< _>", "       |  ||  |", "       |  ||  |", "      _\.:||:./_", "     /____/\____\", "", "     Basic Knight"}
        Dim sSpriteD As String() = {" /\          .--.", " ||         /.--.\", " ||         |====|", " ||         |`::`|", "_||_    .-;`\..../`;_.-^-._", " /\\   /  |...::..|`   :   `|", " |:'\ |   /'''::''|   .:.   |", "  \ /\;-,/\   ::  |..:::::..|", "   \ <` >  >._::_.| ':::::' |", "    `""`   /   ^^  |   ':'   |", "          |       \    :    /", "          |        \   :   /", "          |___/\___|`-.:.-`", "           \_ || _/    `", "           <_ >< _>", "           |  ||  |", "           |  ||  |", "          _\.:||:./_", "         /____/\____\", "", "       Trained  Soldier"}
        Dim wSpriteD As String() = {"  ,   |          .--.", " / \, | ,       /.--.\", "|    =|= >      |====|", " \ /` | `       |`::`|  ", "  `   |     .-;`\..../`;_.-^-._", "     /\\/  /  |...::..|`   :   `|", "     |:'\ |   /'''::''|   .:.   |", "      \ /\;-,/\   ::  |..:::::..|", "      |\ <` >  >._::_.| ':::::' |", "      | `""`   /   ^^  |   ':'   |", "      |       |       \    :    /", "      |       |        \   :   / ", "      |       |___/\___|`-.:.-`", "      |        \_ || _/    `", "      |        <_ >< _>", "      |        |  ||  |", "      |        |  ||  |", "      |       _\.:||:./_", "      |      /____/\____\", "", "          Legendary  Warrior"}

        xInput = 0
        'Change THE MAX LENS WHEN INPLEMENTING KEYS 
        Console.Clear()


        Select Case PlayerCard.Name
            Case "Basic Knight"
                If PlayerCard.isDefending Then
                    playerSprite = kSpriteD
                Else
                    playerSprite = kSprite
                End If

            Case "Trained Soldier"
                If PlayerCard.isDefending Then
                    playerSprite = sSpriteD
                Else
                    playerSprite = sSprite
                End If

            Case "Legendary Warrior"
                If PlayerCard.isDefending Then
                    playerSprite = wSpriteD
                Else
                    playerSprite = wSprite
                End If

        End Select

        Select Case OpponentCard.Name
            Case "Basic Knight"
                If OpponentCard.isDefending Then
                    opponentSprite = kSpriteD
                Else
                    opponentSprite = kSprite
                End If

            Case "Trained Soldier"
                If OpponentCard.isDefending Then
                    opponentSprite = sSpriteD
                Else
                    opponentSprite = sSprite
                End If

            Case "Legendary Warrior"
                If OpponentCard.isDefending Then
                    opponentSprite = wSpriteD
                Else
                    opponentSprite = wSprite
                End If

        End Select


        If wantSprites Then
            returnValue = {playerSprite, opponentSprite}
            Return returnValue
        End If

        displayCharAndStats(PlayerCard, OpponentCard, playerSprite, opponentSprite)



        AttackButton(rPad, True, 0, yOffset)

        DefendButton(rPad, False, maxAttackLen, yOffset)

        If PlayerCard.Heals < 1 Then
            HealButton(rPad, False, maxDefendLen + maxAttackLen, yOffset, disabled:=True)
        Else
            HealButton(rPad, False, maxDefendLen + maxAttackLen, yOffset)
        End If

        If isComputer Then
            If PlayerCard.usedPower Then
                PowerButton(rPad, False, maxDefendLen + maxAttackLen + maxHealLen, yOffset, disabled:=True)
            Else
                PowerButton(rPad, False, maxDefendLen + maxAttackLen + maxHealLen, yOffset, disabled:=False)
            End If
        Else
            If OpponentCard.usedPower Then
                PowerButton(rPad, False, maxDefendLen + maxAttackLen + maxHealLen, yOffset, disabled:=True)
            Else
                PowerButton(rPad, False, maxDefendLen + maxAttackLen + maxHealLen, yOffset, disabled:=False)
            End If
        End If



        Console.SetCursorPosition(0, 0)

        While True
            Select Case PlayerCard.Name
                Case "Basic Knight"
                    If PlayerCard.isDefending Then
                        playerSprite = kSpriteD
                    Else
                        playerSprite = kSprite
                    End If

                Case "Trained Soldier"
                    If PlayerCard.isDefending Then
                        playerSprite = sSpriteD
                    Else
                        playerSprite = sSprite
                    End If

                Case "Legendary Warrior"
                    If PlayerCard.isDefending Then
                        playerSprite = wSpriteD
                    Else
                        playerSprite = wSprite
                    End If

            End Select

            Select Case OpponentCard.Name
                Case "Basic Knight"
                    If OpponentCard.isDefending Then
                        opponentSprite = kSpriteD
                    Else
                        opponentSprite = kSprite
                    End If

                Case "Trained Soldier"
                    If OpponentCard.isDefending Then
                        opponentSprite = sSpriteD
                    Else
                        opponentSprite = sSprite
                    End If

                Case "Legendary Warrior"
                    If OpponentCard.isDefending Then
                        opponentSprite = wSpriteD
                    Else
                        opponentSprite = wSprite
                    End If

            End Select


            input = Console.ReadKey(True).Key

            If (input = 37 Or input = 65) Then
                If (xInput > 0) Then
                    xInput -= 1
                Else
                    xInput = 3
                End If
            ElseIf (input = 39 Or input = 68) Then
                If xInput < 3 Then
                    xInput += 1
                Else
                    xInput = 0
                End If
            ElseIf (input = 13 Or input = 32) Then

                Select Case xInput
                    Case 0

                        selected = {"a"}

                        Exit While

                    Case 1
                        selected = {"d"}

                        Exit While

                    Case 2
                        If PlayerCard.Heals > 0 Then
                            selected = {"h"}
                            Exit While
                        End If


                    Case 3
                        If isComputer Then
                            If PlayerCard.usedPower = False Then
                                selected = {"p"}

                                Exit While
                            End If
                        Else
                            If OpponentCard.usedPower = False Then
                                selected = {"p"}

                                Exit While
                            End If
                        End If
                End Select
            End If



            Console.Clear()

            displayCharAndStats(PlayerCard, OpponentCard, playerSprite, opponentSprite)

            selectInput(xInput, PlayerCard, OpponentCard, playerSprite, opponentSprite, rPad, yOffset, maxAttackLen, maxDefendLen, maxHealLen, isComputer:=isComputer)
            Console.SetCursorPosition(0, 0)


        End While
        Console.Clear()

        returnValue = {selected}
        Return returnValue
    End Function

    Sub selectInput(ByVal xInput As Integer, ByVal PlayerCard As Object, ByVal OpponentCard As Object, ByVal PlayerSprite As String(), ByVal opponentSprite As String(), ByVal rPad As Integer, ByVal yOffset As Integer, ByVal maxAttacklen As Integer, ByVal maxDefendLen As Integer, ByVal maxHealLen As Integer, ByVal isComputer As Boolean)
        Select Case xInput
            Case 0

                AttackButton(rPad, True, 0, yOffset)

                DefendButton(rPad, False, maxAttacklen, yOffset)

                If PlayerCard.Heals < 1 Then
                    HealButton(rPad, False, maxDefendLen + maxAttacklen, yOffset, disabled:=True)
                Else
                    HealButton(rPad, False, maxDefendLen + maxAttacklen, yOffset, disabled:=False)
                End If

                If isComputer Then
                    If PlayerCard.usedPower Then
                        PowerButton(rPad, False, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=True)
                    Else
                        PowerButton(rPad, False, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=False)
                    End If
                Else
                    If OpponentCard.usedPower Then
                        PowerButton(rPad, False, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=True)
                    Else
                        PowerButton(rPad, False, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=False)
                    End If
                End If


            Case 1


                AttackButton(rPad, False, 0, yOffset)

                DefendButton(rPad, True, maxAttacklen, yOffset)

                If PlayerCard.Heals < 1 Then
                    HealButton(rPad, False, maxDefendLen + maxAttacklen, yOffset, disabled:=True)
                Else
                    HealButton(rPad, False, maxDefendLen + maxAttacklen, yOffset)
                End If



                If isComputer Then
                    If PlayerCard.usedPower Then
                        PowerButton(rPad, False, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=True)
                    Else
                        PowerButton(rPad, False, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=False)
                    End If
                Else
                    If OpponentCard.usedPower Then
                        PowerButton(rPad, False, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=True)
                    Else
                        PowerButton(rPad, False, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=False)
                    End If
                End If

            Case 2


                AttackButton(rPad, False, 0, yOffset)

                DefendButton(rPad, False, maxAttacklen, yOffset)

                If PlayerCard.Heals < 1 Then
                    HealButton(rPad, True, maxDefendLen + maxAttacklen, yOffset, disabled:=True)
                Else
                    HealButton(rPad, True, maxDefendLen + maxAttacklen, yOffset)
                End If



                If isComputer Then
                    If PlayerCard.usedPower Then
                        PowerButton(rPad, False, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=True)
                    Else
                        PowerButton(rPad, False, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=False)
                    End If
                Else
                    If OpponentCard.usedPower Then
                        PowerButton(rPad, False, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=True)
                    Else
                        PowerButton(rPad, False, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=False)
                    End If
                End If
            Case 3


                AttackButton(rPad, False, 0, yOffset)

                DefendButton(rPad, False, maxAttacklen, yOffset)

                If PlayerCard.Heals < 1 Then
                    HealButton(rPad, False, maxDefendLen + maxAttacklen, yOffset, disabled:=True)
                Else
                    HealButton(rPad, False, maxDefendLen + maxAttacklen, yOffset)
                End If

                If PlayerCard.usedPower Then
                    PowerButton(rPad, True, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=True)
                Else
                    PowerButton(rPad, True, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=False)
                End If
                If isComputer Then
                    If PlayerCard.usedPower Then
                        PowerButton(rPad, True, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=True)
                    Else
                        PowerButton(rPad, True, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=False)
                    End If
                Else
                    If OpponentCard.usedPower Then
                        PowerButton(rPad, True, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=True)
                    Else
                        PowerButton(rPad, True, maxDefendLen + maxAttacklen + maxHealLen, yOffset, disabled:=False)
                    End If
                End If
        End Select
    End Sub
    Sub playerMovePrompt()
        Dim prompt As String() = {"  _   _   _  __  __    _     ___ _  _    ___ _          _        _    _       ", " |_) |_) |_ (_  (_    |_ |\ | | |_ |_)    | / \   |\/| / \ \  / |_   / \ |\ | ", " |   | \ |_ __) __)   |_ | \| | |_ | \    | \_/   |  | \_/  \/  |_   \_/ | \| "}
        Dim fGp As ConsoleColor = ConsoleColor.DarkGray
        Dim cXp As Integer = 20
        Dim line As String

        Console.SetCursorPosition(cXp, 25)
        For lineIndex = 0 To prompt.Length - 1
            Console.ForegroundColor = fGp
            line = prompt(lineIndex)
            Console.WriteLine(line)
            Console.SetCursorPosition(cXp, lineIndex + 26)
        Next
        Console.SetCursorPosition(0, 0)
    End Sub
    Sub oppMovePrompt()
        Dim prompt As String() = {"  _   _   _  __  __    _     ___ _  _     _  _   _     _   _   _   _        _     ___   ___ _          _        _ ", " |_) |_) |_ (_  (_    |_ |\ | | |_ |_)   |_ / \ |_)   / \ |_) |_) / \ |\ | |_ |\ | |     | / \   |\/| / \ \  / |_ ", " |   | \ |_ __) __)   |_ | \| | |_ | \   |  \_/ | \   \_/ |   |   \_/ | \| |_ | \| |     | \_/   |  | \_/  \/  |_"}
        Dim fGp As ConsoleColor = ConsoleColor.DarkGray
        Dim cXp As Integer = 3
        Dim line As String

        Console.SetCursorPosition(cXp, 25)
        For lineIndex = 0 To prompt.Length - 1
            Console.ForegroundColor = fGp
            line = prompt(lineIndex)
            Console.WriteLine(line)
            Console.SetCursorPosition(cXp, lineIndex + 26)
        Next
        Console.SetCursorPosition(0, 0)

    End Sub

    Sub playerWon(ByVal PlayerCard As Object, ByVal OpponentCard As Object, ByVal PlayerSprite As String(), ByVal OpponentSprite As String())
        Dim winText As String() = {"  _                   _   _              _            ", " |_)  |    /\   \_/  |_  |_)    \    /  / \  |\ |    |", " |    |_  /--\   |   |_  | \     \/\/   \_/  | \|    o", "                                                      "}
        Dim fGw As ConsoleColor = ConsoleColor.Cyan
        Dim cXp As Integer = 34
        Dim line As String

        Console.Clear()
        displayCharAndStats(PlayerCard, OpponentCard, PlayerSprite, OpponentSprite)

        Console.SetCursorPosition(cXp, 25)
        For lineIndex = 0 To winText.Length - 1
            Console.ForegroundColor = fGw
            line = winText(lineIndex)
            Console.WriteLine(line)
            Console.SetCursorPosition(cXp, lineIndex + 26)
        Next
        Console.SetCursorPosition(0, 0)
        waitForEnter()
    End Sub

    Sub opponentWon(ByVal PlayerCard As Object, ByVal OpponentCard As Object, ByVal PlayerSprite As String(), ByVal OpponentSprite As String())
        Dim winText As String() = {"  _    _    _    _          _        ___             _             ", " / \  |_)  |_)  / \  |\ |  |_  |\ |   |     \    /  / \  |\ |    | ", " \_/  |    |    \_/  | \|  |_  | \|   |      \/\/   \_/  | \|    o"}
        Dim fGw As ConsoleColor = ConsoleColor.Magenta
        Dim cXp As Integer = 28
        Dim line As String

        Console.Clear()
        displayCharAndStats(PlayerCard, OpponentCard, PlayerSprite, OpponentSprite)

        Console.SetCursorPosition(cXp, 25)
        For lineIndex = 0 To winText.Length - 1
            Console.ForegroundColor = fGw
            line = winText(lineIndex)
            Console.WriteLine(line)
            Console.SetCursorPosition(cXp, lineIndex + 26)
        Next
        Console.SetCursorPosition(0, 0)
        waitForEnter()
    End Sub

    Sub showOpponentChoice(ByVal choice As String, ByVal PlayerCard As Object, ByVal OpponentCard As Object, ByVal PlayerSprite As String(), ByVal OpponentSprite As String())
        'Mini smushedU*2
        Dim attack As String() = {"  _. _|_ _|_  _.  _ |  ", " (_|  |_  |_ (_| (_ |<"}

        Dim defend As String() = {"  _       _            ", " | \  _ _|_ _  ._   _| ", " |_/ (/_ | (/_ | | (_| "}
        Dim heal As String() = {" |_|  _   _. | ", " | | (/_ (_| | "}
        Dim power As String() = {"  _                 ", " |_) _        _  ._ ", " |  (_) \/\/ (/_ | "}
        Dim preText As String() = {"  _                                                            ", " / \ ._  ._   _  ._   _  ._ _|_   _ |_   _   _  _   _|_  _  o  ", " \_/ |_) |_) (_) | | (/_ | | |_  (_ | | (_) _> (/_   |_ (_) o  ", "     |   |"}
        Dim fGp As ConsoleColor = ConsoleColor.Gray

        Dim cXp As Integer = 17
        Dim cY As Integer = 25
        Dim cYT As Integer = 26
        Dim cX As Integer = cXp + 63
        Dim line As String

        Console.Clear()
        displayCharAndStats(PlayerCard, OpponentCard, PlayerSprite, OpponentSprite)

        Console.SetCursorPosition(cXp, 25)
        For lineIndex = 0 To preText.Length - 1
            Console.ForegroundColor = fGp
            line = preText(lineIndex)
            Console.WriteLine(line)
            Console.SetCursorPosition(cXp, lineIndex + 26)
        Next
        Select Case choice
            Case "a"
                Console.SetCursorPosition(cX, cYT)
                For lineIndex = 0 To attack.Length - 1
                    Console.ForegroundColor = ConsoleColor.DarkRed
                    line = attack(lineIndex)
                    Console.WriteLine(line)
                    Console.SetCursorPosition(cX, lineIndex + 1 + cYT)
                Next
            Case "d"
                Console.SetCursorPosition(cX, cY)
                For lineIndex = 0 To defend.Length - 1
                    Console.ForegroundColor = ConsoleColor.White
                    line = defend(lineIndex)
                    Console.WriteLine(line)
                    Console.SetCursorPosition(cX, lineIndex + 1 + cY)
                Next

            Case "h"
                Console.SetCursorPosition(cX, cYT)
                For lineIndex = 0 To heal.Length - 1
                    Console.ForegroundColor = ConsoleColor.Green
                    line = heal(lineIndex)
                    Console.WriteLine(line)
                    Console.SetCursorPosition(cX, lineIndex + 1 + cYT)
                Next
            Case "p"
                Console.SetCursorPosition(cX, cYT)
                For lineIndex = 0 To power.Length - 1
                    Console.ForegroundColor = ConsoleColor.Blue
                    line = power(lineIndex)
                    Console.WriteLine(line)
                    Console.SetCursorPosition(cX, lineIndex + 1 + cY)
                Next
        End Select
        Console.ForegroundColor = ConsoleColor.White


        Console.SetCursorPosition(0, 0)
        waitForEnter()

    End Sub



    Sub showOpponent(ByVal opponent As Opponent)
        Dim title As String() = {" _     _                      _____                                            _   ", "( )   ( )                    (  _  )                                          ( )_ ", "`\`\_/'/'_    _   _  _ __    | ( ) | _ _    _ _      _     ___     __    ___  | ,_)", "  `\ /'/'_`\ ( ) ( )( '__)   | | | |( '_`\ ( '_`\  /'_`\ /' _ `\ /'__`\/' _ `\| |  ", "   | |( (_) )| (_) || |      | (_) || (_) )| (_) )( (_) )| ( ) |(  ___/| ( ) || |_ ", "   (_)`\___/'`\___/'(_)      (_____)| ,__/'| ,__/'`\___/'(_) (_)`\____)(_) (_)`\__)", "                                    | |    | |                                     ", "                                    (_)    (_)                                     "}
        Dim cX As Integer = 19
        Dim cY As Integer = 0
        Dim line As String
        Dim person As Integer = 0
        Dim oppCard As Object = opponent.cards(0)

        Select Case opponent.cards(0).Name
            Case "Basic Knight"
                person = 0
            Case "Trained Soldier"
                person = 1
            Case "Legendary Warrior"
                person = 2
        End Select

        Console.Clear()
        Console.SetCursorPosition(cX, cY)
        For lineIndex = 0 To title.Length - 1
            Console.ForegroundColor = ConsoleColor.Magenta
            line = title(lineIndex)
            Console.WriteLine(line)
            Console.SetCursorPosition(cX, lineIndex + 1 + cY)
        Next


        showPerson(person, ConsoleColor.White, opponent, True, 0, True, True)
        showVisualStats(oppCard, oppCard, True)


        Console.SetCursorPosition(0, 0)
        waitForEnter()

    End Sub
    'lins of code
    Sub showWarining(ByVal fg As ConsoleColor, ByVal Selected As Boolean, ByVal cX As Integer, ByVal cY As Integer, Optional ByVal disabled As Boolean = False)
        Dim arrowRight As String() = {"          _______   ___       _______   _______   _______   _______       _______   _______   _______   ______          ", "         |   _   | |   |     |   _   | |   _   | |   _   | |   _   |     |   _   \ |   _   | |   _   | |   _  \         ", "         |.  |   | |.  |     |.  |___| |.  |   | |   |___| |.  |___|     |.  l   / |.  |___| |.  |   | |.  |   \        ", "         |.  ____| |.  |___  |.  __)_  |.  _   | |____   | |.  __)_      |.  _   | |.  __)_  |.  _   | |.  |    \       ", "         |:  |     |:  |   | |:  |   | |:  |   | |:  |   | |:  |   |     |:  |   | |:  |   | |:  |   | |:  |    /       ", "         |::.|     |::.. . | |::.. . | |::.|:. | |::.. . | |::.. . |     |::.|:. | |::.. . | |::.|:. | |::.. . /        ", "         `---'     `-------' `-------' `--- ---' `-------' `-------'     `--- ---' `-------' `--- ---' `------'         ", "  _______   ___ ___   _______       _______   _______   ___       ___       _______   ___ ___   ___   ______    _______ ", " |       | |   Y   | |   _   |     |   _   | |   _   | |   |     |   |     |   _   | |   Y   | |   | |   _  \  |   _   |", " |.|   | | |.  |   | |.  |___|     |.  |___| |.  |   | |.  |     |.  |     |.  |   | |.  |   | |.  | |.  |   | |.  |___|", " `-|.  |-' |.  _   | |.  __)_      |.  __)   |.  |   | |.  |___  |.  |___  |.  |   | |. / \  | |.  | |.  |   | |.  |   |", "   |:  |   |:  |   | |:  |   |     |:  |     |:  |   | |:  |   | |:  |   | |:  |   | |:      | |:  | |:  |   | |:  |   |", "   |::.|   |::.|:. | |::.. . |     |::.|     |::.. . | |::.. . | |::.. . | |::.. . | |::.|:. | |::.| |::.|   | |::.. . |", "   `---'   `--- ---' `-------'     `---'     `-------' `-------' `-------' `-------' `--- ---' `---' `--- ---' `-------'", "                                                      __       __       __                                              ", "                                                     |  |     |  |     |  |                                             ", "                                                     |  |     |  |     |  |                                             ", "                                                     |  |     |  |     |  |                                             ", "                                                     |__|     |__|     |__|                                             ", "                                                     |__|     |__|     |__|                                              ", "                                                                                                                         ", "                                                                                                                         ", "                                                                                                                         ", "                                                                                                                         ", "                                                                                                                         ", "                                                                                                                         ", "                                                                                                                         ", "                                                                                                                         "}

        Dim line As String

        If disabled Then
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If


        Console.SetCursorPosition(cX, cY)
        For lineIndex = 0 To arrowRight.Length - 1
            Console.ForegroundColor = fg
            If disabled Then
                Console.ForegroundColor = ConsoleColor.DarkGray
            End If
            line = arrowRight(lineIndex)
            If lineIndex = arrowRight.Length - 2 Then
                Console.Write(line)
                Console.SetCursorPosition(cX, lineIndex + 1 + cY)
                Exit For
            End If
            Console.WriteLine(line)
            Console.SetCursorPosition(cX, lineIndex + 1 + cY)
        Next
    End Sub

    Sub showText(text)
        Console.SetCursorPosition(0, 0)
        Console.Clear()
        Console.WriteLine(text)
        waitForEnter()
    End Sub

    Sub tutorial()
        Console.Clear()

        Console.BackgroundColor = ConsoleColor.Black
        showWarining(ConsoleColor.Yellow, False, 0, 0)
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine("                                                    Press Space To Continue...                                                                                                                                                                  ")
        Console.BackgroundColor = ConsoleColor.Black


        waitForEnter()
        Console.Clear()
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine("BASIC CONTROLS
------------------------------------------------------------------------------------------------------------------------
-Press `enter` or `space` to select
-use arrow keys or `wasd` to navigate
------------------------------------------------------------------------------------------------------------------------
HOW TO PLAY THE GAME
------------------------------------------------------------------------------------------------------------------------
-The player starts with a set amount of coins
-You can use those coins to purchase 3 types of cards:
                -The Basic Knight
                                -Has the weakest strength but is the cheapest 
 
                -The Trained Soldier
                                -Has medium strength and price
 
                -The Legendary Warrior
                                -Has the highest strength but is the most expensive
 
-The player can purchase one of these cards (if they can afford it) and use it to fight against a computer 
 (in a turn based combat manner) that plays intelligently based on the players and its own stats.
 
-As the player kills more opponents, their coins will increase therefore allowing them to buy better, more powerful,
cards. However, the computer will also get better cards and will play smarter.
------------------------------------------------------------------------------------------------------------------------
THE AIM OF THE GAME
------------------------------------------------------------------------------------------------------------------------
-The aim of this game is to kill as many opponents as you can without running out of cards.
-Your high score will be saved, and your goal is to beat your own record.
------------------------------------------------------------------------------------------------------------------------")
        Console.SetCursorPosition(0, 0)


        waitForEnter()
        Console.Clear()
        Console.ForegroundColor = ConsoleColor.White
        Console.SetCursorPosition(0, 0)
        Console.WriteLine("HOW TO FIGHT OPPONENT
------------------------------------------------------------------------------------------------------------------------
READ THE ENTIERITY OF THIS TEXT OR YOU WILL NOT KNOW HOW TO PLAY THIS GAME EFFECTIVLY !!!
 
-Each player and opponent card will have 4 main values: defence value percent, attack value, heal value and health.
 
-When you fight an opponent, you have to either pick 1 of the 4 options: Attack, Defend, Heal, Power.
------------------------------------------------------------------------------------------------------------------------")
        Console.SetCursorPosition(0, 0)
        waitForEnter()
        Console.Clear()
        Console.WriteLine("HOW TO FIGHT OPPONENT
------------------------------------------------------------------------------------------------------------------------
-Attack: Attack lets you minus your attack value to the opponent's health. For example, if you are a basic 
 knight fighting another basic knight, and you chose to attack, you will minus 2 from the opponents 
 health as that is your attack value. So, the opponents health will be 3 as 5-2 = 3.
                
-Defence: Defence is the value of attack you will negate the next time your opponent chooses attack. So, in our 
 example if you chose defence then then next time the opponent attacks (it can be the move after yours or 
 later) the opponent will deal 1 damage instead of 2. This is because the defence value of your knight is 30% so
 the opponent's attack will get reduced by 30%.
 
-Heal: You can only use the heal move 5 times in a fight. When you use heal your health will increase by your 
 heal value. In our example say you have a health of 2 and you use heal, your health will increase to 4. This is 
 because your heal value(2)+your health(2) = 4.
 
-Power: Power is used to boost your 4 main stats for a short period of moves. Firstly, it changes your defence 
 value to 100% until your opponent attacks you (then your defence will revert original). Next, the power move 
 will boost your attack damage by 2 till you attack. After you use your 'super' attack your attack value will 
 revert back to your original. Finally, it will revert your health back to 50% if you are lower than that.
 You can only use your power once a fight and after a fight is over you WILL NOT get your power move back 
 for the next fight.")
        Console.SetCursorPosition(0, 0)
        waitForEnter()
        Console.Clear()

        Console.WriteLine("HOW TO FIGHT OPPONENT
------------------------------------------------------------------------------------------------------------------------
-If you lose to an opponent then next time you fight, the opponent’s health and stats will be saved so you can 
 carry on from where you left just with a different card.
 
-If you can't remember all this. DON'T PANIC. The defence, attack, health will all be displayed live when you 
 are fighting. This is helpful as you don't have to manually calculate each stat.
------------------------------------------------------------------------------------------------------------------------")

        waitForEnter()
        Console.Clear()
        Console.WriteLine("
ADDITIONAL NOTICES
------------------------------------------------------------------------------------------------------------------------
-This game is entirely mad in a console app. This means that all I can do is output text (letters and numbers) to the 
 console and delete the entire screen. That is why making game art, animation, UI (User interface), File Saving, 
 becomes incredibly difficult to create. I had to overcome many challenges and difficulties to make this game feel 
 smooth and run correctly so I would appreciate it if you played this game 
 with the mindset that this project is not supposed to be clean and functional but more of a challenge of how far I 
 can push the limits of a console application.
------------------------------------------------------------------------------------------------------------------------
CREDITS
------------------------------------------------------------------------------------------------------------------------
-When making the ascii art for this game I used 2 main resources: 
                -Text To ascii art -- https://patorjk.com/software/taag/#p=display&f=Graffiti&t=Type%20Something%20 
                -General ascii art images -- https://www.asciiart.eu/
------------------------------------------------------------------------------------------------------------------------
Development Journey
------------------------------------------------------------------------------------------------------------------------
-I made this game single handedly in 18 days.
-It took 40 hours if you calculate just the working hours. 
-There are around 3225 lines of code
-The whole process of the making of this game is documented in my YouTube channel
                -Timelapse of development -- https://www.youtube.com/watch?v=_MffU1I3DO0
------------------------------------------------------------------------------------------------------------------------
 
 
 
 
This game was Made by Prithiv Jith 8F - Hope you enjoy :)")
        waitForEnter()
    End Sub


    Function getRateKills()
        Dim file As New FileStream("highscrore.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite)
        Dim textS As String = ""
        Dim textB As String() = {}
        Dim text As New ArrayList()
        Dim Buffer As New ArrayList()
        Dim aBuffer As Object() = {}
        Dim stats As New ArrayList()
        Dim commaIndex As Integer = 0


        file.Seek(0, SeekOrigin.Begin)
        For itr = 0 To file.Length - 2
            textS += $"{file.ReadByte()},"
        Next
        textS += $"{file.ReadByte()}"
        textB = textS.Split(",")

        For Each b In textB
            text.Add(Chr(CInt(b)))
        Next

        For k As Integer = 0 To text.Count - 1
            If text(k) = "," Then
                Exit For
            End If
            commaIndex += 1
        Next

        For i As Integer = 0 To commaIndex - 1
            Buffer.Add(text(i))
        Next
        aBuffer = Buffer.ToArray()
        stats.Add(CInt(Join(aBuffer, "")))

        Buffer.Clear()
        For j As Integer = commaIndex + 1 To text.Count - 1
            Buffer.Add(text(j))
        Next

        aBuffer = Buffer.ToArray()
        stats.Add(CInt(Join(aBuffer, "")))



        file.Close()
        Return stats
    End Function
    Sub saveRateKills(ByVal player As Player)
        player.updateRating()

        Dim stats As Object = getRateKills()
        Dim file As New FileStream("highscrore.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite)
        Dim text As String = ""
        Dim rating As Boolean = stats(1) < player.rating
        Dim kills As Boolean = stats(0) < player.opponentsKilled

        ' Debug($"{rating},{kills}")

        If kills And Not rating Then
            text = $"{player.opponentsKilled},{stats(1)}"
        ElseIf Not kills And rating Then
            text = $"{stats(0)},{player.rating}"
        ElseIf kills And rating Then
            text = $"{player.opponentsKilled},{player.rating}"
        ElseIf Not kills And Not rating Then
            file.Close()
            Exit Sub
        End If



        file.SetLength(0)

        For Each ch In text
            file.WriteByte(CByte(Asc(ch)))
        Next

        file.Close()
    End Sub

    Sub gameOverText(ByVal fg As ConsoleColor, ByVal Selected As Boolean, ByVal cX As Integer, ByVal cY As Integer, Optional ByVal disabled As Boolean = False)
        Dim gameOverText As String() = {"  ▄████ ▄▄▄      ███▄ ▄███▓█████     ▒█████  ██▒   █▓█████ ██▀███  ", " ██▒ ▀█▒████▄   ▓██▒▀█▀ ██▓█   ▀    ▒██▒  ██▓██░   █▓█   ▀▓██ ▒ ██▒", "▒██░▄▄▄▒██  ▀█▄ ▓██    ▓██▒███      ▒██░  ██▒▓██  █▒▒███  ▓██ ░▄█ ▒", "░▓█  ██░██▄▄▄▄██▒██    ▒██▒▓█  ▄    ▒██   ██░ ▒██ █░▒▓█  ▄▒██▀▀█▄  ", "░▒▓███▀▒▓█   ▓██▒██▒   ░██░▒████▒   ░ ████▓▒░  ▒▀█░ ░▒████░██▓ ▒██▒", " ░▒   ▒ ▒▒   ▓▒█░ ▒░   ░  ░░ ▒░ ░   ░ ▒░▒░▒░   ░ ▐░ ░░ ▒░ ░ ▒▓ ░▒▓░", "  ░   ░  ▒   ▒▒ ░  ░      ░░ ░  ░     ░ ▒ ▒░   ░ ░░  ░ ░  ░ ░▒ ░ ▒░", "░ ░   ░  ░   ▒  ░      ░     ░      ░ ░ ░ ▒      ░░    ░    ░░   ░ ", "      ░      ░  ░      ░     ░  ░       ░ ░       ░    ░  ░  ░     ", "                                                 ░                 "}

        Dim line As String

        If disabled Then
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If



        Console.SetCursorPosition(cX, cY)
        For lineIndex = 0 To gameOverText.Length - 1
            Console.ForegroundColor = fg
            If disabled Then
                Console.ForegroundColor = ConsoleColor.DarkGray
            End If
            line = gameOverText(lineIndex)
            Console.WriteLine(line)
            Console.SetCursorPosition(cX, lineIndex + 1 + cY)
        Next
    End Sub
    Sub betterLuck(ByVal fg As ConsoleColor, ByVal Selected As Boolean, ByVal cX As Integer, ByVal cY As Integer, Optional ByVal disabled As Boolean = False)
        Dim betterluck As String() = {"  ____       _   _              _                _     ", " |  _ \     | | | |            | |              | |    ", " | |_) | ___| |_| |_ ___ _ __  | |    _   _  ___| | __ ", " |  _ < / _ | __| __/ _ | '__| | |   | | | |/ __| |/ / ", " | |_) |  __| |_| ||  __| |    | |___| |_| | (__|   <  ", " |____/ \___|\__|\__\___|_|    |______\__,_|\___|_|\_\"}

        Dim line As String

        If disabled Then
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If



        Console.SetCursorPosition(cX, cY)
        For lineIndex = 0 To betterluck.Length - 1
            Console.ForegroundColor = fg
            If disabled Then
                Console.ForegroundColor = ConsoleColor.DarkGray
            End If
            line = betterluck(lineIndex)
            Console.WriteLine(line)
            Console.SetCursorPosition(cX, lineIndex + 1 + cY)
        Next
    End Sub

    Sub nextTime(ByVal fg As ConsoleColor, ByVal Selected As Boolean, ByVal cX As Integer, ByVal cY As Integer, Optional ByVal disabled As Boolean = False)
        Dim nextTime As String() = {"  _   _           _     _______ _                  _ ", " | \ | |         | |   |__   __(_)                | |", " |  \| | _____  _| |_     | |   _ _ __ ___   ___  | |", " | . ` |/ _ \ \/ | __|    | |  | | '_ ` _ \ / _ \ | |", " | |\  |  __/>  <| |_     | |  | | | | | | |  __/ |_|", " |_| \_|\___/_/\_\\__|    |_|  |_|_| |_| |_|\___| (_)"}
        Dim line As String

        If disabled Then
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If



        Console.SetCursorPosition(cX, cY)
        For lineIndex = 0 To nextTime.Length - 1
            Console.ForegroundColor = fg
            If disabled Then
                Console.ForegroundColor = ConsoleColor.DarkGray
            End If
            line = nextTime(lineIndex)
            Console.WriteLine(line)
            Console.SetCursorPosition(cX, lineIndex + 1 + cY)
        Next
    End Sub

    Sub GameOver(ByVal player As Player)
        Dim xOffset As Integer = 5
        Console.Clear()
        gameOverText(ConsoleColor.DarkRed, False, 26, 0)

        betterLuck(ConsoleColor.Cyan, False, 0 + xOffset, 7)
        nextTime(ConsoleColor.Cyan, False, 57 + xOffset, 7)
        Console.SetCursorPosition(0, Console.CursorTop + 2)
        Console.ForegroundColor = ConsoleColor.DarkYellow
        Console.WriteLine("-------------------------------------------------------  Score  --------------------------------------------------------")

        Console.WriteLine()
        Console.WriteLine()
        Console.Write("   ")
        showkills(player)
        Console.WriteLine()
        Console.WriteLine()
        Console.SetCursorPosition(0, Console.CursorTop)
        Console.ForegroundColor = ConsoleColor.DarkYellow
        Console.WriteLine("-----------------------------------------------------  High Score  -----------------------------------------------------")
        Console.WriteLine()
        Console.WriteLine()
        Console.Write("   ")
        showkills(player, True)
        Console.WriteLine()
        Console.WriteLine()

        Console.SetCursorPosition(0, Console.CursorTop)
        Console.ForegroundColor = ConsoleColor.DarkYellow
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------------")

        Console.ForegroundColor = ConsoleColor.Yellow
        Console.SetCursorPosition(51, Console.CursorTop)
        Console.WriteLine("Made By Prithiv 8F")

        waitForEnter()
        End
    End Sub

    Sub checkEmpty()
        Dim file As New FileStream("highscrore.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite)
        If file.Length = 0 Then
            For Each ch In "0,0"
                file.WriteByte(CByte(Asc(ch)))
            Next
        End If
        file.Close()
    End Sub

    Sub Main()
        Const title As String = "       █     █░ ▄▄▄       ██▀███       ██████  ██▓ ███▄ ▄███▓ █    ██  ██▓    ▄▄▄     ▄▄▄█████▓ ▒█████   ██▀███  
       ▓█░ █ ░█░▒████▄    ▓██ ▒ ██▒   ▒██    ▒ ▓██▒▓██▒▀█▀ ██▒ ██  ▓██▒▓██▒   ▒████▄   ▓  ██▒ ▓▒▒██▒  ██▒▓██ ▒ ██▒
       ▒█░ █ ░█ ▒██  ▀█▄  ▓██ ░▄█ ▒   ░ ▓██▄   ▒██▒▓██    ▓██░▓██  ▒██░▒██░   ▒██  ▀█▄ ▒ ▓██░ ▒░▒██░  ██▒▓██ ░▄█ ▒
       ░█░ █ ░█ ░██▄▄▄▄██ ▒██▀▀█▄       ▒   ██▒░██░▒██    ▒██ ▓▓█  ░██░▒██░   ░██▄▄▄▄██░ ▓██▓ ░ ▒██   ██░▒██▀▀█▄  
       ░░██▒██▓  ▓█   ▓██▒░██▓ ▒██▒   ▒██████▒▒░██░▒██▒   ░██▒▒▒█████▓ ░██████▒▓█   ▓██▒ ▒██▒ ░ ░ ████▓▒░░██▓ ▒██▒
       ░ ▓░▒ ▒   ▒▒   ▓▒█░░ ▒▓ ░▒▓░   ▒ ▒▓▒ ▒ ░░▓  ░ ▒░   ░  ░░▒▓▒ ▒ ▒ ░ ▒░▓  ░▒▒   ▓▒█░ ▒ ░░   ░ ▒░▒░▒░ ░ ▒▓ ░▒▓░
         ▒ ░ ░    ▒   ▒▒ ░  ░▒ ░ ▒░   ░ ░▒  ░ ░ ▒ ░░  ░      ░░░▒░ ░ ░ ░ ░ ▒  ░ ▒   ▒▒ ░   ░      ░ ▒ ▒░   ░▒ ░ ▒░
         ░   ░    ░   ▒     ░░   ░    ░  ░  ░   ▒ ░░      ░    ░░░ ░ ░   ░ ░    ░   ▒    ░      ░ ░ ░ ▒    ░░   ░ 
           ░          ░  ░   ░              ░   ░         ░      ░         ░  ░     ░  ░            ░ ░     ░     "
        Dim player1 As New Player()
        Dim player2 As New Player()

        Dim Opp1 As New Opponent()

        Dim playerCard As Object
        Dim selectedIndex As Integer

        Dim player2PlayerCard As Object
        Dim player2SelectedIndex As Integer

        Dim isComputer As Boolean


        checkEmpty() 'populates the text file that saves high scores if empty

        Console.CursorVisible = False

        startMenu(title) 'displays title
        'tutorial()
        Console.Clear()
        isComputer = numberOfPlayers(title)
        ' checks if player cant buy any cards and game over if so
        If player1.coins < New BasicKnight().Price And player1.cards.Count < 1 Then
            GameOver(player1)
        End If


        'opens menu that has option to shop and to continue to fight oponent.
        fightShop(title, player1, yOverride:=True, isComputer:=isComputer, turn:=0)
        If Not isComputer Then
            fightShop(title, player2, yOverride:=True, isComputer:=isComputer, turn:=1)
        End If

        'updates the player rating
        player1.updateRating()
        player2.updateRating()

        'if opnent has been defeated aldready add a card

        If Opp1.cards.Count < 1 And isComputer Then
            Opp1.addRandomCard(player1.rating, player1)
        End If

        'displays opponent and asks user to select their card
        If isComputer Then
            showOpponent(Opp1)
        End If

        selectedIndex = selectCard(player1, isComputer, turn:=0)
        playerCard = player1.cards(selectedIndex)
        If Not isComputer Then
            player2SelectedIndex = selectCard(player2, isComputer, turn:=1)
            player2PlayerCard = player2.cards(player2SelectedIndex)
        End If


        While True

            'fightScreen(player1, Opp1, PlayerCardIndex:=selectedIndex, isComputer:=isComputer)
            If isComputer Then
                player1.fight(Opp1, player1, isComputer, selectedIndex)
            Else
                player1.fight(player2, player1, isComputer, selectedIndex, player2SelectedIndex)

            End If


            If player1.coins < New BasicKnight().Price And player1.cards.Count < 1 Then
                GameOver(player1)
                Exit While
            End If


            fightShop(title, player1, isComputer:=isComputer, turn:=0)
            If Not isComputer Then
                fightShop(title, player2, isComputer:=isComputer, turn:=1)
            End If

            player1.updateRating()
            player2.updateRating()

            If Opp1.cards.Count < 1 And isComputer Then
                Opp1.addRandomCard(player1.rating, player1)
            End If

            If isComputer Then
                showOpponent(Opp1)
            End If

            selectedIndex = selectCard(player1, isComputer:=isComputer, turn:=0)
            playerCard = player1.cards(selectedIndex)
            If Not isComputer Then
                player2SelectedIndex = selectCard(player2, isComputer:=isComputer, turn:=1)
                player2PlayerCard = player2.cards(player2SelectedIndex)
            End If
        End While
        waitForEnter()
    End Sub

    'n = """x""".split("\n")
    'for i in n[:-1]:
    '   print(f"\"{i}\", ", end="")
    '   print(f"\"{n[-1]}\"", end="")

End Module
